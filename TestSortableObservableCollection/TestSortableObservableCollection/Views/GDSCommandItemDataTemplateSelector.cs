using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TestSortableObservableCollection.Interfaces;

namespace TestSortableObservableCollection.Views
{
    class GDSCommandItemDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate GDSCommandSubgroupTemplate { get; set; }

        public DataTemplate GDSCommandItemTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is IGDSCommandSubgroupViewModel)
                return GDSCommandSubgroupTemplate;

            if (item is IGDSCommandViewModel)
                return GDSCommandItemTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}
