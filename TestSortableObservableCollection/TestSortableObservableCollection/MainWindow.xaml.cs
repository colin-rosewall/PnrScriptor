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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestSortableObservableCollection.ViewModels;

namespace TestSortableObservableCollection
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SubgroupItemWindow subgroupWindow = null;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new GDSCommandTreeViewModel();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var p = DataContext as GDSCommandTreeViewModel;
            if (p != null)
            {
                var sel = p.Root.FirstOrDefault(i => i.IsItemSelected);
                if (sel == null)
                {
                    //sel = p.Root
                }
            }

        }

        private void AddSubgroup_Click(object sender, RoutedEventArgs e)
        {
            var tvm = DataContext as GDSCommandTreeViewModel;
            if (tvm != null)
            {
                tvm.GDSSubgroupToWorkOn = new GDSCommandSubgroupViewModel(null, "empty");
                if (subgroupWindow == null)
                {
                    subgroupWindow = new SubgroupItemWindow(tvm);
                    subgroupWindow.Owner = this;
                }

                subgroupWindow.Show();
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
                    if (subgroupWindow == null)
                    {
                        subgroupWindow = new SubgroupItemWindow(tvm);
                        subgroupWindow.Owner = this;
                    }

                    subgroupWindow.Show();
                }
            }
        }
    }
}
