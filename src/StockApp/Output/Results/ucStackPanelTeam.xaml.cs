using StockApp.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StockApp.Output.Results
{
    /// <summary>
    /// Interaction logic for ucStackPanelTeam.xaml
    /// </summary>
    public partial class ucStackPanelTeam : UserControl
    {
        private readonly Team team;
        private bool printSpieler;
        public ucStackPanelTeam(int rank, Team team, bool printSpieler = false)
        {

            this.Rank = rank;
            this.team = team;
            this.printSpieler = printSpieler;

            InitializeComponent();
        }

        public int Rank { get; private set; }
        public string TeamName
        {
            get
            {
                return team?.TeamName ?? "";
            }
        }

        public string SpielPunkte
        {
            get
            {
                return team?.SpielPunkteString ?? "";
            }
        }

        public string StockPunkte
        {
            get
            {
                return team?.StockPunkteString ?? "";
            }
        }

        public string StockNote
        {
            get
            {
                return team?.StockNote.ToString("F3") ?? "";
            }
        }

        public string SpielerListe
        {
            get
            {
                if (printSpieler)
                {
                    string t = string.Empty;
                    foreach (var s in team.Players)
                    {
                        t += (string.IsNullOrWhiteSpace(t))
                            ? $"{s.LastName} {s.FirstName}"
                            : $", {s.LastName} {s.FirstName}";
                    }
                    return t;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
