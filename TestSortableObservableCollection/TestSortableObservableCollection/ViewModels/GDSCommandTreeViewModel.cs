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
using TestSortableObservableCollection.Models;

namespace TestSortableObservableCollection.ViewModels
{
    public class GDSCommandTreeViewModel : Base.BaseViewModel
    {
        private ObservableCollection<IGDSCommandItemViewModel> _root = null;
        private ICommand _saveSubgroupCommand = null;
        private ICommand _renameSubgroupCommand = null;
        private ICommand _deleteSubgroupCommand = null;

        private ICommand _saveGDSCmdCommand = null;
        private ICommand _deleteGDSCmdCommand = null;
        private ICommand _saveTreeCommand = null;
        private ICommand _cutGDSCmdCommand = null;
        private ICommand _pasteGDSCmdCommand = null;

        private ICommand _selectedItemChangedCommand = null;
        private IGDSCommandItemViewModel _currentlySelectedItem { get; set; }
        private IGDSCommandViewModel _itemToCut { get; set; }
        private IGDSCommandSubgroupViewModel _GDSSubgroupToWorkOn = null;
        private IGDSCommandViewModel _GDSCommandToWorkOn = null;
        public Action CloseSubgroupWindow { get; set; }
        public Action CloseGDSCommandWindow { get; set; }

        public delegate void AddGDSCmdToCacheDelegate(IGDSCommandViewModel newItem);
        public event AddGDSCmdToCacheDelegate RaiseAddGDSCmdToCache;

