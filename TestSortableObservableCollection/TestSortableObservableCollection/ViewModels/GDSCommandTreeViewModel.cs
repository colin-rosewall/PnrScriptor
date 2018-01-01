using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using TestSortableObservableCollection.Interfaces;
using System.Windows.Input;
using TestSortableObservableCollection.ViewModels.Base;
using System.Windows;

namespace TestSortableObservableCollection.ViewModels
{
    public class GDSCommandTreeViewModel : Base.BaseViewModel
    {
        private ObservableCollection<IGDSCommandItemViewModel> _root = null;
        private ICommand _saveSubgroupCommand = null;
        private ICommand _selectedItemChangedCommand = null;
        private IGDSCommandItemViewModel _currentlySelectedItem { get; set; }
        private IGDSCommandSubgroupViewModel _GDSSubgroupToWorkOn = null;
        public Action CloseSubgroupWindow { get; set; }
        

        public GDSCommandTreeViewModel()
        {
            _saveSubgroupCommand = new RelayCommand<object>(SaveSubgroup_Executed, SaveSubgroup_CanExecute);
            _selectedItemChangedCommand = new RelayCommand<object>(SelectedItemChanged);

            _root = new ObservableCollection<IGDSCommandItemViewModel>();
            IGDSCommandSubgroupViewModel rootItem = new GDSCommandSubgroupViewModel(null, "Root");

            _root.Add(rootItem);

            IGDSCommandSubgroupViewModel sabreItem = new GDSCommandSubgroupViewModel(rootItem, "Sabre");
            rootItem.AddChildItem(sabreItem);

            IGDSCommandSubgroupViewModel galileoItem = new GDSCommandSubgroupViewModel(rootItem, "Galileo");
            rootItem.AddChildItem(galileoItem);

            IGDSCommandViewModel addAdult = new GDSCommandViewModel(galileoItem, "Add Gal Adult");
            galileoItem.Children.Add(addAdult);

            rootItem.Children.Sort(k => k.Description);
        }

        public ObservableCollection<IGDSCommandItemViewModel> Root
        {
            get
            {
                return _root;
            }
        }


        public IGDSCommandSubgroupViewModel GDSSubgroupToWorkOn
        {
            get
            {
                return _GDSSubgroupToWorkOn;
            }
            set
            {
                _GDSSubgroupToWorkOn = value;
                NotifyPropertyChanged(() => GDSSubgroupToWorkOn);
            }
        }

        public ICommand SaveSubgroupCommand
        {
            get
            {
                return _saveSubgroupCommand;
            }
            set
            {
                _saveSubgroupCommand = value;
            }
        }

        public ICommand SelectedItemChangedCommand
        {
            get
            {
                return _selectedItemChangedCommand;
            }
            set
            {
                _selectedItemChangedCommand = value;
            }
        }
        public void SaveSubgroup_Executed(object obj)
        {
            if (GDSSubgroupToWorkOn != null)
            {
                if (_currentlySelectedItem != null)
                {
                    if (GDSSubgroupToWorkOn.Parent == null)
                    {
                        IGDSCommandSubgroupViewModel newItem = new GDSCommandSubgroupViewModel(_currentlySelectedItem, GDSSubgroupToWorkOn.Description);
                        _currentlySelectedItem.AddChildItem(newItem);
                        CloseSubgroupWindow();
                    }
                }
            }
        }

        public bool SaveSubgroup_CanExecute(object obj)
        {
            bool result = false;

            if (_GDSSubgroupToWorkOn != null)
            {
                result = !(_GDSSubgroupToWorkOn.HasErrors);
            }

            return result;
        }

        public void SelectedItemChanged(object obj)
        {
            if (obj is IGDSCommandItemViewModel == false)
                return;

            var item = obj as IGDSCommandItemViewModel;
            _currentlySelectedItem = item;
        }
    }
}
