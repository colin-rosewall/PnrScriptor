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
using TestSortableObservableCollection.ViewModels;

namespace TestSortableObservableCollection.Views
{
    /// <summary>
    /// Interaction logic for PnrScriptsWindow.xaml
    /// </summary>
    public partial class PnrScriptsWindow : Window
    {
        private PnrScriptSubgroupWindow _subgroupWindow = null;

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
                if (_subgroupWindow == null)
                {
                    _subgroupWindow = new PnrScriptSubgroupWindow(tvm);
                    _subgroupWindow.Owner = this;
                }

                _subgroupWindow.Show();
            }
        }

        private void RenameSubgroup_Click(object sender, RoutedEventArgs e)
        {
            var tvm = DataContext as PnrScriptTreeViewModel;

            if (tvm != null)
            {
                if (tvm.CurrentlySelectedItem != null && tvm.CurrentlySelectedItem.Parent != null)
                {
                    tvm.PnrScriptSubgroupToWorkOn = new PnrScriptSubgroupViewModel(tvm.CurrentlySelectedItem.Parent, tvm.CurrentlySelectedItem.Description);
                    if (_subgroupWindow == null)
                    {
                        _subgroupWindow = new PnrScriptSubgroupWindow(tvm);
                        _subgroupWindow.Owner = this;
                    }

                    _subgroupWindow.Show();
                }
            }
        }
    }


}
