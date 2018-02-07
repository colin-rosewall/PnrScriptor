using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSortableObservableCollection.ViewModels;

namespace TestSortableObservableCollection.Interfaces
{
    public interface IPnrScriptBaseItemViewModel
    {
        UInt64 UniqueID { get; set; }
        SortableObservableCollection<IPnrScriptBaseItemViewModel> Children { get; }
        bool IsItemExpanded { get; set; }
        bool IsItemSelected { get; set; }

        string Description { get; set; }

        IPnrScriptBaseItemViewModel Parent { get; }

        void AddChildItem(IPnrScriptBaseItemViewModel item);
        object Clone();

    }
}
