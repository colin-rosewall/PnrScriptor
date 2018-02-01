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
        
        //string Upgrade();
        void SaveTree(GDSCommandTreeViewModel vm);
        void LoadTree(GDSCommandTreeViewModel vm);
    }
}
