using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;
using TestSortableObservableCollection.Interfaces;
using TestSortableObservableCollection.Models;
using TestSortableObservableCollection.ViewModels;
using TestSortableObservableCollection.Views;

namespace TestSortableObservableCollection.Views
{
    /// <summary>
    /// Interaction logic for GDSCommandsWindow.xaml
    /// </summary>
    public partial class GDSCommandsWindow : Window
    {
        private GDSCommandSubgroupWindow _subgroupWindow = null;
        private GDSCommandWindow _gdsCommandWindow = null;

        public GDSCommandsWindow(GDSCommandTreeViewModel tvm)
        {
            InitializeComponent();

            DataContext = tvm;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }


        private void AddSubgroup_Click(object sender, RoutedEventArgs e)
        {
            var tvm = DataContext as GDSCommandTreeViewModel;
            if (tvm != null)
            {
                tvm.GDSSubgroupToWorkOn = new GDSCommandSubgroupViewModel(null, "empty");
                if (_subgroupWindow == null)
                {
                    _subgroupWindow = new GDSCommandSubgroupWindow(tvm);
                    _subgroupWindow.Owner = this;
                }

                _subgroupWindow.Show();
            }
        }

        private void RenameSubgroup_Click(object sender, RoutedEventArgs e)
        {
            var tvm = DataContext as GDSCommandTreeViewModel;
            if (tvm != null)
            {
                if (tvm.CurrentlySelectedItem != null && tvm.CurrentlySelectedItem.Parent != null)
                {
                    tvm.GDSSubgroupToWorkOn = new GDSCommandSubgroupViewModel(tvm.CurrentlySelectedItem.Parent, tvm.CurrentlySelectedItem.Description);
                    if (_subgroupWindow == null)
                    {
                        _subgroupWindow = new GDSCommandSubgroupWindow(tvm);
                        _subgroupWindow.Owner = this;
                    }

                    _subgroupWindow.Show();
                }
            }
        }

        private void AddGDSCommand_Click(object sender, RoutedEventArgs e)
        {
            var tvm = DataContext as GDSCommandTreeViewModel;
            if (tvm != null)
            {
                tvm.GDSCommandToWorkOn = new GDSCommandViewModel(null, "empty", "");
                if (_gdsCommandWindow == null)
                {
                    _gdsCommandWindow = new GDSCommandWindow(tvm);
                    _gdsCommandWindow.Owner = this;
                }

                _gdsCommandWindow.Show();
            }
        }

        private void ChangeGDSCommand_Click(object sender, RoutedEventArgs e)
        {
            var tvm = DataContext as GDSCommandTreeViewModel;
            if (tvm != null)
            {
                if (tvm.CurrentlySelectedItem != null && tvm.CurrentlySelectedItem.Parent != null)
                {
                    var existingItem = tvm.CurrentlySelectedItem as GDSCommandViewModel;
                    if (existingItem != null)
                    {
                        tvm.GDSCommandToWorkOn = new GDSCommandViewModel(existingItem.Parent, existingItem.Description, existingItem.CommandLines, existingItem.Guid);
                        if (_gdsCommandWindow == null)
                        {
                            _gdsCommandWindow = new GDSCommandWindow(tvm);
                            _gdsCommandWindow.Owner = this;
                        }

                        _gdsCommandWindow.Show();
                    }
                }
            }
        }
    }
}
