using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace StockMaster.Behaviours
{
    public class SelectAllFocusBehavior
    {
        public static bool GetEnable(FrameworkElement frameworkElement)
        {
            return (bool)frameworkElement.GetValue(EnableProperty);
        }

        public static void SetEnable(FrameworkElement frameworkElement, bool value)
        {
            frameworkElement.SetValue(EnableProperty, value);
        }

        public static readonly DependencyProperty EnableProperty =
                 DependencyProperty.RegisterAttached("Enable",
                    typeof(bool), typeof(SelectAllFocusBehavior),
                    new FrameworkPropertyMetadata(false, OnEnableChanged));

        private static void OnEnableChanged
                   (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement frameworkElement)) return;

            if (e.NewValue is bool == false) return;

            if ((bool)e.NewValue)
            {
                frameworkElement.GotFocus += SelectAll;
                frameworkElement.PreviewMouseDown += IgnoreMouseButton;
            }
            else
            {
                frameworkElement.GotFocus -= SelectAll;
                frameworkElement.PreviewMouseDown -= IgnoreMouseButton;
            }
        }

        private static void SelectAll(object sender, RoutedEventArgs e)
        {
            var frameworkElement = e.OriginalSource as FrameworkElement;
            if (frameworkElement is TextBox textbox)
                textbox.SelectAll();
            else if (frameworkElement is PasswordBox passwordbox)
                passwordbox.SelectAll();
        }

        private static void IgnoreMouseButton
                (object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!(sender is FrameworkElement frameworkElement) || frameworkElement.IsKeyboardFocusWithin) return;
            e.Handled = true;
            frameworkElement.Focus();
        }

    }
}
