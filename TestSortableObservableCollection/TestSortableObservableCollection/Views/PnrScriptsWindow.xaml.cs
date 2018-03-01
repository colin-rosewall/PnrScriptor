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
        private PnrScriptWindow _pnrScriptWindow = null;

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

        private void AddPnrScript_Click(object sender, RoutedEventArgs e)
        {
            var tvm = DataContext as PnrScriptTreeViewModel;
            if (tvm != null)
            {
                tvm.PnrScriptToWorkOn = new PnrScriptViewModel(null, "empty", new System.Collections.ObjectModel.ObservableCollection<IGDSCommandViewModel>());
                if (_pnrScriptWindow == null)
                {
                    _pnrScriptWindow = new PnrScriptWindow(tvm);
                    _pnrScriptWindow.Owner = this;
                }

                _pnrScriptWindow.Show();
            }
        }

        private void ChangePnrScript_Click(object sender, RoutedEventArgs e)
        {
            var tvm = DataContext as PnrScriptTreeViewModel;
            if (tvm != null)
            {
                if (tvm.CurrentlySelectedItem != null && tvm.CurrentlySelectedItem.Parent != null)
                {
                    var existingItem = tvm.CurrentlySelectedItem as PnrScriptViewModel;
                    if (existingItem != null)
                    {
                        tvm.PnrScriptToWorkOn = new PnrScriptViewModel(existingItem.Parent, existingItem.Description, existingItem.GDSCommands);
                        if (_pnrScriptWindow == null)
                        {
                            _pnrScriptWindow = new PnrScriptWindow(tvm);
                            _pnrScriptWindow.Owner = this;
                        }

                        _pnrScriptWindow.Show();
                    }
                }
            }
        }
    }


}
