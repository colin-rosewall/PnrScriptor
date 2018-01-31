using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TestSortableObservableCollection.Interfaces;
using TestSortableObservableCollection.ViewModels;


namespace TestSortableObservableCollection.Models
{
    public static class GDSCmdTreeModel
    {
        private const string GDSCommandsFilename = "GDSCommands.txt";

        public static string Upgrade()
        {
            string errMsg = string.Empty;
            bool fileNeedsUpgrade = false;
            string fileVersion = string.Empty;

            // if file exists, open file, check if file needs upgrading.  If it does, read all rows and save the file in the new format.
            if (File.Exists(GDSCommandsFilename))
            {
                using (var reader = new StreamReader(GDSCommandsFilename))
                {
                    if (reader != null)
                    {
                        string line = reader.ReadLine();
                        if (line.Length > 0)
                        {
                            if (line.StartsWith("<Nodes>"))
                            {
                                line = reader.ReadLine();
                                if (line.StartsWith("</Nodes>") == false)
                                {
                                    line = line.ToUpper();
                                    if (line.Contains("GUID=") == false)
                                    {
                                        if (line.Contains("TYPE=") && line.Contains("LEVEL=") && line.Contains("PARENTID=") && line.Contains("DESCRIPTION"))
                                        {
                                            fileVersion = "1.0";
                                            fileNeedsUpgrade = true;
                                        }
                                        else
                                        {
                                            errMsg = string.Format("{0} - could not determine file format", GDSCommandsFilename);
                                        }
                                    }
                                }
                            }
                            else
                                errMsg = string.Format("{0} - invalid first line", GDSCommandsFilename);
                        }
                        else
                        {
                            errMsg = string.Format("{0} - first line cannot be empty", GDSCommandsFilename);
                        }
                        reader.Close();
                    }
                    else
                        errMsg = string.Format("{0} - Error creating StreamReader", GDSCommandsFilename);
                }
            }
            else
            {
                //errMsg = string.Format("{0} - does not exist", GDSCommandsFilename);
            }
            
            if (fileNeedsUpgrade && fileVersion.Length > 0)
            {
                //GDSCommandTreeViewModel vm = new GDSCommandTreeViewModel();
                //LoadTree(vm);
                //SaveTree("1.1", vm);
            }

            return errMsg;
        }

        public static void SaveTree(string fileVersion, GDSCommandTreeViewModel vm)
        {
            if (fileVersion == "1.1")
            {
                if (vm != null && vm.Root != null && vm.Root.Count > 0)
                {
                    UInt64 uniqueID = 0;
                    StringBuilder nodesAsXml = new StringBuilder();

                    foreach (var item in vm.Root)
                    {
                        TraverseInLevelOrder(item, ref uniqueID, ref nodesAsXml);
                    }
                    if (nodesAsXml.Length > 0)
                    {
                        using (var writer = new StreamWriter(GDSCommandsFilename, false))
                        {
                            writer.WriteLine("<Nodes>");
                            writer.Write(nodesAsXml.ToString());
                            writer.WriteLine("</Nodes>");
                            writer.Flush();
                            writer.Close();
                        }
                    }
                }
            }
        }


        public static void SaveTree(GDSCommandTreeViewModel vm)
        {
            if (vm != null && vm.Root != null && vm.Root.Count > 0)
            {
                UInt64 uniqueID = 0;
                StringBuilder nodesAsXml = new StringBuilder();

                foreach (var item in vm.Root)
                {
                    TraverseInLevelOrder(item, ref uniqueID, ref nodesAsXml);
                }
                if (nodesAsXml.Length > 0)
                {
                    using (var writer = new StreamWriter(GDSCommandsFilename, false))
                    {
                        writer.WriteLine("<Nodes>");
                        writer.Write(nodesAsXml.ToString());
                        writer.WriteLine("</Nodes>");
                        writer.Flush();
                        writer.Close();
                    }
                }
            }
        }

