using StockMaster.BaseClasses;
using StockMaster.Commands;
using StockMaster.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace StockMaster.ViewModels
{
    /// <summary>
    /// file contains two classes and a interface
    /// - Interface
    /// - ViewModel
    /// - ViewModel for Design
    /// </summary>
    public interface ILiveResultViewModel
    {
        ObservableCollection<(int Platzierung, Team Team)> Ergebnisliste { get; }
    }


    public class LiveResultViewModel : BaseViewModel, IDialogRequestClose, ILiveResultViewModel
    {
        public event EventHandler<WindowCloseRequestedEventArgs> WindowCloseRequested;
        public event EventHandler<DialogCloseRequestedEventArgs> DialogCloseRequested;

        readonly Tournament tournament;

        public LiveResultViewModel(Tournament tournament)
        {
            this.tournament = tournament;
            tournament.PropertyChanged += Tournament_PropertyChanged;
        }

        private void Tournament_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(Ergebnisliste));
        }


        private ICommand _closeCommand;
        public ICommand CloseCommand
        {
            get
            {

                return _closeCommand ?? (_closeCommand = new RelayCommand(
                    (p) =>
                    {
                        WindowCloseRequested?.Invoke(this, new WindowCloseRequestedEventArgs());
                        DialogCloseRequested?.Invoke(null, null);
                    },
                    (p) => true));
            }
        }


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

    public class LiveResultDesignViewModel : ILiveResultViewModel
    {
        public LiveResultDesignViewModel()
        {
            this.Ergebnisliste = new ObservableCollection<(int, Team)>
            {
                (1, new Team("ESF Hankofen")),
                (2, new Team("TV Geiselhöring")),
                (3, new Team("EC EBRA Aiterhofen")),
                (4, new Team("EC Pilsting")),
                (5, new Team("DJK Leiblfing")),
                (6, new Team("EC Welchenberg")),
                (7, new Team("SV Salching")),
                (8, new Team("EC Straßkirchen")),
                (9, new Team("DJK Aigen Am Inn"))
            };
        }


        public ObservableCollection<(int Platzierung, Team Team)> Ergebnisliste
        {
            get;
        }
    }


}
