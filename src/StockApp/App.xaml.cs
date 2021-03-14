﻿using StockApp.Dialogs;
using StockApp.ViewModels;
using StockApp.Views;
using System;
using System.Windows;

namespace StockApp
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IDialogService dialogService = new DialogService(MainWindow);
            dialogService.Register<LiveResultViewModel, LiveResultView>();

            var viewModel = new MainViewModel(dialogService);
            var view = new MainWindow()
            {
                DataContext = viewModel
            };
            viewModel.ExitApplicationAction = new Action(view.Close);

            view.ShowDialog();
        }
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Application.Current.Shutdown(0);
        }
    }
}
