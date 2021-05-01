using StockApp.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StockApp.UserControls.StockTV
{
    /// <summary>
    /// Interaction logic for UserControlStockTV.xaml
    /// </summary>
    public partial class UserControlStockTV : UserControl, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected void RaiseAllPropertiesChanged()
        {
            foreach (var p in this.GetType().GetProperties())
            {
                RaisePropertyChanged(p.Name);
            }
        }
        #endregion

        #region Konstruktor
        public UserControlStockTV()
        {
            InitializeComponent();
        }
        #endregion

        #region DependencyProperty StockTV

        public static readonly DependencyProperty StockTVProperty = DependencyProperty.Register(
          "StockTV", typeof(BaseClasses.StockTV), typeof(UserControlStockTV), new PropertyMetadata(null, StockTVChanged));
        public BaseClasses.StockTV StockTV
        {
            get { return (BaseClasses.StockTV)GetValue(StockTVProperty); }
            set { SetValue(StockTVProperty, value); }
        }

        #endregion

        public List<BaseClasses.ColorModis> ColorModis
        {
            get => Enum.GetValues(typeof(BaseClasses.ColorModis)).Cast<BaseClasses.ColorModis>().ToList();
        }

        public List<BaseClasses.GameModis> GameModis
        {
            get => Enum.GetValues(typeof(BaseClasses.GameModis)).Cast<BaseClasses.GameModis>().ToList();
        }

        public List<BaseClasses.NextBahnModis> NextBahnModis
        {
            get => Enum.GetValues(typeof(BaseClasses.NextBahnModis)).Cast<BaseClasses.NextBahnModis>().ToList();
        }


        #region Properties

        public int BahnNummer
        {
            get => StockTV?.TVSettings.Bahn ?? -2;
            set
            {
                StockTV.TVSettings.Bahn = value;
                RaisePropertyChanged();
            }
        }

        public int PunkteProKehre
        {
            get => StockTV?.TVSettings.PointsPerTurn ?? -2;
            set
            {
                StockTV.TVSettings.PointsPerTurn = value;
                RaisePropertyChanged();
            }
        }

        public int KehrenProSpiel
        {
            get => StockTV?.TVSettings.TurnsPerGame ?? -2;
            set
            {
                StockTV.TVSettings.TurnsPerGame = value;
                RaisePropertyChanged();
            }
        }

        public BaseClasses.GameModis GameModus
        {
            get => StockTV?.TVSettings.GameModus ?? BaseClasses.GameModis.Training;
            set
            {
                StockTV.TVSettings.GameModus = value;
                RaisePropertyChanged();
            }
        }

        public BaseClasses.ColorModis ColorScheme
        {
            get => StockTV?.TVSettings.ColorScheme ?? BaseClasses.ColorModis.Normal;
            set
            {
                StockTV.TVSettings.ColorScheme = value;
                RaisePropertyChanged();
            }
        }

        public BaseClasses.NextBahnModis NextBahnModus
        {
            get => StockTV?.TVSettings.NextBahnModus ?? BaseClasses.NextBahnModis.Links;
            set
            {
                StockTV.TVSettings.NextBahnModus = value;
                RaisePropertyChanged();
            }
        }


        public bool IsOnline { get => StockTV?.IsOnline ?? false; }

        #endregion

        #region Events

        private void StockTV_StockTVSettingsChanged(object sender, BaseClasses.StockTVSettingsChangedEventArgs stockTVSettings)
        {
            RaiseAllPropertiesChanged();
        }

        private static void StockTVChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is BaseClasses.StockTV tv)
            {
                tv.StockTVSettingsChanged += (o as UserControlStockTV).StockTV_StockTVSettingsChanged;
                tv.StockTVOnlineChanged += (o as UserControlStockTV).StockTV_StockTVOnlineChanged;
                (o as UserControlStockTV).RaiseAllPropertiesChanged();
            }
        }

        private void StockTV_StockTVOnlineChanged(object sender, bool IsOnline)
        {
            RaisePropertyChanged(nameof(this.IsOnline));
        }

        #endregion

        #region Commands

        private ICommand _sendTVSettingsCommand;
        public ICommand SendTVSettingsCommand
        {
            get
            {
                return _sendTVSettingsCommand ??= new RelayCommand(
                    (p) =>
                    {
                        StockTV.TVSettingsSend();
                    },
                    (p) =>
                    {
                        return true;
                    });
            }
        }

        private ICommand _getTVSettingsCommand;
        public ICommand GetTVSettingsCommand
        {
            get
            {
                return _getTVSettingsCommand ??= new RelayCommand(
                    (p) =>
                    {
                        StockTV.TVSettingsGet();
                    },
                    (p) =>
                    {
                        return true;
                    });
            }
        }

        #endregion

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //http://192.168.100.136:8080/#Apps%20manager
            if (e.ClickCount == 2)
                System.Diagnostics.Process.Start($"http://{StockTV.IPAddress}:8080/#Apps%20manager");

        }
    }

}
