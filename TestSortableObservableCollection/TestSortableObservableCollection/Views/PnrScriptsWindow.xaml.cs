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
using TestSortableObservableCollection.AppConstants;
using TestSortableObservableCollection.Models;
using TestSortableObservableCollection.ViewModels;
using System.Collections.ObjectModel;

namespace TestSortableObservableCollection.Views
{
    /// <summary>
    /// Interaction logic for PnrScriptsWindow.xaml
    /// </summary>
    public partial class PnrScriptsWindow : Window
    {
        //private PnrScriptSubgroupWindow _subgroupWindow = null;
        //private PnrScriptWindow _pnrScriptWindow = null;
        private DispatcherTimer timerForSaveEvent;

        public PnrScriptsWindow(PnrScriptTreeViewModel pnrScriptsTVM)
        {
            InitializeComponent();

            DataContext = pnrScriptsTVM;
            if (pnrScriptsTVM.OpenScriptGenerationWindow == null)
            {
                pnrScriptsTVM.OpenScriptGenerationWindow = new Action(GenerateScriptClick);
            }
        }

        private void AddSubgroup_Click(object sender, RoutedEventArgs e)
        {
            var tvm = DataContext as PnrScriptTreeViewModel;

            if (tvm != null)
            {
                //tvm.PnrScriptSubgroupToWorkOn = new PnrScriptSubgroupViewModel(null, "empty");
                //if (_subgroupWindow == null)
                //{
                //    _subgroupWindow = new PnrScriptSubgroupWindow(tvm);
                //    _subgroupWindow.Owner = Application.Current.MainWindow;
                //}

                //_subgroupWindow.Show();
            }
        }

        private void RenameSubgroup_Click(object sender, RoutedEventArgs e)
        {
            var tvm = DataContext as PnrScriptTreeViewModel;

            if (tvm != null)
            {
                //if (tvm.CurrentlySelectedItem != null && tvm.CurrentlySelectedItem.Parent != null)
                //{
                //    tvm.PnrScriptSubgroupToWorkOn = new PnrScriptSubgroupViewModel(tvm.CurrentlySelectedItem.Parent, tvm.CurrentlySelectedItem.Description);
                //    if (_subgroupWindow == null)
                //    {
                //        _subgroupWindow = new PnrScriptSubgroupWindow(tvm);
                //        _subgroupWindow.Owner = Application.Current.MainWindow;
                //    }

                //    _subgroupWindow.Show();
                //}
            }
        }

        private void AddPnrScript_Click(object sender, RoutedEventArgs e)
        {
            var tvm = DataContext as PnrScriptTreeViewModel;
            if (tvm != null)
            {
                if (tvm.CurrentlySelectedItem != null)
                {
                    PnrScriptViewModel vm = new PnrScriptViewModel(Constants.WindowMode.Add, tvm.CurrentlySelectedItem, "empty", tvm.GDSCmdTreeViewModel, new ObservableCollection<IGDSCommandViewModel>(), tvm.SaveNotification, null);
                    PnrScriptWindow psw = new PnrScriptWindow(vm);
                    psw.Owner = Application.Current.MainWindow;
                    psw.Show();
                }

                //tvm.PnrScriptToWorkOn = new PnrScriptViewModel(null, "empty", new System.Collections.ObjectModel.ObservableCollection<IGDSCommandViewModel>());
                //if (_pnrScriptWindow == null)
                //{
                //    _pnrScriptWindow = new PnrScriptWindow(tvm);
                //    _pnrScriptWindow.Owner = Application.Current.MainWindow;
                //}

                //_pnrScriptWindow.Show();
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
                        PnrScriptViewModel vm = new PnrScriptViewModel(Constants.WindowMode.Change, existingItem.Parent, existingItem.Description, tvm.GDSCmdTreeViewModel, existingItem.GDSCommands, tvm.SaveNotification, existingItem);
                        PnrScriptWindow psw = new PnrScriptWindow(vm);
                        psw.Owner = Application.Current.MainWindow;
                        psw.Show();

                        //tvm.PnrScriptToWorkOn = new PnrScriptViewModel(existingItem.Parent, existingItem.Description, existingItem.GDSCommands);
                        //if (_pnrScriptWindow == null)
                        //{
                        //    _pnrScriptWindow = new PnrScriptWindow(tvm);
                        //    _pnrScriptWindow.Owner = Application.Current.MainWindow;
                        //}

                        //_pnrScriptWindow.Show();
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
            var tvm = DataContext as PnrScriptTreeViewModel;

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

        // object sender, RoutedEventArgs e
        private void GenerateScriptClick()
        {
            var tvm = DataContext as PnrScriptTreeViewModel;

            if (tvm != null)
            {
                ScriptGenerationViewModel vm = new ScriptGenerationViewModel();
                vm.ScriptInput = tvm.GeneratedScript;

                ScriptGenerationWindow sgw = new ScriptGenerationWindow(vm);
                sgw.Owner = Application.Current.MainWindow;
                sgw.Show();
            }
        }

    }


}
