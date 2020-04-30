using StockMaster.Dialogs;
using StockMaster.Models;
using StockMaster.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace StockMaster
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //IDialogService dialogService = new DialogService(MainWindow);
            //dialogService.Register<LiveResultViewModel, LiveResultView>();

            IWindowService windowServie = new WindowService(MainWindow);
            windowServie.Register<LiveResultViewModel, LiveResultView>();

            var viewModel = new MainViewModel(windowServie);
            var view = new MainWindow() { DataContext = viewModel };
            
            view.ShowDialog();
            
            // https://www.youtube.com/watch?v=OqKaV4d4PXg
        }
    }
}
