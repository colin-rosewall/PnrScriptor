using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TestSortableObservableCollection.Interfaces;
using TestSortableObservableCollection.Models;
using TestSortableObservableCollection.ViewModels;

namespace TestSortableObservableCollection.Views
{
    /// <summary>
    /// Interaction logic for PnrScriptWindow.xaml
    /// </summary>
    public partial class PnrScriptWindow : Window
    {
        public PnrScriptWindow(PnrScriptViewModel tvm)
        {
            InitializeComponent();

            DataContext = tvm;

            this.Title += String.Format(" - ({0})", tvm.CurrentWindowMode.ToString());

            if (tvm.ClosePnrScriptWindow == null)
                tvm.ClosePnrScriptWindow = new Action(this.Close);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
