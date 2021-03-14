using System;
using System.Collections.Generic;
using System.Windows;

namespace StockMaster.Dialogs
{
    public class DialogService : IDialogService
    {
        private  Window owner;
        public DialogService(Window owner)
        {
            this.owner = owner;
            Mappings = new Dictionary<Type, Type>();
        }

        public void SetOwner(Window owner)
        {
            this.owner = owner;
        }

        public IDictionary<Type, Type> Mappings { get; }
        public void Register<TViewModel, TView>()
                                            where TViewModel : IDialogRequestClose
                                            where TView : IDialog
        {
            if (Mappings.ContainsKey(typeof(TViewModel)))
            {
                throw new ArgumentException($"$Type {typeof(TViewModel)} is already mapped to type {typeof(TView)}");
            }

            Mappings.Add(typeof(TViewModel), typeof(TView));
        }

        public bool? ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : IDialogRequestClose
        {
            Type viewType = Mappings[typeof(TViewModel)];
            IDialog dialog = (IDialog)Activator.CreateInstance(viewType);
            EventHandler<DialogCloseRequestedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                viewModel.DialogCloseRequested -= handler;
                if (e.DialogResult.HasValue)
                {
                    dialog.DialogResult = e.DialogResult;
                }
                else
                {
                    dialog.Close();
                }
            };

            viewModel.DialogCloseRequested += handler;
            dialog.DataContext = viewModel;
            dialog.Owner = owner;

            return dialog.ShowDialog();

        }

        public void Show<TViewModel>(TViewModel viewModel) where TViewModel : IDialogRequestClose
        {
            Type viewType = Mappings[typeof(TViewModel)];
            IDialog dialog = (IDialog)Activator.CreateInstance(viewType);
            EventHandler<WindowCloseRequestedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                viewModel.WindowCloseRequested -= handler;
                
                dialog.Close();
            };

            viewModel.WindowCloseRequested += handler;
            dialog.DataContext = viewModel;
            dialog.Owner = owner;

            dialog.Show();
        }
    }

  
}
