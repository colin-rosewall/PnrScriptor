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

namespace TestSortableObservableCollection.Models
{
    public class PnrScriptTreeModelVer001 : IPnrScriptTreeModel
    {
        public string LoadTree(PnrScriptTreeViewModel vm)
        {
            throw new NotImplementedException();
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

    }
}
