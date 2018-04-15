using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSortableObservableCollection.Interfaces;
using TestSortableObservableCollection.ViewModels;
using TestSortableObservableCollection.AppConstants;
using System.Xml;
using System.IO;
using System.Collections.ObjectModel;

namespace TestSortableObservableCollection.Models
{
    public class PnrScriptTreeModelVer001 : IPnrScriptTreeModel
    {
        public string LoadTree(PnrScriptTreeViewModel vm)
        {
            string errMsg = string.Empty;

            if (vm != null && vm.Root != null)
            {
                if (File.Exists(Constants.PnrScriptsFilename))
                {
                    using (var reader = new StreamReader(Constants.PnrScriptsFilename))
                    {
                        using (var xmlReader = XmlReader.Create(reader))
                        {
                            while (xmlReader.EOF == false && errMsg.Length == 0 && xmlReader.ReadToFollowing("Node"))
                            {
                                XmlReader inner = xmlReader.ReadSubtree();
                                errMsg = ParseNode(inner, vm);
                            }
                        }
                    }
                }
                if (vm.Root.Count == 0)
                {
                    IPnrScriptSubgroupViewModel rootItem = new PnrScriptSubgroupViewModel(null, "Root");
                    vm.Root.Add(rootItem);
                }
            }

            return errMsg;

        }

        private string ParseNode(XmlReader inner, PnrScriptTreeViewModel vm)
        {
            bool nodeParsedCorrectly = false;
            string classType = string.Empty;
            string level = string.Empty;
            string uniqueID = string.Empty;
            string parentID = string.Empty;
            string description = string.Empty;
            //string commandLines = string.Empty;
            ObservableCollection<IGDSCommandViewModel> gdsCmds = null;
            string guid = string.Empty;
            string elementName = string.Empty;
            string errMsg = string.Empty;

            try
            {
                while (inner.EOF == false && errMsg.Length == 0 && inner.Read())
                {
                    if (inner.HasAttributes)
                    {
                        classType = inner.GetAttribute("Type");
                        level = inner.GetAttribute("Level");
                        uniqueID = inner.GetAttribute("UniqueID");
                        parentID = inner.GetAttribute("ParentID");
                    }

                    if (inner.NodeType == XmlNodeType.Element && inner.Name.Length > 0)
                    {
                        elementName = inner.Name.ToUpper();
                        switch (elementName)
                        {
                            case "GDSCMDS":
                                gdsCmds = new ObservableCollection<IGDSCommandViewModel>();
                                break;
                        }
                    }
                    else if (inner.NodeType == XmlNodeType.Text)
                    {
                        switch (elementName)
                        {
                            case "DESCRIPTION":
                                description = inner.Value;
                                break;

                            case "GUID":
                                GDSCommandViewModel cmd = GDSCmdCache.GetGDSCmd(inner.Value);
                                if (cmd != null)
                                {
                                    IGDSCommandViewModel newItem = new GDSCommandViewModel(cmd.Parent, cmd.Description, cmd.CommandLines, cmd.Guid);
                                    gdsCmds.Add(newItem);
                                }
                                break;
                        }
                    }
                }
                nodeParsedCorrectly = (classType.Length > 0 && level.Length > 0 && uniqueID.Length > 0 && parentID.Length > 0);

                if (nodeParsedCorrectly)
                {
                    IPnrScriptBaseItemViewModel parent = null;
                    IPnrScriptBaseItemViewModel newItem = null;
                    int intLevel = int.Parse(level);
                    if (classType.Contains("PnrScriptSubgroupViewModel"))
                    {
                        if (intLevel == 0)
                        {
                            newItem = new PnrScriptSubgroupViewModel(null, description);
                            vm.Root.Add(newItem);
                        }
                        else
                        {
                            parent = FindParent(vm, UInt64.Parse(parentID));
                            if (parent != null)
                            {
                                newItem = new PnrScriptSubgroupViewModel(parent, description);
                                parent.AddChildItem(newItem);
                            }
                        }
                        if (newItem != null)
                        {
                            newItem.UniqueID = UInt64.Parse(uniqueID);
                        }
                    }
                    else if (classType.Contains("PnrScriptViewModel"))
                    {
                        if (intLevel == 0)
                        {
                            newItem = new PnrScriptViewModel(null, description, gdsCmds);
                            vm.Root.Add(newItem);
                        }
                        else
                        {
                            parent = FindParent(vm, UInt64.Parse(parentID));
                            if (parent != null)
                            {
                                newItem = new PnrScriptViewModel(parent, description, gdsCmds);
                                parent.AddChildItem(newItem);
                            }
                        }
                        if (newItem != null)
                        {
                            newItem.UniqueID = UInt64.Parse(uniqueID);
                        }
                    }
                }
                else
                {
                    errMsg = string.Format("Invalid Node - UniqueID={0} Description={1}", uniqueID, description);
                }
            }
            finally
            {
                inner.Close();
            }

            return errMsg;
        }

