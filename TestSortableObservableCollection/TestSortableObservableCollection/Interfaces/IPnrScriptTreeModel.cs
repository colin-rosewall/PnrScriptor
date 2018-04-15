using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSortableObservableCollection.ViewModels;

namespace TestSortableObservableCollection.Interfaces
{
    public interface IPnrScriptTreeModel
    {
        string Upgrade(PnrScriptTreeViewModel vm);
        void SaveTree(PnrScriptTreeViewModel vm);
        string LoadTree(PnrScriptTreeViewModel vm);
        void UpdateTree(PnrScriptTreeViewModel vm, IGDSCommandViewModel itemUsedForUpdating);
    }
}
