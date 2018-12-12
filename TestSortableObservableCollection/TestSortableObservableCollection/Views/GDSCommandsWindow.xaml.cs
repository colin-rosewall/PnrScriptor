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
using System.Windows.Threading;
using System.Xml;
using System.Xml.Serialization;
using TestSortableObservableCollection.Interfaces;
using TestSortableObservableCollection.AppConstants;
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
        private DispatcherTimer timerForSaveEvent;

        public GDSCommandsWindow(GDSCommandTreeViewModel tvm)
        {
            InitializeComponent();

            DataContext = tvm;

        }


        private void AddSubgroup_Click(object sender, RoutedEventArgs e)
        {
            var tvm = DataContext as GDSCommandTreeViewModel;
            if (tvm != null)
            {
                if (tvm.CurrentlySelectedItem != null)
                {
                    GDSCommandSubgroupViewModel vm = new GDSCommandSubgroupViewModel(Constants.WindowMode.Add, tvm.CurrentlySelectedItem, "empty", tvm.SaveNotification, null);
                    GDSCommandSubgroupWindow sw = new GDSCommandSubgroupWindow(vm);
                    sw.Owner = Application.Current.MainWindow;
                    sw.Show();
                }
            }
        }

        private void RenameSubgroup_Click(object sender, RoutedEventArgs e)
        {
            var tvm = DataContext as GDSCommandTreeViewModel;
            if (tvm != null)
            {
                if (tvm.CurrentlySelectedItem != null && tvm.CurrentlySelectedItem.Parent != null)
                {
                    var existingItem = tvm.CurrentlySelectedItem as GDSCommandSubgroupViewModel;
                    if (existingItem != null)
                    {
                        GDSCommandSubgroupViewModel vm = new GDSCommandSubgroupViewModel(Constants.WindowMode.Change, existingItem.Parent, existingItem.Description, tvm.SaveNotification, existingItem);
                        GDSCommandSubgroupWindow sw = new GDSCommandSubgroupWindow(vm);
                        sw.Owner = Application.Current.MainWindow;
                        sw.Show();
                    }
                }
            }
        }

        private void AddGDSCommand_Click(object sender, RoutedEventArgs e)
        {
            var tvm = DataContext as GDSCommandTreeViewModel;
            if (tvm != null)
            {
                //tvm.GDSCommandToWorkOn = new GDSCommandViewModel(null, "empty", "");
                //if (_gdsCommandWindow == null)
                //{
                //    _gdsCommandWindow = new GDSCommandWindow(tvm);
                //    _gdsCommandWindow.Owner = Application.Current.MainWindow;
                //}

                //_gdsCommandWindow.Show();
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
                        //tvm.GDSCommandToWorkOn = new GDSCommandViewModel(existingItem.Parent, existingItem.Description, existingItem.CommandLines, existingItem.Guid);
                        //if (_gdsCommandWindow == null)
                        //{
                        //    _gdsCommandWindow = new GDSCommandWindow(tvm);
                        //    _gdsCommandWindow.Owner = Application.Current.MainWindow;
                        //}

                        //_gdsCommandWindow.Show();
                    }
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timerForSaveEvent = new DispatcherTimer();
            timerForSaveEvent.Tick += new EventHandler(tick);
            timerForSaveEvent.Interval = new TimeSpan(0, 1, 1);
            timerForSaveEvent.Start();
        }

        private void tick(object sender, EventArgs e)
        {
            var tvm = DataContext as GDSCommandTreeViewModel;

            if (tvm != null && tvm.SaveTreeCommand != null)
            {
                if (tvm.SaveTreeCommand.CanExecute(null))
                {
                    tvm.SaveTreeCommand.Execute(null);
                }
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (timerForSaveEvent != null)
            {
                timerForSaveEvent.Stop();
                timerForSaveEvent.IsEnabled = false;
                timerForSaveEvent = null;
            }
        }
    }
}
