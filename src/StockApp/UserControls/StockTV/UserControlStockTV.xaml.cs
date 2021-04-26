using System.Windows;
using System.Windows.Controls;

namespace StockApp.UserControls.StockTV
{
    /// <summary>
    /// Interaction logic for UserControlStockTV.xaml
    /// </summary>
    public partial class UserControlStockTV : UserControl
    {
        public UserControlStockTV()
        {
            InitializeComponent();
        }

        public BaseClasses.StockTV StockTV
        {
            get { return (BaseClasses.StockTV)GetValue(StockTVProperty); }
            set { SetValue(StockTVProperty, value); }
        }

        public static readonly DependencyProperty StockTVProperty = DependencyProperty.Register(
            "StockTV", typeof(BaseClasses.StockTV), typeof(UserControlStockTV), new PropertyMetadata(null, StockTVChanged));

        private static void StockTVChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
