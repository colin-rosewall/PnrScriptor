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
using TestSortableObservableCollection.ViewModels;

namespace TestSortableObservableCollection
{
    /// <summary>
    /// Interaction logic for PnrScriptsWindow.xaml
    /// </summary>
    public partial class PnrScriptsWindow : Window
    {
        public PnrScriptsWindow()
        {
            InitializeComponent();
            PnrScriptTreeViewModel vm = new PnrScriptTreeViewModel();
            DataContext = vm;
        }

        private void AddSubgroup_Click(object sender, RoutedEventArgs e)
        {
            var tvm = DataContext as PnrScriptTreeViewModel;

            if (tvm != null)
            {
                tvm.PnrScriptSubgroupToWorkOn = new PnrScriptSubgroupViewModel(null, "empty");
            }
        }

        private void RenameSubgroup_Click(object sender, RoutedEventArgs e)
        {
            var tvm = DataContext as PnrScriptTreeViewModel;

            if (tvm != null)
            {

            }
        }
    }


}
