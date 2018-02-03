using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TestSortableObservableCollection.AppConstants;
using TestSortableObservableCollection.Interfaces;
using TestSortableObservableCollection.ViewModels;

namespace TestSortableObservableCollection.Models
{
    public class GDSCmdTreeModelVer002 : IGDSCmdTreeModel
    {
        public void LoadTree(GDSCommandTreeViewModel vm)
        {
            throw new NotImplementedException();
        }

        public void SaveTree(GDSCommandTreeViewModel vm)
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
                    using (var writer = new StreamWriter(Constants.GDSCommandsFilename, false))
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

        private IGDSCommandItemViewModel FindParent(GDSCommandTreeViewModel vm, UInt64 parentIDRequested)
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

        private void TraverseInLevelOrder(IGDSCommandItemViewModel item)
        {
            Queue<Tuple<int, IGDSCommandItemViewModel>> q = new Queue<Tuple<int, IGDSCommandItemViewModel>>();

            if (item != null)
                q.Enqueue(new Tuple<int, IGDSCommandItemViewModel>(0, item));

            while (q.Count > 0)
            {
                var queueItem = q.Dequeue();
                int level = queueItem.Item1;
                IGDSCommandItemViewModel currentItem = queueItem.Item2;
                IGDSCommandViewModel gdsCmdItem = currentItem as IGDSCommandViewModel;
                if (gdsCmdItem != null && gdsCmdItem.Guid.Length == 0)
                    gdsCmdItem.Guid = System.Guid.NewGuid().ToString();

                foreach (var child in currentItem.Children)
                {
                    q.Enqueue(new Tuple<int, IGDSCommandItemViewModel>(level + 1, child));
                }
            }

        }
        private void TraverseInLevelOrder(IGDSCommandItemViewModel item, ref UInt64 uniqueID, ref StringBuilder nodesAsXml)
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

        private string FormatItemAsXml(int level, UInt64 uniqueID, IGDSCommandItemViewModel currentItem)
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
                        writer.WriteElementString("Description", currentItem.Description);
                        var gdsCommand = currentItem as IGDSCommandViewModel;
                        if (gdsCommand != null)
                        {
                            writer.WriteElementString("CommandLines", gdsCommand.CommandLines);
                            writer.WriteElementString("Guid", gdsCommand.Guid);
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

        public string Upgrade(GDSCommandTreeViewModel vm)
        {
            string errMsg = string.Empty;

            if (vm != null && vm.Root != null && vm.Root.Count > 0)
            {
                foreach (var item in vm.Root)
                {
                    TraverseInLevelOrder(item);
                }
            }

            return errMsg;
        }
    }
}
