using StockMaster.BaseClasses;
using StockMaster.Commands;
using StockMaster.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace StockMaster.Models
{
    public class LiveResultViewModel :  TBaseClass , IDialogRequestClose
    {
        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;


        readonly Tournament tournament;

        public LiveResultViewModel(Tournament tournament)
        {
            this.tournament = tournament;

            OkCommand = new RelayCommand(
                (p) =>
                {
                    CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
                },
                (p) => true);

            CancelCommand = new RelayCommand(
               (p) => { CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)); },
               (p) => true);

            tournament.PropertyChanged += Tournament_PropertyChanged;
        }

        private void Tournament_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(Ergebnisliste));
        }

        
        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }


        public ObservableCollection<(int Platzierung, Team Team)> Ergebnisliste
        {
            get
            {
                var liste = new ObservableCollection<(int _platzierung, Team _team)>();
                int i = 1;
                foreach (var t in tournament.GetTeamsRanked())
                {
                    liste.Add((i, t));
                    i++;
                }
                return liste;
            }
        }
    }
}
