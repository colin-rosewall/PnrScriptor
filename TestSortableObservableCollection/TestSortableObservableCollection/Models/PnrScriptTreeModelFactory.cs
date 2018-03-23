using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSortableObservableCollection.Interfaces;

namespace TestSortableObservableCollection.Models
{
    public static class PnrScriptTreeModelFactory
    {
        public static IPnrScriptTreeModel GetModel(string fileVersion)
        {
            IPnrScriptTreeModel newItem = null;

            switch (fileVersion)
            {
                case ("001"):
                    newItem = new PnrScriptTreeModelVer001();
                    break;
            }

            return newItem;
        }
    }
}
