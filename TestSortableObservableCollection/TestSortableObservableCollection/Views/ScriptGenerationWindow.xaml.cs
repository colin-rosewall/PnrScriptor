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
using TestSortableObservableCollection.ViewModels;
using System.Text.RegularExpressions;

namespace TestSortableObservableCollection.Views
{
    /// <summary>
    /// Interaction logic for ScriptGenerationWindow.xaml
    /// </summary>
    public partial class ScriptGenerationWindow : Window
    {
        
        public ScriptGenerationWindow(ScriptGenerationViewModel vm)
        {
            InitializeComponent();

            DataContext = vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ScriptGenerationViewModel;

            if (vm != null)
            {
                string maskRegEx = @"([a-zA-Z\d\/ -]+\*\*)([\da-zA-Z ]*[.\d-]*)";
                Regex test1 = new Regex(maskRegEx);
                if (test1.IsMatch(vm.ScriptInput))
                {
                    MatchCollection mc = test1.Matches(vm.ScriptInput);
                    foreach (Match m in mc)
                    {
                        
                        GroupCollection groups = m.Groups;
                        if (groups.Count > 2)
                        {
                            MessageBox.Show(string.Format("{0}:{1}", groups[1].Value, groups[2].Value));
                        }
                    }
                }
            }

        }
    }
}
