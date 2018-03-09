using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using TestSortableObservableCollection.Interfaces;
using TestSortableObservableCollection.ViewModels;
using System.Windows.Input;
using TestSortableObservableCollection.ViewModels.Base;


namespace TestSortableObservableCollection.ViewModels
{
    public class PnrScriptTreeViewModel : Base.BaseViewModel
    {
        private ObservableCollection<IPnrScriptBaseItemViewModel> _root = null;
        private ICommand _saveSubgroupCommand = null;
        private ICommand _renameSubgroupCommand = null;
        private ICommand _deleteSubgroupCommand = null;

        private ICommand _savePnrScriptCommand = null;

        private ICommand _deleteListItemCommand = null;

        private ICommand _selectedItemChangedCommand = null;

        private ICommand _mouseDoubleClickCommand = null;

        private IPnrScriptBaseItemViewModel _currentlySelectedItem { get; set; }
        private IPnrScriptViewModel _itemToCut { get; set; }
        private IPnrScriptSubgroupViewModel _pnrScriptSubgroupToWorkOn = null;
        private IPnrScriptViewModel _pnrScriptToWorkOn = null;
        public Action CloseSubgroupWindow { get; set; }
        public Action ClosePnrScriptWindow { get; set; }
        private GDSCommandTreeViewModel _gdsCmdTreeViewModel = null;

        public PnrScriptTreeViewModel()
        {
            _saveSubgroupCommand = new RelayCommand<object>(SaveSubgroup_Executed, SaveSubgroup_CanExecute);
            _renameSubgroupCommand = new RelayCommand<object>(RenameSubgroup_Executed, RenameSubgroup_CanExecute);
            _deleteSubgroupCommand = new RelayCommand<object>(DeleteSubgroup_Executed, DeleteSubgroup_CanExecute);

            _savePnrScriptCommand = new RelayCommand<object>(SavePnrScript_Executed);

            _deleteListItemCommand = new RelayCommand<object>(DeleteListItem_Executed);

            _mouseDoubleClickCommand = new RelayCommand<object>(MouseDoubleClick_Executed, MouseDoubleClick_CanExecute);

            _selectedItemChangedCommand = new RelayCommand<object>(SelectedItemChanged);

            _root = new ObservableCollection<IPnrScriptBaseItemViewModel>();

            IPnrScriptSubgroupViewModel rootItem = new PnrScriptSubgroupViewModel(null, "Root");
            _root.Add(rootItem);

            IPnrScriptSubgroupViewModel maskItem = new PnrScriptSubgroupViewModel(rootItem, "Mask");
            rootItem.AddChildItem(maskItem);

        }

        public ObservableCollection<IPnrScriptBaseItemViewModel> Root
        {
            get
            {
                return _root;
            }
        }

        public IPnrScriptSubgroupViewModel PnrScriptSubgroupToWorkOn
        {
            get
            {
                return _pnrScriptSubgroupToWorkOn;
            }
            set
            {
                _pnrScriptSubgroupToWorkOn = value;
                NotifyPropertyChanged(() => PnrScriptSubgroupToWorkOn);
            }
        }

        public IPnrScriptViewModel PnrScriptToWorkOn
        {
            get
            {
                return _pnrScriptToWorkOn;
            }
            set
            {
                _pnrScriptToWorkOn = value;
                NotifyPropertyChanged(() => PnrScriptToWorkOn);
            }
        }

        public GDSCommandTreeViewModel GDSCmdTreeViewModel
        {
            get
            {
                return _gdsCmdTreeViewModel;
            }
            set
            {
                _gdsCmdTreeViewModel = value;
                NotifyPropertyChanged(() => GDSCmdTreeViewModel);
            }
        }