        public static void LoadTree(GDSCommandTreeViewModel vm)
        {
            if (vm != null && vm.Root != null)
            {
                if (File.Exists(GDSCommandsFilename))
                {
                    using (var reader = new StreamReader(GDSCommandsFilename))
                    {
                        using (var xmlReader = XmlReader.Create(reader))
                        {
                            while (xmlReader.Read())
                            {
                                if (xmlReader.IsStartElement())
                                {
                                    if (xmlReader.Name.ToUpper() == "NODE")
                                    {
                                        bool nodeParsedCorrectly = false;
                                        string classType = xmlReader.GetAttribute("Type");
                                        string level = xmlReader.GetAttribute("Level");
                                        string uniqueID = xmlReader.GetAttribute("UniqueID");
                                        string parentID = xmlReader.GetAttribute("ParentID");
                                        string description = string.Empty;
                                        string commandLines = string.Empty;
                                        if (xmlReader.Read())
                                        {
                                            if (xmlReader.IsStartElement())
                                            {
                                                if (xmlReader.Name.ToUpper() == "DESCRIPTION")
                                                {
                                                    if (xmlReader.Read())
                                                    {
                                                        if (xmlReader.NodeType == XmlNodeType.Text)
                                                        {
                                                            if (xmlReader.HasValue)
                                                                description = xmlReader.Value;
                                                            if (xmlReader.Read())
                                                            {
                                                                if (xmlReader.NodeType == XmlNodeType.EndElement)
                                                                {
                                                                    if (xmlReader.Name.ToUpper() == "DESCRIPTION")
                                                                    {
                                                                        if (xmlReader.Read())
                                                                        {
                                                                            if (xmlReader.IsStartElement())
                                                                            {
                                                                                if (xmlReader.Name.ToUpper() == "COMMANDLINES")
                                                                                {
                                                                                    if (xmlReader.Read())
                                                                                    {
                                                                                        if (xmlReader.NodeType == XmlNodeType.Text)
                                                                                        {
                                                                                            if (xmlReader.HasValue)
                                                                                                commandLines = xmlReader.Value;
                                                                                            if (xmlReader.Read())
                                                                                            {
                                                                                                if (xmlReader.NodeType == XmlNodeType.EndElement)
                                                                                                {
                                                                                                    if (xmlReader.Name.ToUpper() == "COMMANDLINES")
                                                                                                    {
                                                                                                        xmlReader.Read();
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                            if (xmlReader.NodeType == XmlNodeType.EndElement)
                                                                            {
                                                                                if (xmlReader.Name.ToUpper() == "NODE")
                                                                                {
                                                                                    nodeParsedCorrectly = true;
                                                                                }
                                                                            }

                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            if (nodeParsedCorrectly)
                                            {                                                
                                                IGDSCommandItemViewModel parent = null;
                                                IGDSCommandItemViewModel newItem = null;
                                                int intLevel = int.Parse(level);
                                                if (classType.Contains("GDSCommandSubgroupViewModel"))
                                                {
                                                    if (intLevel == 0)
                                                    {
                                                        newItem = new GDSCommandSubgroupViewModel(null, description);
                                                        vm.Root.Add(newItem);
                                                    }
                                                    else
                                                    {
                                                        parent = FindParent(vm, UInt64.Parse(parentID));
                                                        if (parent != null)
                                                        {
                                                            newItem = new GDSCommandSubgroupViewModel(parent, description);
                                                            parent.AddChildItem(newItem);
                                                        }
                                                    }
                                                    if (newItem != null)
                                                    {
                                                        newItem.UniqueID = UInt64.Parse(uniqueID);
                                                    }
                                                }
                                                else if (classType.Contains("GDSCommandViewModel"))
                                                {
                                                    if (intLevel == 0)
                                                    {
                                                        newItem = new GDSCommandViewModel(null, description, commandLines);
                                                        vm.Root.Add(newItem);
                                                    }
                                                    else
                                                    {
                                                        parent = FindParent(vm, UInt64.Parse(parentID));
                                                        if (parent != null)
                                                        {
                                                            newItem = new GDSCommandViewModel(parent, description, commandLines);
                                                            parent.AddChildItem(newItem);
                                                        }
                                                    }
                                                    if (newItem != null)
                                                    {
                                                        newItem.UniqueID = UInt64.Parse(uniqueID);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (vm.Root.Count == 0)
                {
                    IGDSCommandSubgroupViewModel rootItem = new GDSCommandSubgroupViewModel(null, "Root");
                    vm.Root.Add(rootItem);
                }
            }
        }

        private static IGDSCommandItemViewModel FindParent(GDSCommandTreeViewModel vm, UInt64 parentIDRequested)
        {
            IGDSCommandItemViewModel parent = null;
            bool parentFound = false;

            foreach (var topLevelItem in vm.Root)
            {
                Queue<Tuple<int, IGDSCommandItemViewModel>> q = new Queue<Tuple<int, IGDSCommandItemViewModel>>();
                if (topLevelItem != null)
                    q.Enqueue(new Tuple<int, IGDSCommandItemViewModel>(0, topLevelItem));

                while (parentFound == false && q.Count > 0)
                {
                    var queueItem = q.Dequeue();
                    int level = queueItem.Item1;

                    IGDSCommandItemViewModel currentItem = queueItem.Item2;
                    if (currentItem.UniqueID == parentIDRequested)
                    {
                        parentFound = true;
                        parent = currentItem;
                    }
                    else
                    {
                        foreach (var child in currentItem.Children)
                        {
                            q.Enqueue(new Tuple<int, IGDSCommandItemViewModel>(level + 1, child));
                        }
                    }
                }
                if (parentFound)
                {
                    break;
                }
            }

            return parent;
        }
        private static void TraverseInLevelOrder(IGDSCommandItemViewModel item, ref UInt64 uniqueID, ref StringBuilder nodesAsXml)
        {
            Queue<Tuple<int, IGDSCommandItemViewModel>> q = new Queue<Tuple<int, IGDSCommandItemViewModel>>();

            if (item != null)
                q.Enqueue(new Tuple<int, IGDSCommandItemViewModel>(0, item));

            while (q.Count > 0)
            {
                var queueItem = q.Dequeue();
                int level = queueItem.Item1;
                IGDSCommandItemViewModel currentItem = queueItem.Item2;
                currentItem.UniqueID = uniqueID;

                string xmlOutput = string.Empty;
                xmlOutput = FormatItemAsXml(level, uniqueID, currentItem);

                if (xmlOutput.Length > 0)
                {
                    nodesAsXml.AppendLine(xmlOutput);
                }

                uniqueID++;

                foreach (var child in currentItem.Children)
                {
                    q.Enqueue(new Tuple<int, IGDSCommandItemViewModel>(level + 1, child));
                }
            }
        }

        private static string FormatItemAsXml(int level, UInt64 uniqueID, IGDSCommandItemViewModel currentItem)
        {
            string xmlOutput = string.Empty;

            if (currentItem is IGDSCommandSubgroupViewModel || currentItem is IGDSCommandViewModel)
            {
                XmlWriterSettings xmlSettings = new XmlWriterSettings();
                xmlSettings.OmitXmlDeclaration = true;

                using (var stringWriter = new StringWriter())
                {
                    using (var writer = XmlWriter.Create(stringWriter, xmlSettings))
                    {
                        writer.WriteStartElement("Node");
                        writer.WriteAttributeString("Type", currentItem.GetType().ToString());
                        writer.WriteAttributeString("Level", level.ToString());
                        writer.WriteAttributeString("UniqueID", uniqueID.ToString());
                        writer.WriteAttributeString("ParentID", (currentItem.Parent == null ? 0.ToString() : currentItem.Parent.UniqueID.ToString()));
                        if (currentItem.Guid.Length == 0)
                        {
                            currentItem.Guid = System.Guid.NewGuid().ToString();
                        }
                        writer.WriteAttributeString("Guid", currentItem.Guid);
                        writer.WriteElementString("Description", currentItem.Description);
                        var gdsCommand = currentItem as IGDSCommandViewModel;
                        if (gdsCommand != null)
                        {
                            writer.WriteElementString("CommandLines", gdsCommand.CommandLines);
                        }
                        writer.WriteEndElement();
                        writer.Flush();
                        writer.Close();

                        xmlOutput = stringWriter.ToString();
                    }
                }
            }

            return xmlOutput;
        }

    }
}
