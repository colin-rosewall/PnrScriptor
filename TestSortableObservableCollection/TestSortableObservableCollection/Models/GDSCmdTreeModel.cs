﻿using System;
using System.Collections.Generic;
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
                        writer.Write(nodesAsXml.ToString());
                        writer.Flush();
                        writer.Close();
                    }
                }
            }
        }

        public static void LoadTree()
        {

            if (File.Exists(GDSCommandsFilename))
            {
                using (var reader = new StreamReader(GDSCommandsFilename))
                {
                    //string fileContents = reader.ReadToEnd();
                    //reader.Close();

                    using (var xmlReader = XmlReader.Create(reader))
                    {
                        while (xmlReader.Read())
                        {
                            if (xmlReader.IsStartElement())
                            {
                                if (xmlReader.Name.ToUpper() == "NODE")
                                {
                                    string classType = xmlReader.GetAttribute("Type");
                                    string level = xmlReader.GetAttribute("Level");
                                    string uniqueID = xmlReader.GetAttribute("UniqueID");
                                    string parentID = xmlReader.GetAttribute("ParentID");
                                    string description = xmlReader.Value;
                                    xmlReader.MoveToElement();
                                    xmlReader.ReadEndElement();
                                }
                            }
                        }
                    }
                }
            }
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
