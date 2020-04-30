using System.Windows;

namespace StockMaster.Dialogs
{
    public interface IWindow
    {
        object DataContext { get; set; }
        bool? DialogResult { get; set; }
        Window Owner { get; set; }
        void Close();
        void Show();
    }
}
