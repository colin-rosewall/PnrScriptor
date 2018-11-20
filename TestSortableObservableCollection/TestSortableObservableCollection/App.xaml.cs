// #define script
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
        private string LoadAmaTestString1()
        {
            string lines = @"NM1SMITHSON/DORRISMRS
AN23JULJNBDXB/AEK/CU
ss1U2
AN01AUGDXBJNB/AEK/CB
ss1B2
AP0423560987";

            return lines;
        }

        private string LoadSabreTestString1()
        {
            string lines = @"114JUNSYDLON¥EK-Y
01Y1Y2
-JAMES/ARRON
PE¥BOB@GMAIL.COM¥
90784578457
7TAW/
W- 275 FREE STREET
3DOCS/DB/19SEP64/M/JAMES/ARRON
6 CR
ER
FOPCASH
6 CR
ER
";
            return lines;
        }

        private string LoadGalTestString1()
        {
            string lines = @"A11DECSYDBNE/QF
01Y1
N.BROWNING/BOB
P.CTCW 02 2927 4736
T.TAU/05SEP
R.CR
ER";
            return lines;
        }

        public string LoadSabreNaskString1()
        {
            string lines = @"5TMASK/DATE**6DEC17........T4**82345..........
            5TDOC TYPE**ET..............PSEUDO**KG3I......
            5TF/BASIS OR-**Y............TOTAL**3720.00....
            5TTAXES**...................CHD/INF AMT**.....
            5TFARE REF---**.............SHOP REF**........
            5TFOP**001..................SECTORS**1.2......
            5TOTHER INFO-**...............................
            5TOTHER INFO-**...............................";

            return lines;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            string errMsg = string.Empty;
            

            base.OnStartup(e);
#if script 
            ScriptGenerationViewModel sm = new ScriptGenerationViewModel();
            sm.ScriptInput = LoadGalTestString1();

            Window sc = new ScriptGenerationWindow(sm);
            sc.Show();
#else
            // this will cause the cache to load
            GDSCmdCache.GetGDSCmd("nothing");

            //errMsg = Upgrade();
            //if (errMsg.Length > 0)
            //{
            //    MessageBox.Show(string.Format("Error starting the application - {0}", errMsg));
            //}
            //else
            //{
            Window main = new MainWindow();
            main.Show();
            //}

#endif
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
