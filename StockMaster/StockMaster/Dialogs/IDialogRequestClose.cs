using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StockMaster.Dialogs
{
    public interface IDialogRequestClose
    {
        event EventHandler<DialogCloseRequestedEventArgs> DialogCloseRequested;
        event EventHandler<WindowCloseRequestedEventArgs> WindowCloseRequested;
    }

    

   

    
}
