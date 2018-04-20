using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSortableObservableCollection.ViewModels;
using TestSortableObservableCollection.AppConstants;
using System.Xml;
using TestSortableObservableCollection.Interfaces;

namespace TestSortableObservableCollection.Models
{
    public static class GDSCmdCache
    {
        private static Dictionary<string, GDSCommandViewModel> GDSCmds = null;

        public static GDSCommandViewModel GetGDSCmd(string guid)
        {
            GDSCommandViewModel fetchedItem = null;
            string errMsg = string.Empty;

            if (GDSCmds == null)
            {
                errMsg = LoadGDSCmds();

                if (errMsg.Length > 0)
                    throw new ApplicationException(errMsg);
            }


            GDSCmds.TryGetValue(guid, out fetchedItem);

            return fetchedItem;
        }

        public static void AddGDSCmdToCache(IGDSCommandViewModel itemToBeAdded)
        {
            if (itemToBeAdded.Guid.Length > 0)
            {
                GDSCommandViewModel newItem = new GDSCommandViewModel(null, itemToBeAdded.Description, itemToBeAdded.CommandLines, itemToBeAdded.Guid);
                if (newItem != null)
                {
                    GDSCmds.Add(newItem.Guid, newItem);
                }
            }
        }

        public static void UpdateGDSCmdToCache(IGDSCommandViewModel itemToBeUpdated)
        {
            if (itemToBeUpdated.Guid.Length > 0)
            {
                GDSCommandViewModel fetchedItem = null;

                GDSCmds.TryGetValue(itemToBeUpdated.Guid, out fetchedItem);

                if (fetchedItem != null)
                {
                    fetchedItem.Description = itemToBeUpdated.Description;
                    fetchedItem.CommandLines = itemToBeUpdated.CommandLines;
                }
            }
        }

        public static void DeleteGDSCmdFromCache(IGDSCommandViewModel itemToBeDeleted)
        {
            if (itemToBeDeleted.Guid.Length > 0)
            {
                if (GDSCmds.ContainsKey(itemToBeDeleted.Guid))
                    GDSCmds.Remove(itemToBeDeleted.Guid);
            }
        }

        private static string LoadGDSCmds()
        {
            string errMsg = string.Empty;

            GDSCmds = new Dictionary<string, GDSCommandViewModel>();
            if (File.Exists(Constants.GDSCommandsFilename))
            {
                using (var reader = new StreamReader(Constants.GDSCommandsFilename))
                {
                    using (var xmlReader = XmlReader.Create(reader))
                    {
                        while (xmlReader.EOF == false && errMsg.Length == 0 && xmlReader.ReadToFollowing("Node"))
                        {
                            XmlReader inner = xmlReader.ReadSubtree();
                            errMsg = ParseNode(inner);
                        }
                    }
                }
            }

            return errMsg;
        }

        private static string ParseNode(XmlReader inner)
        {
            bool nodeParsedCorrectly = false;
            string classType = string.Empty;
            string level = string.Empty;
            string uniqueID = string.Empty;
            string parentID = string.Empty;
            string description = string.Empty;
            string commandLines = string.Empty;
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
                    }
                    else if (inner.NodeType == XmlNodeType.Text)
                    {
                        switch (elementName)
                        {
                            case "DESCRIPTION":
                                description = inner.Value;
                                break;

                            case "COMMANDLINES":
                                commandLines = inner.Value;
                                // Xml normalizes new line so we need to change it back
                                commandLines = commandLines.Replace("\n", Environment.NewLine);
                                break;

                            case "GUID":
                                guid = inner.Value;
                                break;
                        }
                    }
                }
                nodeParsedCorrectly = (classType.Length > 0 && level.Length > 0 && uniqueID.Length > 0 && parentID.Length > 0);
                if (classType.Contains("GDSCommandViewModel"))
                {
                    nodeParsedCorrectly = nodeParsedCorrectly && guid.Length > 0;
                }

                if (nodeParsedCorrectly)
                {
                    if (classType.Contains("GDSCommandViewModel"))
                    {
                        GDSCommandViewModel newItem = null;
                        
                        newItem = new GDSCommandViewModel(null, description, commandLines);

                        if (newItem != null)
                        {
                            newItem.UniqueID = UInt64.Parse(uniqueID);
                            newItem.Guid = guid;  // Only GDS Commands have a guid
                            GDSCmds.Add(guid, newItem);
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
    }
}
