using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSortableObservableCollection.Interfaces
{
    public interface IGDSCommandViewModel : IGDSCommandItemViewModel, INotifyDataErrorInfo
    {
        string CommandLines { get; set; }
    }
}
