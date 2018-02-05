using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSortableObservableCollection.Interfaces;

namespace TestSortableObservableCollection.Models
{
    public static class GDSCmdTreeModelFactory
    {
        public static IGDSCmdTreeModel GetModel(string fileVersion)
        {
            IGDSCmdTreeModel newItem = null;

            switch (fileVersion)
            {
                case ("001"):
                    newItem = new GDSCmdTreeModelVer001();
                    break;

                case ("002"):
                    newItem = new GDSCmdTreeModelVer002();
                    break;

            }

            return newItem;
        }
    }
}