        private IPnrScriptBaseItemViewModel FindParent(PnrScriptTreeViewModel vm, UInt64 parentIDRequested)
        {
            IPnrScriptBaseItemViewModel parent = null;
            bool parentFound = false;

            foreach (var topLevelItem in vm.Root)
            {
                Queue<Tuple<int, IPnrScriptBaseItemViewModel>> q = new Queue<Tuple<int, IPnrScriptBaseItemViewModel>>();
                if (topLevelItem != null)
                    q.Enqueue(new Tuple<int, IPnrScriptBaseItemViewModel>(0, topLevelItem));

                while (parentFound == false && q.Count > 0)
                {
                    var queueItem = q.Dequeue();
                    int level = queueItem.Item1;

                    IPnrScriptBaseItemViewModel currentItem = queueItem.Item2;
                    if (currentItem.UniqueID == parentIDRequested)
                    {
                        parentFound = true;
                        parent = currentItem;
                    }
                    else
                    {
                        foreach (var child in currentItem.Children)
                        {
                            q.Enqueue(new Tuple<int, IPnrScriptBaseItemViewModel>(level + 1, child));
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

        public void SaveTree(PnrScriptTreeViewModel vm)
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
                    using (var writer = new StreamWriter(Constants.PnrScriptsFilename, false))
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

        public string Upgrade(PnrScriptTreeViewModel vm)
        {
            throw new NotImplementedException();
        }

        private void TraverseInLevelOrder(IPnrScriptBaseItemViewModel item, ref UInt64 uniqueID, ref StringBuilder nodesAsXml)
        {
            Queue<Tuple<int, IPnrScriptBaseItemViewModel>> q = new Queue<Tuple<int, IPnrScriptBaseItemViewModel>>();

            if (item != null)
                q.Enqueue(new Tuple<int, IPnrScriptBaseItemViewModel>(0, item));

            while (q.Count > 0)
            {
                var queueItem = q.Dequeue();
                int level = queueItem.Item1;
                IPnrScriptBaseItemViewModel currentItem = queueItem.Item2;
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
                    q.Enqueue(new Tuple<int, IPnrScriptBaseItemViewModel>(level + 1, child));
                }
            }
        }

        private void TraverseInLevelOrderForUpdating(IPnrScriptBaseItemViewModel item, IGDSCommandViewModel itemUsedForUpdating)
        {
            Queue<Tuple<int, IPnrScriptBaseItemViewModel>> q = new Queue<Tuple<int, IPnrScriptBaseItemViewModel>>();

            if (item != null)
                q.Enqueue(new Tuple<int, IPnrScriptBaseItemViewModel>(0, item));

            while (q.Count > 0)
            {
                var queueItem = q.Dequeue();
                int level = queueItem.Item1;
                IPnrScriptBaseItemViewModel currentItem = queueItem.Item2;
                //currentItem.UniqueID = uniqueID;

                var pnrScript = currentItem as IPnrScriptViewModel;

                if (pnrScript != null && pnrScript.GDSCommands != null)
                {
                    // only update gds cmds with the same guid
                    var cmds = pnrScript.GDSCommands.Where(c => c.Guid == itemUsedForUpdating.Guid);
                    foreach (var i in cmds)
                    {
                        i.Description = itemUsedForUpdating.Description;
                        i.CommandLines = itemUsedForUpdating.CommandLines;
                    }
                }

                //uniqueID++;

                foreach (var child in currentItem.Children)
                {
                    q.Enqueue(new Tuple<int, IPnrScriptBaseItemViewModel>(level + 1, child));
                }
            }

        }
        private string FormatItemAsXml(int level, UInt64 uniqueID, IPnrScriptBaseItemViewModel currentItem)
        {
            string xmlOutput = string.Empty;

            if (currentItem is IPnrScriptSubgroupViewModel || currentItem is IPnrScriptViewModel)
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
                        writer.WriteElementString("Description", currentItem.Description);
                        var pnrScript = currentItem as IPnrScriptViewModel;
                        if (pnrScript != null)
                        {
                            if (pnrScript.GDSCommands != null)
                            {
                                writer.WriteStartElement("GdsCmds");
                                foreach (var cmd in pnrScript.GDSCommands)
                                {
                                    writer.WriteElementString("Guid", cmd.Guid);
                                }
                                writer.WriteEndElement();
                            }
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

        public void UpdateTree(PnrScriptTreeViewModel vm, IGDSCommandViewModel itemUsedForUpdating)
        {
            if (vm != null && vm.Root != null && vm.Root.Count > 0)
            {
                foreach (var item in vm.Root)
                {
                    TraverseInLevelOrderForUpdating(item, itemUsedForUpdating);
                }
            }
        }
    }
}
