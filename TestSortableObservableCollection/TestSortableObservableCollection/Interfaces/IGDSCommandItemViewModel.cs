﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSortableObservableCollection.ViewModels;

namespace TestSortableObservableCollection.Interfaces
{
    public interface IGDSCommandItemViewModel
    {
        UInt64 UniqueID { get; set; }
        SortableObservableCollection<IGDSCommandItemViewModel> Children { get; }
        bool IsItemExpanded { get; set; }
        bool IsItemSelected { get; set; }

        string Description { get; set; }

        IGDSCommandItemViewModel Parent { get; }

        string Guid { get; set; }

        void AddChildItem(IGDSCommandItemViewModel item);
        object Clone();

    }
}
