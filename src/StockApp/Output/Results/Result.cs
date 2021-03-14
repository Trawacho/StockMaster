using StockApp.BaseClasses;
using StockApp.Converters;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;

namespace StockApp.Output.Results
{
    public class Result
    {
        private FixedDocument document;
        private readonly Tournament tournament;

        public Result(Tournament tournament)
        {
            this.tournament = tournament;
        }


        public FixedDocument CreateResult(Size pageSize)
        {
            document = new FixedDocument();
            document.DocumentPaginator.PageSize = pageSize;

            //Ergebnisliste - Überschriften
            var ergListe = new ucResult();
            ergListe._ArtDesWettbewerbs.Content = tournament.TournamentName;
            ergListe._Austragungsort.Content = tournament.Venue;
            ergListe._Durchführer.Content = tournament.Operator;
            ergListe._Veranstalter.Content = tournament.Organizer;
            ergListe._Datum.Content = tournament.DateOfTournament.ToString("dddd, dd.MM.yyyy");


            var rankedTeams = tournament.GetTeamsRanked();
            int r = 1;
            
            foreach (var team in rankedTeams)
            {
                var spT = new ucStackPanelTeam(r, team, (r <= tournament.NumberOfTeamsWithNamedPlayerOnResult));
                r++;
                ergListe._spTeams.Children.Add(spT);
            }

            //Eintragen der Offiziellen
            if (string.IsNullOrWhiteSpace(tournament.Referee.Name)){
                ergListe._stackPanelSchiedsrichter.Visibility = Visibility.Collapsed;
            }
            ergListe._textblockSchiedsrichterName.Text = tournament.Referee.Name;
            ergListe._textblockSchiedsrichterClub.Text = tournament.Referee.ClubName;

            if (string.IsNullOrWhiteSpace(tournament.CompetitionManager.Name))
            {
                ergListe._stackPanelWettbewerbsleiter.Visibility = Visibility.Collapsed;
            }
            ergListe._textblockWettbewerbsleiterName.Text = tournament.CompetitionManager.Name;
            ergListe._textblockWettbewerbsleiterClub.Text = tournament.CompetitionManager.ClubName;

            if (string.IsNullOrWhiteSpace(tournament.ComputingOfficer.Name))
            {
                ergListe._stackPanelRechenbüro.Visibility = Visibility.Collapsed;
            }
            ergListe._textblockRechenbüroName.Text = tournament.ComputingOfficer.Name;
            ergListe._textblockRechenbüroClub.Text = tournament.ComputingOfficer.ClubName;

            SetPagePanelToDocument(ergListe);
            return document;

            
        }


        private void SetPagePanelToDocument(Control control)
        {
            var newPage = Helpers.GetNewPage(document.DocumentPaginator.PageSize);
            newPage.HorizontalAlignment = HorizontalAlignment.Center;
            newPage.VerticalAlignment = VerticalAlignment.Center;

            //panel.HorizontalAlignment = HorizontalAlignment.Center;
            //panel.VerticalAlignment = VerticalAlignment.Center;

            FixedPage.SetTop(control, PixelConverter.CmToPx(1));
            FixedPage.SetLeft(control, PixelConverter.CmToPx(0.7));
            newPage.Children.Add(control);

            PageContent content = new PageContent();
            ((IAddChild)content).AddChild(newPage);
            document.Pages.Add(content);
        }




    }
}
