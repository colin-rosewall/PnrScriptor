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

        private PnrScriptTreeViewModel pnrScriptsTVM = null;

        public MainWindow()
        {
            InitializeComponent();
            this.Show();

            gdsCmdsTVM = new GDSCommandTreeViewModel();

            IGDSCmdTreeModel gdsCmdsModel = GDSCmdTreeModelFactory.GetModel("002");
            gdsCmdsModel.LoadTree(gdsCmdsTVM);


            pnrScriptsTVM = new PnrScriptTreeViewModel();
            pnrScriptsTVM.GDSCmdTreeViewModel = gdsCmdsTVM;

            IPnrScriptTreeModel pnrScriptsModel = PnrScriptTreeModelFactory.GetModel("001");
            pnrScriptsModel.LoadTree(pnrScriptsTVM);

            gdsCmdsTVM._updatePnrScriptTVM = new UpdatePnrScriptTVMDelegate(pnrScriptsTVM.UpdateGDSCmdToPnrScriptTVM);

            ShowGDSCommandsWindow();
            ShowPnrScriptsWindow();
        }

        private void ShowGDSCommandsWindow()
        {
            GDSCommandsWindow gdsCommandsWindow = null;

            gdsCommandsWindow = new GDSCommandsWindow(gdsCmdsTVM);
            gdsCommandsWindow.Owner = Application.Current.MainWindow;
            gdsCommandsWindow.Show();
        }

        private void ShowPnrScriptsWindow()
        {
            PnrScriptsWindow pnrScriptsWindow = null;

            pnrScriptsWindow = new PnrScriptsWindow(pnrScriptsTVM);
            pnrScriptsWindow.Owner = Application.Current.MainWindow;
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
            if (pnrScriptsTVM != null)
                pnrScriptsTVM = null;

            if (gdsCmdsTVM != null)
            {
                gdsCmdsTVM._updatePnrScriptTVM = null;
                gdsCmdsTVM = null;
            }
        }
    }
}
