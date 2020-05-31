using StockMaster.BaseClasses;
using StockMaster.Converters;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;

namespace StockMaster.Output.Receipts
{
    public class Receipt
    {
        FixedDocument document;
        private readonly Tournament tournament;
        public Receipt(Tournament tournament)
        {
            this.tournament = tournament;
        }

        public FixedDocument CreateReceipts(Size pageSize)
        {
            document = new FixedDocument();
            document.DocumentPaginator.PageSize = pageSize;


            var receipts = new List<Control>();

            foreach (var team in tournament.Teams.Where(t => !t.IsVirtual))
            {
                var receipt = new ucReceipt();
                receipt.labelAn.Content = tournament.Organizer;
                receipt.labelVon.Content = team.TeamName;
                receipt.labelEUR.Content = tournament.EntryFee.Value.ToString("C");
                receipt.labelVerbal.Content = $"-- {tournament.EntryFee.Verbal} --";
                receipt.labelZweck.Content = tournament.TournamentName;
                receipt.labelOrtDatum.Content = tournament.Venue + ", " + tournament.DateOfTournament.ToString("dd.MM.yyyy");

                receipts.Add(receipt);
            }


            var pagePanel = new StackPanel();

            foreach (var r in receipts)
            {

                pagePanel.Children.Add(r);
                pagePanel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                pagePanel.Arrange(new Rect(0, 0, pagePanel.DesiredSize.Width, pagePanel.DesiredSize.Height));

                if (pagePanel.ActualHeight + r.ActualHeight > document.DocumentPaginator.PageSize.Height)
                {
                    SetPagePanelToDocument(pagePanel);
                    pagePanel = new StackPanel();
                }
            }
            if (pagePanel.Children.Count > 0)
            {
                SetPagePanelToDocument(pagePanel);
            }

            return document;
        }

        private void SetPagePanelToDocument(StackPanel panel)
        {
            var newPage = Helpers.GetNewPage(document.DocumentPaginator.PageSize);
            newPage.HorizontalAlignment = HorizontalAlignment.Center;
            newPage.VerticalAlignment = VerticalAlignment.Center;

            panel.HorizontalAlignment = HorizontalAlignment.Center;
            panel.VerticalAlignment = VerticalAlignment.Center;
            
            FixedPage.SetTop(panel, PixelConverter.CmToPx(1));
            FixedPage.SetLeft(panel, PixelConverter.CmToPx(0.7));
            newPage.Children.Add(panel);

            PageContent content = new PageContent();
            ((IAddChild)content).AddChild(newPage);
            document.Pages.Add(content);
        }

        
    }
}
