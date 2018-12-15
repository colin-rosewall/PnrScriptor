using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using TestSortableObservableCollection.AppConstants;
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
        private ICommand _renameSubgroupCommand = null;
        private ICommand _deleteSubgroupCommand = null;

        private ICommand _deleteGDSCmdCommand = null;
        private ICommand _saveTreeCommand = null;
        private ICommand _cutGDSCmdCommand = null;
        private ICommand _pasteGDSCmdCommand = null;

        private ICommand _selectedItemChangedCommand = null;
        private IGDSCommandItemViewModel _currentlySelectedItem { get; set; }
        private IGDSCommandViewModel _itemToCut { get; set; }

        public UpdatePnrScriptTVMDelegate _updatePnrScriptTVM = null;  // this only gets used when changing an existing gds command

        private bool _isDirty = false;

        public GDSCommandTreeViewModel()
        {
            IsDirty = false;
            _renameSubgroupCommand = new RelayCommand<object>(RenameSubgroup_Executed, RenameSubgroup_CanExecute);
            _deleteSubgroupCommand = new RelayCommand<object>(DeleteSubgroup_Executed, DeleteSubgroup_CanExecute);

            _deleteGDSCmdCommand = new RelayCommand<object>(DeleteGDSCmd_Executed, DeleteGDSCmd_CanExecute);
            _cutGDSCmdCommand = new RelayCommand<object>(CutGDSCmd_Executed, CutGDSCmd_CanExecute);
            _pasteGDSCmdCommand = new RelayCommand<object>(PasteGDSCmd_Executed, PasteGDSCmd_CanExecute);

            _saveTreeCommand = new RelayCommand<object>(SaveTree_Executed, SaveTree_CanExecute);

            _selectedItemChangedCommand = new RelayCommand<object>(SelectedItemChanged);

            _root = new ObservableCollection<IGDSCommandItemViewModel>();
        }

        public bool IsDirty
        {
            get { return _isDirty; }
            set { _isDirty = value; NotifyPropertyChanged(() => IsDirty); }
        }
        public ObservableCollection<IGDSCommandItemViewModel> Root
        {
            get
            {
                return _root;
            }
        }

        public IGDSCommandItemViewModel CurrentlySelectedItem
        {
            get
            {
                return _currentlySelectedItem;
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

        public void RenameSubgroup_Executed(object obj)
        {
            // we do not need to do anything here
        }

        public bool RenameSubgroup_CanExecute(object obj)
        {
            bool result = false;

            IGDSCommandSubgroupViewModel itemToBeRenamed = obj as IGDSCommandSubgroupViewModel;
            if (itemToBeRenamed != null)
                result = (itemToBeRenamed.Parent != null);

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
                    IsDirty = true;

                    GDSCmdCache.DeleteGDSCmdFromCache(itemToBeDeleted);
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
                    IGDSCommandViewModel newItem = new GDSCommandViewModel(Constants.WindowMode.None, itemToPasteInto, _itemToCut.Description, _itemToCut.CommandLines, _itemToCut.Guid, null, null);
                    itemToPasteInto.AddChildItem(newItem);
                    IsDirty = true;
                    // We do not need to update GDSCmdCache with the new parent itemToPasteInto because the parent is not set when the cache is loaded from file

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
                            IsDirty = true;
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
            if (IsDirty)
            {
                IGDSCmdTreeModel model = GDSCmdTreeModelFactory.GetModel("002");
                model.SaveTree(this);
                IsDirty = false;
            }
        }

        public bool SaveTree_CanExecute(object obj)
        {
            bool result = false;

            if (_root != null && _root.Count > 0)
                result = true;

            return result;
        }

        public void SaveNotification(IGDSCommandItemViewModel parent, Constants.WindowMode wm)
        {
            if (parent != null)
            {
                IsDirty = true;
                if (wm == Constants.WindowMode.Add)
                {
                    SortByDescription(parent);
                }
                else if (wm == Constants.WindowMode.Change)
                {
                    SortByDescription(parent);
                }
            }
        }

    }

}
