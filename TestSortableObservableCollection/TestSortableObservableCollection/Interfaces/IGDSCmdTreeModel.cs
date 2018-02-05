using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSortableObservableCollection.ViewModels;

namespace TestSortableObservableCollection.Interfaces
{
    public interface IGDSCmdTreeModel
    {
        string Upgrade(GDSCommandTreeViewModel vm);
        void SaveTree(GDSCommandTreeViewModel vm);
        string LoadTree(GDSCommandTreeViewModel vm);
    }
}
