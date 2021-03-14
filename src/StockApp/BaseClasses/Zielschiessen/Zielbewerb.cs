﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace StockApp.BaseClasses.Zielschiessen
{
    public class Zielbewerb : TBaseBewerb
    {
        private readonly List<Teilnehmer> _teilnehmerliste;
        private readonly List<int> _zielBahnen;

        public Zielbewerb()
        {
            this._teilnehmerliste = new List<Teilnehmer>();
            this.AddNewTeilnehmer();

            this._zielBahnen = new List<int>();

            for (int i = 1; i <= 15; i++)
            {
                this._zielBahnen.Add(i);
            }
        }

        /// <summary>
        /// ReadOnly Liste aller Teilnehmer
        /// </summary>
        public IOrderedEnumerable<Teilnehmer> Teilnehmerliste
        {
            get
            {
                return this._teilnehmerliste.OrderBy(t => t.Startnummer);
            }
        }

        /// <summary>
        /// Auflistung aller Bahnen (15 Bahnen werden automatisch ergzeugt)
        /// </summary>
        public IOrderedEnumerable<int> Bahnen
        {
            get
            {
                return this._zielBahnen.OrderBy(b => b);
            }
        }

        /// <summary>
        /// Auflistung aller freier Bahnen
        /// </summary>
        public IOrderedEnumerable<int> FreieBahnen
        {
            get
            {
                var belegteBahnen = Teilnehmerliste.Where(t => t.AktuelleBahn > 0).Select(b => b.AktuelleBahn);
                return Bahnen.Where(b => !belegteBahnen.Contains(b)).OrderBy(x => x);
            }
        }

        /// <summary>
        /// Wird ausgeführt, wenn sich der Status <see cref="Teilnehmer.HasOnlineWertung"/> bei einem <see cref="Teilnehmer"/> ändert.
        /// <br>Startet den <see cref="NetworkService"/> wenn der erste <see cref="Teilnehmer"/> online geht</br>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TeilnehmerHasOnlineWertungChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(Teilnehmer.HasOnlineWertung)) return;

            if (Teilnehmerliste.Any(t => t.HasOnlineWertung) && !NetworkService.Instance.IsRunning())
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("NetworkService starten, da mindestens eine Wertung Online ist");
#endif
                NetworkService.Instance.Start(this);
            }

            RaisePropertyChanged(nameof(FreieBahnen));
        }

        /// <summary>
        /// Ein neuer Teilnehmer wird der Liste hinten hinzugefügt. 
        /// </summary>
        internal void AddNewTeilnehmer()
        {
            var teilnehmer = new Teilnehmer();
            teilnehmer.PropertyChanged += TeilnehmerHasOnlineWertungChanged;
            teilnehmer.Startnummer = this._teilnehmerliste.Count + 1;
            this._teilnehmerliste.Add(teilnehmer);
            RaisePropertyChanged(nameof(this.Teilnehmerliste));
        }

        /// <summary>
        /// True, wenn die Teilnehmerliste nicht voll ist (<= 30)
        /// </summary>
        /// <returns></returns>
        internal bool CanAddTeilnehmer()
        {
            return _teilnehmerliste.Count() <= 30;
        }

        /// <summary>
        /// Der Teilnehmer wird aus der Liste entfernt
        /// </summary>
        /// <param name="teilnehmer"></param>
        internal void RemoveTeilnehmer(Teilnehmer teilnehmer)
        {
            this._teilnehmerliste.Remove(teilnehmer);
            for (int i = 0; i < _teilnehmerliste.Count; i++)
            {
                _teilnehmerliste[i].Startnummer = i + 1;
            }
            RaisePropertyChanged(nameof(this.Teilnehmerliste));
        }

        /// <summary>
        /// True, solange die Anzahl der Teilnehmeer > 1 ist
        /// </summary>
        /// <returns></returns>
        internal bool CanRemoveTeilnehmer()
        {
            return _teilnehmerliste.Count() > 1;
        }

        internal void MoveTeilnehmer(int oldIndex, int newIndex)
        {
            var teilnehmer = _teilnehmerliste[oldIndex];

            if (newIndex < 0)
            {
                throw new ArgumentOutOfRangeException("neue Startnummer darf nicht kleiner 1 sein");
            }

            _teilnehmerliste.RemoveAt(oldIndex);
            _teilnehmerliste.Insert(newIndex, teilnehmer);

            for (int i = 0; i < _teilnehmerliste.Count; i++)
            {
                _teilnehmerliste[i].Startnummer = i + 1;
            }

            RaisePropertyChanged(nameof(this.Teilnehmerliste));
        }

    }
}
