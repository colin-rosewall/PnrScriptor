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

namespace TestSortableObservableCollection
{
    /// <summary>
    /// Interaction logic for SubgroupItemWindow.xaml
    /// </summary>
    public partial class SubgroupItemWindow : Window
    {
        IGDSCommandItemViewModel _parent = null;

        public SubgroupItemWindow()
        {
            InitializeComponent();
        }

        public void SetAddMode(IGDSCommandItemViewModel parent)
        {
            _parent = parent;
            txtSubgroupDescription.Text = "";
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            string newSubgroupDescription = txtSubgroupDescription.Text.Trim();

            if (newSubgroupDescription.Length > 0 )
            {
                IGDSCommandSubgroupViewModel newItem = new GDSCommandSubgroupViewModel(_parent, newSubgroupDescription);
                _parent.Children.Add(newItem);
                this.Close();
            }
        }
    }
}