        public GDSCommandTreeViewModel()
        {
            _saveSubgroupCommand = new RelayCommand<object>(SaveSubgroup_Executed, SaveSubgroup_CanExecute);
            _renameSubgroupCommand = new RelayCommand<object>(RenameSubgroup_Executed, RenameSubgroup_CanExecute);
            _deleteSubgroupCommand = new RelayCommand<object>(DeleteSubgroup_Executed, DeleteSubgroup_CanExecute);

            _saveGDSCmdCommand = new RelayCommand<object>(SaveGDSCmd_Executed);
            _deleteGDSCmdCommand = new RelayCommand<object>(DeleteGDSCmd_Executed, DeleteGDSCmd_CanExecute);
            _cutGDSCmdCommand = new RelayCommand<object>(CutGDSCmd_Executed, CutGDSCmd_CanExecute);
            _pasteGDSCmdCommand = new RelayCommand<object>(PasteGDSCmd_Executed, PasteGDSCmd_CanExecute);

            _saveTreeCommand = new RelayCommand<object>(SaveTree_Executed, SaveTree_CanExecute);

            _selectedItemChangedCommand = new RelayCommand<object>(SelectedItemChanged);

            _root = new ObservableCollection<IGDSCommandItemViewModel>();
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

        public IGDSCommandViewModel GDSCommandToWorkOn
        {
            get
            {
                return _GDSCommandToWorkOn;
            }
            set
            {
                _GDSCommandToWorkOn = value;
                NotifyPropertyChanged(() => GDSCommandToWorkOn);
            }
        }

        public IGDSCommandItemViewModel CurrentlySelectedItem
        {
            get
            {
                return _currentlySelectedItem;
            }
        }

        public ICommand SaveGDSCmdCommand
        {
            get
            {
                return _saveGDSCmdCommand;
            }
            set
            {
                _saveGDSCmdCommand = value;
            }
        }

        public ICommand DeleteGDSCmdCommand
        {
            get
            {
                return _deleteGDSCmdCommand;
            }
            set
            {
                _deleteGDSCmdCommand = value;
            }
        }

        public ICommand CutGDSCmdCommand
        {
            get
            {
                return _cutGDSCmdCommand;
            }
            set
            {
                _cutGDSCmdCommand = value;
            }
        }

        public ICommand PasteGDSCmdCommand
        {
            get
            {
                return _pasteGDSCmdCommand;
            }
            set
            {
                _pasteGDSCmdCommand = value;
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

        public ICommand SaveTreeCommand
        {
            get
            {
                return _saveTreeCommand;
            }
            set
            {
                _saveTreeCommand = value;
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

        private void SortByDescription(IGDSCommandItemViewModel parent)
        {
            parent.Children.Sort(k => k.Description);
        }

        public void SaveGDSCmd_Executed(object obj)
        {
            if (GDSCommandToWorkOn != null)
            {
                if (_currentlySelectedItem != null)
                {
                    if (GDSCommandToWorkOn.Parent == null)
                    {
                        // this creates a new item
                        IGDSCommandViewModel newItem = new GDSCommandViewModel(_currentlySelectedItem, GDSCommandToWorkOn.Description, GDSCommandToWorkOn.CommandLines, System.Guid.NewGuid().ToString());
                        _currentlySelectedItem.AddChildItem(newItem);
                        // ToDo: add newItem to GDSCmdCache
                        if (RaiseAddGDSCmdToCache != null)
                            RaiseAddGDSCmdToCache(newItem);
                        CloseGDSCommandWindow();
                        SortByDescription(_currentlySelectedItem);
                    }
                    else
                    {
                        var existingItem = _currentlySelectedItem as GDSCommandViewModel;
                        if (existingItem != null)
                        {
                            existingItem.Description = GDSCommandToWorkOn.Description;
                            existingItem.CommandLines = GDSCommandToWorkOn.CommandLines;
                            // ToDo: update GDSCmdCache with existingItem description and command lines
                            CloseGDSCommandWindow();
                            if (existingItem.Parent != null)
                                SortByDescription(existingItem.Parent);
                            else
                                SortByDescription(existingItem);
                        }
                    }
                }
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
                        // this creates a new item
                        IGDSCommandSubgroupViewModel newItem = new GDSCommandSubgroupViewModel(_currentlySelectedItem, GDSSubgroupToWorkOn.Description);
                        _currentlySelectedItem.AddChildItem(newItem);
                        CloseSubgroupWindow();
                        SortByDescription(_currentlySelectedItem);
                    }
                    else
                    {
                        // this renames an item
                        _currentlySelectedItem.Description = GDSSubgroupToWorkOn.Description;
                        CloseSubgroupWindow();
                        if (_currentlySelectedItem.Parent != null)
                            SortByDescription(_currentlySelectedItem.Parent);
                        else
                            SortByDescription(_currentlySelectedItem);
                    }
                }
            }
        }

        public void RenameSubgroup_Executed(object obj)
        {
            // do nothing 
        }

        public bool RenameSubgroup_CanExecute(object obj)
        {
            bool result = false;

            IGDSCommandSubgroupViewModel itemToBeRenamed = obj as IGDSCommandSubgroupViewModel;
            if (itemToBeRenamed != null)
                result = (itemToBeRenamed.Parent != null);

            return result;
        }

        public bool SaveSubgroup_CanExecute(object obj)
        {
            bool result = false;

            if (_GDSSubgroupToWorkOn != null)
            {
                result = !(_GDSSubgroupToWorkOn.HasErrors);
                if (result == true)
                {
                    if (GDSSubgroupToWorkOn.Parent != null)
                        result = !(_currentlySelectedItem.Parent == null);
                }
            }

            return result;
        }

        public void DeleteGDSCmd_Executed(object obj)
        {
            IGDSCommandViewModel itemToBeDeleted = obj as IGDSCommandViewModel;

            if (itemToBeDeleted != null)
            {
                if (itemToBeDeleted.Parent != null)
                {
                    _currentlySelectedItem = itemToBeDeleted.Parent;
                    _currentlySelectedItem.Children.Remove(itemToBeDeleted);
                    // ToDo: Remove itemToBeDeleted from GDSCmdCache
                }
            }
        }

        public void CutGDSCmd_Executed(object obj)
        {
            IGDSCommandViewModel itemToBeCut = obj as IGDSCommandViewModel;

            if (itemToBeCut != null && itemToBeCut.Parent != null)
            {
                _itemToCut = itemToBeCut;
            }
        }

        public bool CutGDSCmd_CanExecute(object obj)
        {
            bool result = true;

            return result;
        }

        public void PasteGDSCmd_Executed(object obj)
        {
            IGDSCommandSubgroupViewModel itemToPasteInto = obj as IGDSCommandSubgroupViewModel;

            if (itemToPasteInto != null)
            {
                if (_itemToCut != null)
                {
                    IGDSCommandViewModel newItem = new GDSCommandViewModel(itemToPasteInto, _itemToCut.Description, _itemToCut.CommandLines, _itemToCut.Guid);
                    itemToPasteInto.AddChildItem(newItem);
                    // ToDo: we do not need to update GDSCmdCache with the new parent itemToPasteInto because the parent is not set when the cache is loaded from file

                    var parent = _itemToCut.Parent;
                    if (parent != null)
                    {
                        parent.Children.Remove(_itemToCut);
                        _itemToCut = null;
                    }
                }
            }
        }

        public bool PasteGDSCmd_CanExecute(object obj)
        {
            bool result = false;

            if (_itemToCut != null)
                result = true;

            return result;
        }

        public void DeleteSubgroup_Executed(object obj)
        {
            IGDSCommandSubgroupViewModel itemToBeDeleted = obj as IGDSCommandSubgroupViewModel;

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

        public bool DeleteGDSCmd_CanExecute(object obj)
        {
            bool result = true;

            return result;
        }

        public bool DeleteSubgroup_CanExecute(object obj)
        {
            bool result = false;

            IGDSCommandSubgroupViewModel itemToBeDeleted = obj as IGDSCommandSubgroupViewModel;
            if (itemToBeDeleted != null)
                if (itemToBeDeleted.Parent != null)
                    if (itemToBeDeleted.Children != null)
                        result = (itemToBeDeleted.Children.Count == 0);

            return result;
        }

        public void SelectedItemChanged(object obj)
        {
            if (obj is IGDSCommandItemViewModel == false)
                return;

            var item = obj as IGDSCommandItemViewModel;
            _currentlySelectedItem = item;
        }

        public void SaveTree_Executed(object obj)
        {
            IGDSCmdTreeModel model = GDSCmdTreeModelFactory.GetModel("002");
            model.SaveTree(this);
        }

        public bool SaveTree_CanExecute(object obj)
        {
            bool result = false;

            if (_root != null && _root.Count > 0)
                result = true;

            return result;
        }
    }

}
