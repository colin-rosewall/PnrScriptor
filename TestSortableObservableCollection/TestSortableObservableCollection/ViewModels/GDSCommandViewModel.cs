using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSortableObservableCollection.Interfaces;

namespace TestSortableObservableCollection.ViewModels
{
    public class GDSCommandViewModel : Base.BaseViewModel, IGDSCommandViewModel
    {
        private string _description = null;
        private string _commandLines = null;
        private IGDSCommandItemViewModel _parent = null;
        private SortableObservableCollection<IGDSCommandItemViewModel> _children = null;
        private bool _IsItemExpanded = false;
        private bool _IsItemSelected = false;

        public GDSCommandViewModel(IGDSCommandItemViewModel parent, string theDescription, string theCommandLines)
        {
            _parent = parent;
            _description = theDescription;
            _commandLines = theCommandLines;
            _children = new SortableObservableCollection<IGDSCommandItemViewModel>();
        }

        public SortableObservableCollection<IGDSCommandItemViewModel> Children
        {
            get
            {
                return _children;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                if (_description != value)
                {
                    _description = value;
                    NotifyPropertyChanged(() => Description);
                }
            }
        }

        public string CommandLines
        {
            get
            {
                return _commandLines;
            }

            set
            {
                if (_commandLines != value)
                {
                    _commandLines = value;
                    NotifyPropertyChanged(() => CommandLines);
                }
            }
        }

        public bool IsItemExpanded
        {
            get
            {
                return _IsItemExpanded;
            }

            set
            {
                if (_IsItemExpanded != value)
                {
                    _IsItemExpanded = value;
                    NotifyPropertyChanged(() => IsItemExpanded);
                }
            }
        }

        public bool IsItemSelected
        {
            get
            {
                return _IsItemSelected;
            }

            set
            {
                if (_IsItemSelected != value)
                {
                    _IsItemSelected = value;
                    NotifyPropertyChanged(() => IsItemSelected);
                }
            }
        }

        public IGDSCommandItemViewModel Parent
        {
            get
            {
                return _parent;
            }
        }

        public void AddChildItem(IGDSCommandItemViewModel item)
        {
            _children.Add(item);
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
