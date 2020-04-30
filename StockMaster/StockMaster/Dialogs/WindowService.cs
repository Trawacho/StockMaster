using System;
using System.Collections.Generic;
using System.Windows;

namespace StockMaster.Dialogs
{
    public class WindowService : IWindowService
    {
        private readonly Window owner;
        public IDictionary<Type, Type> Mappings { get; }
        public WindowService(Window owner)
        {
            this.owner = owner;
            Mappings = new Dictionary<Type, Type>();
        }

        public void Register<TViewModel, TView>()
                                                where TViewModel : IDialogRequestClose
                                                where TView : IWindow
        {
            if (Mappings.ContainsKey(typeof(TViewModel)))
            {
                throw new ArgumentException($"$Type {typeof(TViewModel)} is already mapped to type {typeof(TView)}");
            }

            Mappings.Add(typeof(TViewModel), typeof(TView));
        }

        public void Show<TViewModel>(TViewModel viewModel) where TViewModel : IDialogRequestClose
        {
            Type viewType = Mappings[typeof(TViewModel)];

            IWindow window = (IWindow)Activator.CreateInstance(viewType);

            EventHandler<DialogCloseRequestedEventArgs> handler = null;

            handler = (sender, e) =>
            {
                viewModel.CloseRequested -= handler;
                if (e.DialogResult.HasValue && e.DialogResult == true)
                {
                    window.Close();
                }
                else
                {
                    window.Close();
                }
            };

            viewModel.CloseRequested += handler;
            window.DataContext = viewModel;
            window.Owner = owner;

            window.Show();
        }
    }
}
