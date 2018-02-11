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
using System.Xml;
using TestSortableObservableCollection.Views;

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

            //errMsg = Upgrade();
            if (errMsg.Length > 0)
            {
                MessageBox.Show(string.Format("Error starting the application - {0}", errMsg));
            }
            else
            {
                //Window main = new GDSCommandsWindow();
                //main.Show();
                Window main = new PnrScriptsWindow();
                main.Show();
            }

        }

        public string Upgrade()
        {
            string errMsg = string.Empty;
            bool fileNeedsUpgrade = false;
            string fileVersion = string.Empty;

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
                        errMsg = successorModel.Upgrade(vm);
                        successorModel.SaveTree(vm);
                        successorModel = null;
                    }
                    model = null;
                }
            }

            return errMsg;
        }

        private void ParseNode(XmlReader inner, ref bool fileNeedsUpgrade, ref string fileVersion)
        {
            bool nodeParsedCorrectly = false;
            string classType = string.Empty;
            string guid = string.Empty;
            bool ignoreThisNode = false;
            bool startOfGuidFound = false;

            try
            {
                while (inner.EOF == false && ignoreThisNode == false && inner.Read())
                {
                    if (inner.HasAttributes)
                    {
                        classType = inner.GetAttribute("Type");
                        if (classType.Length > 0 && classType.ToUpper().Contains("GDSCOMMANDVIEWMODEL") == false)
                        {
                            ignoreThisNode = true;
                        }
                    }
                    if (ignoreThisNode == false && inner.NodeType == XmlNodeType.Element)
                    {
                        if (inner.Name.Length > 0 && inner.Name.ToUpper() == "GUID")
                        {
                            startOfGuidFound = true;
                        }
                    }
                    if (ignoreThisNode == false && startOfGuidFound == true)
                    {
                        if (inner.NodeType == XmlNodeType.Text)
                        {
                            guid = inner.Value;
                            nodeParsedCorrectly = true;
                        }
                    }
                }
            }
            finally
            {
                inner.Close();
            }
            
            if (nodeParsedCorrectly && guid.Length > 0)
            {
                fileVersion = "002";
                fileNeedsUpgrade = false;
            }
            else if (ignoreThisNode == false && startOfGuidFound == false)
            {
                fileVersion = "001";
                fileNeedsUpgrade = true;
            }
        }

        private void DetermineFileVersion(ref bool fileNeedsUpgrade, ref string fileVersion, ref string errMsg)
        {
            using (var reader = new StreamReader(Constants.GDSCommandsFilename))
            {
                using (var xmlReader = XmlReader.Create(reader))
                {
                    while (xmlReader.EOF == false && fileVersion.Length == 0 && xmlReader.ReadToFollowing("Node"))
                    {
                        XmlReader inner = xmlReader.ReadSubtree();
                        ParseNode(inner, ref fileNeedsUpgrade, ref fileVersion);
                    }
                }
            }
        }
    }
}
