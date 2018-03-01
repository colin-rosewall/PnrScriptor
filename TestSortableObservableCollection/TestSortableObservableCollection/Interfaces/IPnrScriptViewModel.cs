using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace TestSortableObservableCollection.Interfaces
{
    public interface IPnrScriptViewModel : IPnrScriptBaseItemViewModel, INotifyDataErrorInfo
    {
        ObservableCollection<IGDSCommandViewModel> GDSCommands { get; set; }
    }
}
