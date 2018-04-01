using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSortableObservableCollection.ViewModels
{
    public static class CloningExtensions
    {
        public static ObservableCollection<T> DeepCopy<T>(this ObservableCollection<T> list) where T : ICloneable
        {
            return new ObservableCollection<T>(list.Select(x => x.Clone()).Cast<T>());
        }
    }
}
