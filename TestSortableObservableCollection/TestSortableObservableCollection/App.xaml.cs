using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TestSortableObservableCollection.Models;

namespace TestSortableObservableCollection
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            string errMsg = string.Empty;

            base.OnStartup(e);

            errMsg = GDSCmdTreeModel.Upgrade();
            if (errMsg.Length > 0)
            {
                MessageBox.Show(string.Format("Error starting the application - {0}", errMsg));
            }

            //Window main = new MainWindow();
            //main.Show();

        }

    }
}
