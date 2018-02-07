using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace TestSortableObservableCollection.Interfaces
{
    public interface IPnrScriptViewModel : IPnrScriptBaseItemViewModel
    {
        ObservableCollection<IGDSCommandViewModel> GDSCommands { get; }
    }
}
