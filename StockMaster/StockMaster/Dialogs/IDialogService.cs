using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMaster.Dialogs
{
    public interface IDialogService
    {
        void Register<TViewModel, TView>() where TViewModel : IDialogRequestClose
                                           where TView : IDialog;

        bool? ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : IDialogRequestClose;

        void Show<TViewModel>(TViewModel viewModel) where TViewModel : IDialogRequestClose;

    }

    //public interface IWindowService
    //{
    //    void Register<TViewModel, TView>() where TViewModel : IDialogRequestClose
    //                                      where TView : IWindow;

    //    void Show<TViewModel>(TViewModel viewModel) where TViewModel : IDialogRequestClose;
    //}
}
