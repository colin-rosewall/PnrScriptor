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
        GDSCommandTreeViewModel tvm = null;

        public MainWindow()
        {
            InitializeComponent();
            this.Show();

            tvm = new GDSCommandTreeViewModel();
            IGDSCmdTreeModel model = GDSCmdTreeModelFactory.GetModel("002");
            model.LoadTree(tvm);

            ShowGDSCommandsWindow();
            ShowPnrScriptsWindow();
        }

        private void ShowGDSCommandsWindow()
        {
            GDSCommandsWindow gdsCommandsWindow = null;

            gdsCommandsWindow = new GDSCommandsWindow(tvm);
            gdsCommandsWindow.Owner = this;
            gdsCommandsWindow.Show();
        }

        private void ShowPnrScriptsWindow()
        {
            PnrScriptsWindow pnrScriptsWindow = null;

            pnrScriptsWindow = new PnrScriptsWindow(tvm);
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
    }
}
