using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace TestSortableObservableCollection.Behaviours
{
    public static class WindowCloseBehaviour
    {
        public static readonly DependencyProperty HideDontCloseProperty = 
            DependencyProperty.RegisterAttached("HideDontClose", typeof(bool), typeof(WindowCloseBehaviour), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnHideDontCloseChanged)));

        public static bool GetHideDontClose(DependencyObject d)
        {
            return ((bool)d.GetValue(HideDontCloseProperty));
        }

        public static void SetHideDontClose(DependencyObject d, bool value)
        {
            d.SetValue(HideDontCloseProperty, value);
        }

        private static void OnHideDontCloseChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Window w = d as Window;

            if (w == null)
                return;

            if ((bool)e.NewValue)
                w.Closing += W_Closing;
            else
                w.Closing -= W_Closing;

        }

        private static void W_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Window w = sender as Window;

            if (w == null)
                return;
            else
            {
                e.Cancel = true;
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (DispatcherOperationCallback)(arg =>
                {
                    w.Hide();
                    return null;
                }), null);
            }
        }
    }
}
