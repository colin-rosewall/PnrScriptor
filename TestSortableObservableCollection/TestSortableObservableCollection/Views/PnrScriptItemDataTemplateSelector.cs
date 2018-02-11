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
    class PnrScriptItemDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate PnrScriptSubgroupTemplate { get; set; }

        public DataTemplate PnrScriptItemTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is IPnrScriptSubgroupViewModel)
                return PnrScriptSubgroupTemplate;

            if (item is IPnrScriptViewModel)
                return PnrScriptItemTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}