        public IPnrScriptBaseItemViewModel CurrentlySelectedItem
        {
            get
            {
                return _currentlySelectedItem;
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

        public ICommand RenameSubgroupCommand
        {
            get
            {
                return _renameSubgroupCommand;
            }
            set
            {
                _renameSubgroupCommand = value;
            }
        }

        public ICommand DeleteSubgroupCommand
        {
            get
            {
                return _deleteSubgroupCommand;
            }
            set
            {
                _deleteSubgroupCommand = value;
            }
        }

        public ICommand SavePnrScriptCommand
        {
            get
            {
                return _savePnrScriptCommand;
            }
            set
            {
                _savePnrScriptCommand = value;
            }
        }

        public ICommand DeleteListItemCommand
        {
            get
            {
                return _deleteListItemCommand;
            }
            set
            {
                _deleteListItemCommand = value;
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

        public ICommand MouseDoubleClickCommand
        {
            get
            {
                return _mouseDoubleClickCommand;
            }
            set
            {
                _mouseDoubleClickCommand = value;
            }
        }

        private void SortByDescription(IPnrScriptBaseItemViewModel parent)
        {
            parent.Children.Sort(k => k.Description);
        }

        public void SaveSubgroup_Executed(object obj)
        {
            if (PnrScriptSubgroupToWorkOn != null)
            {
                if (_currentlySelectedItem != null)
                {
                    if (PnrScriptSubgroupToWorkOn.Parent == null)
                    {
                        // this creates a new item
                        IPnrScriptSubgroupViewModel newItem = new PnrScriptSubgroupViewModel(_currentlySelectedItem, PnrScriptSubgroupToWorkOn.Description);
                        _currentlySelectedItem.AddChildItem(newItem);
                        CloseSubgroupWindow();
                        SortByDescription(_currentlySelectedItem);
                    }
                    else
                    {
                        // this renames an item
                        _currentlySelectedItem.Description = PnrScriptSubgroupToWorkOn.Description;
                        CloseSubgroupWindow();
                        if (CurrentlySelectedItem.Parent != null)
                            SortByDescription(_currentlySelectedItem.Parent);
                        else
                            SortByDescription(_currentlySelectedItem);
                    }
                }
            }
        }

        public bool SaveSubgroup_CanExecute(object obj)
        {
            bool result = false;

            if (_pnrScriptSubgroupToWorkOn != null)
            {
                result = !(_pnrScriptSubgroupToWorkOn.HasErrors);
                if (result == true)
                {
                    if (PnrScriptSubgroupToWorkOn.Parent != null)
                        result = !(_currentlySelectedItem.Parent == null);
                }
            }

            return result;
        }

        public void RenameSubgroup_Executed(object obj)
        {
            // do nothing 
        }

        public bool RenameSubgroup_CanExecute(object obj)
        {
            bool result = false;

            IPnrScriptSubgroupViewModel itemToBeRenamed = obj as IPnrScriptSubgroupViewModel;
            if (itemToBeRenamed != null)
                result = (itemToBeRenamed.Parent != null);

            return result;
        }

        public void DeleteSubgroup_Executed(object obj)
        {
            IPnrScriptSubgroupViewModel itemToBeDeleted = obj as IPnrScriptSubgroupViewModel;

            if (itemToBeDeleted != null)
            {
                if (itemToBeDeleted.Children != null)
                {
                    if (itemToBeDeleted.Children.Count == 0)
                    {
                        if (itemToBeDeleted.Parent != null)
                        {
                            _currentlySelectedItem = itemToBeDeleted.Parent;
                            _currentlySelectedItem.Children.Remove(itemToBeDeleted);
                        }
                    }
                }
            }
        }

        public bool DeleteSubgroup_CanExecute(object obj)
        {
            bool result = false;

            IPnrScriptSubgroupViewModel itemToBeDeleted = obj as IPnrScriptSubgroupViewModel;
            if (itemToBeDeleted != null)
                if (itemToBeDeleted.Parent != null)
                    if (itemToBeDeleted.Children != null)
                        result = (itemToBeDeleted.Children.Count == 0);

            return result;
        }

        public void SelectedItemChanged(object obj)
        {
            if (obj is IPnrScriptBaseItemViewModel == false)
                return;

            var item = obj as IPnrScriptBaseItemViewModel;
            _currentlySelectedItem = item;
        }

        public void MouseDoubleClick_Executed(object obj)
        {
            GDSCommandTreeViewModel tvm = obj as GDSCommandTreeViewModel;
            if (tvm != null)
            {
                IGDSCommandViewModel clickedItem = tvm.CurrentlySelectedItem as IGDSCommandViewModel;

                if (clickedItem != null)
                {
                    if (clickedItem.Parent != null)
                    {
                        if (_pnrScriptToWorkOn != null)
                        {
                            _pnrScriptToWorkOn.GDSCommands.Add(clickedItem);
                        }
                    }
                }
            }
        }

        public bool MouseDoubleClick_CanExecute(object obj)
        {
            // todo: this method does not get called
            bool result = false;
            IGDSCommandViewModel clickedItem = obj as IGDSCommandViewModel;

            if (clickedItem != null)
            {
                if (clickedItem.Parent != null)
                {
                    if (clickedItem.Children != null)
                    {
                        result = (clickedItem.Children.Count == 0);
                    }
                }
            }

            return result;
        }

        public void SavePnrScript_Executed(object obj)
        {
            if (PnrScriptToWorkOn != null)
            {
                if (_currentlySelectedItem != null)
                {
                    if (PnrScriptToWorkOn.Parent == null)
                    {
                        // this creates a new item
                        IPnrScriptViewModel newItem = new PnrScriptViewModel(_currentlySelectedItem, PnrScriptToWorkOn.Description, PnrScriptToWorkOn.GDSCommands);
                        _currentlySelectedItem.AddChildItem(newItem);
                        ClosePnrScriptWindow();
                        SortByDescription(_currentlySelectedItem);
                    }
                    else
                    {
                        var existingItem = _currentlySelectedItem as PnrScriptViewModel;
                        if (existingItem != null)
                        {
                            existingItem.Description = _pnrScriptToWorkOn.Description;
                            existingItem.GDSCommands = _pnrScriptToWorkOn.GDSCommands;
                            ClosePnrScriptWindow();
                            if (existingItem.Parent != null)
                                SortByDescription(existingItem.Parent);
                            else
                                SortByDescription(existingItem);
                        }
                    }
                }
            }
        }

        public void DeleteListItem_Executed(object obj)
        {
            _pnrScriptToWorkOn.GDSCommands.Add(new GDSCommandViewModel(null, "New Description", "No command lines"));
        }
    }
}
