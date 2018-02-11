using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestSortableObservableCollection.Views
{
    class MyBindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new MyBindingProxy();
        }

        public object Data
        {
            get
            {
                return (object)GetValue(DataProperty);
            }
            set
            {
                SetValue(DataProperty, value);
            }
        }

        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(object), typeof(MyBindingProxy), new UIPropertyMetadata(null));

    }
}
