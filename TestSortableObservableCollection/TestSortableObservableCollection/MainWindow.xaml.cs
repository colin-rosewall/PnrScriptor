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
            var p = DataContext as GDSCommandTreeViewModel;
            if (p != null)
            {
                SubgroupItemWindow w = new SubgroupItemWindow(p);
                
                w.ShowDialog();

            }
        }
    }
}
