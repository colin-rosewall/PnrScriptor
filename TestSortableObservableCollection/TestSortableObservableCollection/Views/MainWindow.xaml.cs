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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GDSCommandTreeViewModel gdsCmdsTVM = null;
        private GDSCommandTreeViewModel.AddGDSCmdToCacheDelegate addToCache = new GDSCommandTreeViewModel.AddGDSCmdToCacheDelegate(GDSCmdCache.AddGDSCmdToCache);
        private GDSCommandTreeViewModel.UpdateGDSCmdToCacheDelegate updateToCache = new GDSCommandTreeViewModel.UpdateGDSCmdToCacheDelegate(GDSCmdCache.UpdateGDSCmdToCache);
        private GDSCommandTreeViewModel.DeleteGDSCmdFromCacheDelegate deleteFromCache = new GDSCommandTreeViewModel.DeleteGDSCmdFromCacheDelegate(GDSCmdCache.DeleteGDSCmdFromCache);
        private GDSCommandTreeViewModel.UpdateGDSCmdToCacheDelegate updateToPnrScriptTVM = null;

        private PnrScriptTreeViewModel pnrScriptsTVM = null;

        public MainWindow()
        {
            InitializeComponent();
            this.Show();

            gdsCmdsTVM = new GDSCommandTreeViewModel();
            
            gdsCmdsTVM.RaiseAddGDSCmdToCache += addToCache;
            gdsCmdsTVM.RaiseUpdateGDSCmdToCache += updateToCache;
            gdsCmdsTVM.RaiseDeleteGDSCmdFromCache += deleteFromCache;

            IGDSCmdTreeModel gdsCmdsModel = GDSCmdTreeModelFactory.GetModel("002");
            gdsCmdsModel.LoadTree(gdsCmdsTVM);


            pnrScriptsTVM = new PnrScriptTreeViewModel();
            pnrScriptsTVM.GDSCmdTreeViewModel = gdsCmdsTVM;

            IPnrScriptTreeModel pnrScriptsModel = PnrScriptTreeModelFactory.GetModel("001");
            pnrScriptsModel.LoadTree(pnrScriptsTVM);

            updateToPnrScriptTVM = new GDSCommandTreeViewModel.UpdateGDSCmdToCacheDelegate(pnrScriptsTVM.UpdateGDSCmdToPnrScriptTVM);
            gdsCmdsTVM.RaiseUpdateGDSCmdToCache += updateToPnrScriptTVM;

            ShowGDSCommandsWindow();
            ShowPnrScriptsWindow();
        }

        private void ShowGDSCommandsWindow()
        {
            GDSCommandsWindow gdsCommandsWindow = null;

            gdsCommandsWindow = new GDSCommandsWindow(gdsCmdsTVM);
            gdsCommandsWindow.Owner = this;
            gdsCommandsWindow.Show();
        }

        private void ShowPnrScriptsWindow()
        {
            PnrScriptsWindow pnrScriptsWindow = null;

            pnrScriptsWindow = new PnrScriptsWindow(pnrScriptsTVM);
            pnrScriptsWindow.Owner = this;
            pnrScriptsWindow.Show();
        }

        private void CloseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ShowGDSCommandsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowGDSCommandsWindow();
        }

        private void ShowPnrScriptsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowPnrScriptsWindow();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (gdsCmdsTVM != null)
            {
                gdsCmdsTVM.RaiseAddGDSCmdToCache -= addToCache;
                gdsCmdsTVM.RaiseUpdateGDSCmdToCache -= updateToCache;
                gdsCmdsTVM.RaiseDeleteGDSCmdFromCache -= deleteFromCache;
                gdsCmdsTVM.RaiseUpdateGDSCmdToCache -= updateToPnrScriptTVM;
            }
        }
    }
}
