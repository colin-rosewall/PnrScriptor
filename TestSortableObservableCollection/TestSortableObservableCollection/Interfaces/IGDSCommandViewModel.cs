using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSortableObservableCollection.Interfaces
{
    public interface IGDSCommandViewModel : IGDSCommandItemViewModel, INotifyDataErrorInfo, ICloneable
    {
        string CommandLines { get; set; }
    }
}
