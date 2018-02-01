using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TestSortableObservableCollection.Interfaces;
using TestSortableObservableCollection.AppConstants;
using TestSortableObservableCollection.Models;
using TestSortableObservableCollection.ViewModels;

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

            // errMsg = Upgrade();
            if (errMsg.Length > 0)
            {
                MessageBox.Show(string.Format("Error starting the application - {0}", errMsg));
            }

            //Window main = new MainWindow();
            //main.Show();

        }

        private void DetermineFileVersion(ref bool fileNeedsUpgrade, ref string fileVersion, ref string errMsg)
        {
            bool ignoreCase = true;
            bool fileVersionUnknown = true;

            using (var reader = new StreamReader(Constants.GDSCommandsFilename))
            {
                if (reader != null)
                {
                    string line = string.Empty;
                    while (reader.EndOfStream == false && errMsg.Length == 0 && fileVersionUnknown)
                    {
                        line = reader.ReadLine();
                        if (line.Length > 0)
                        {
                            if (line.StartsWith("<Nodes>", ignoreCase, null) || line.StartsWith("</Nodes>", ignoreCase, null))
                            {
                                // do nothing because the line is valid
                            }
                            else
                            {
                                line = line.ToUpper();
                                if (line.Contains("COMMANDLINES"))
                                {
                                    if (line.Contains("TYPE=") && line.Contains("LEVEL=") && line.Contains("PARENTID=") && line.Contains("DESCRIPTION"))
                                    {
                                        if (line.Contains("GUID") == false)
                                        {
                                            fileVersion = "001";
                                            fileNeedsUpgrade = true;
                                            fileVersionUnknown = false;
                                        }
                                    }
                                    else
                                    {
                                        errMsg = string.Format("{0} - could not determine file format", Constants.GDSCommandsFilename);
                                    }
                                }
                            }
                        }
                    }

                    reader.Close();
                }
                else
                    errMsg = string.Format("{0} - Error creating StreamReader", Constants.GDSCommandsFilename);
            }
        }

        public string Upgrade()
        {
            string errMsg = string.Empty;
            bool fileNeedsUpgrade = false;
            string fileVersion = string.Empty;

            // if file exists, open file, check if file needs upgrading.  If it does, read all rows and save the file in the new format.
            if (File.Exists(Constants.GDSCommandsFilename))
            {
                DetermineFileVersion(ref fileNeedsUpgrade, ref fileVersion, ref errMsg);
            }

            if (fileNeedsUpgrade && fileVersion.Length > 0)
            {
                IGDSCmdTreeModel model = GDSCmdTreeModelFactory.GetModel(fileVersion);
                if (model != null)
                {
                    GDSCommandTreeViewModel vm = new GDSCommandTreeViewModel();
                    model.LoadTree(vm);
                    IGDSCmdTreeModel successorModel = GDSCmdTreeModelFactory.GetModel("002");
                    if (successorModel != null)
                    {
                        // we need to assign new guids to the gds commands in vm

                        //successorModel.SaveTree(vm);
                    }
                        
                }
            }

            return errMsg;
        }


    }
}
