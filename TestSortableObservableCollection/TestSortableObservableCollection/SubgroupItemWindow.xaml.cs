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
using System.Windows.Threading;
using TestSortableObservableCollection.Interfaces;
using TestSortableObservableCollection.ViewModels;

namespace TestSortableObservableCollection
{
    /// <summary>
    /// Interaction logic for SubgroupItemWindow.xaml
    /// </summary>
    public partial class SubgroupItemWindow : Window
    {
        public SubgroupItemWindow(GDSCommandTreeViewModel tvm)
        {
            InitializeComponent();
            DataContext = tvm;
            if (tvm.CloseSubgroupWindow == null)
                tvm.CloseSubgroupWindow = new Action(this.Hide);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
