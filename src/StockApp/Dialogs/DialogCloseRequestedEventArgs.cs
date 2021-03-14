﻿using System;

namespace StockApp.Dialogs
{
    public class DialogCloseRequestedEventArgs : EventArgs
    {
        public DialogCloseRequestedEventArgs(bool? dialogResult)
        {
            DialogResult = dialogResult;
        }

        public bool? DialogResult { get; }
    }

    public class WindowCloseRequestedEventArgs : EventArgs
    {
        public WindowCloseRequestedEventArgs()
        {

        }
    }

}
