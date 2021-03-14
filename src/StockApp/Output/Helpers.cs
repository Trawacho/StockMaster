using System.Windows;
using System.Windows.Documents;

namespace StockApp.Output
{
    internal static class Helpers
    {
        internal static FixedPage GetNewPage(Size pageSize)
        {
            FixedPage page = new FixedPage
            {
                Width = pageSize.Width,
                Height = pageSize.Height
            };
            return page;
        }
    }
}
