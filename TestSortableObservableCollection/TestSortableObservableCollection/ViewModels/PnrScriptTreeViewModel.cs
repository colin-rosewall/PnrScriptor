using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using TestSortableObservableCollection.Interfaces;
using TestSortableObservableCollection.ViewModels;
using TestSortableObservableCollection.AppConstants;
using System.Windows.Input;
using TestSortableObservableCollection.ViewModels.Base;
using TestSortableObservableCollection.Models;


namespace TestSortableObservableCollection.ViewModels
{
    public class PnrScriptTreeViewModel : Base.BaseViewModel
    {
        private ObservableCollection<IPnrScriptBaseItemViewModel> _root = null;
        private ICommand _saveSubgroupCommand = null;
        private ICommand _renameSubgroupCommand = null;
        private ICommand _deleteSubgroupCommand = null;

        //private ICommand _savePnrScriptCommand = null;
        private ICommand _deletePnrScriptCommand = null;
        private ICommand _saveTreeCommand = null;
        private ICommand _cutPnrScriptCommand = null;
        private ICommand _pastePnrScriptCommand = null;

        //private ICommand _removeGDSCmdCommand = null;  // this removes items from the gds cmds listbox

        private ICommand _selectedItemChangedCommand = null;

        //private ICommand _mouseDoubleClickCommand = null;

        private ICommand _generateScriptCommand = null;

        private IPnrScriptBaseItemViewModel _currentlySelectedItem { get; set; }
        private IPnrScriptViewModel _itemToCut { get; set; }
        //private IPnrScriptSubgroupViewModel _pnrScriptSubgroupToWorkOn = null;
        //private IPnrScriptViewModel _pnrScriptToWorkOn = null;
        //public Action CloseSubgroupWindow { get; set; }
        //public Action ClosePnrScriptWindow { get; set; }
        public Action OpenScriptGenerationWindow { get; set; }
        private GDSCommandTreeViewModel _gdsCmdTreeViewModel = null;

        //private IGDSCommandViewModel _currentlySelectedGdsCmd { get; set; }

        private string _generatedScript { get; set; }

        private bool _isDirty = false;

        public PnrScriptTreeViewModel()
        {
            IsDirty = false;
            //_saveSubgroupCommand = new RelayCommand<object>(SaveSubgroup_Executed, SaveSubgroup_CanExecute);
            _renameSubgroupCommand = new RelayCommand<object>(RenameSubgroup_Executed, RenameSubgroup_CanExecute);
            _deleteSubgroupCommand = new RelayCommand<object>(DeleteSubgroup_Executed, DeleteSubgroup_CanExecute);

            //_savePnrScriptCommand = new RelayCommand<object>(SavePnrScript_Executed);
            _deletePnrScriptCommand = new RelayCommand<object>(DeletePnrScript_Executed, DeletePnrScript_CanExecute);
            _cutPnrScriptCommand = new RelayCommand<object>(CutPnrScript_Executed, CutPnrScript_CanExecute);
            _pastePnrScriptCommand = new RelayCommand<object>(PastePnrScript_Executed, PastePnrScript_CanExecute);

            _saveTreeCommand = new RelayCommand<object>(SaveTree_Executed, SaveTree_CanExecute);

            //_removeGDSCmdCommand = new RelayCommand<object>(RemoveGDSCmd_Executed);

            //_mouseDoubleClickCommand = new RelayCommand<object>(MouseDoubleClick_Executed, MouseDoubleClick_CanExecute);

            _generateScriptCommand = new RelayCommand<object>(GenerateScript_Executed);

            _selectedItemChangedCommand = new RelayCommand<object>(SelectedItemChanged);

            _root = new ObservableCollection<IPnrScriptBaseItemViewModel>();

        }

        public bool IsDirty
        {
            get { return _isDirty; }
            set { _isDirty = value; NotifyPropertyChanged(() => IsDirty); }
        }

        public ObservableCollection<IPnrScriptBaseItemViewModel> Root
        {
            get
            {
                return _root;
            }
        }

        //public IPnrScriptSubgroupViewModel PnrScriptSubgroupToWorkOn
        //{
        //    get
        //    {
        //        return _pnrScriptSubgroupToWorkOn;
        //    }
        //    set
        //    {
        //        _pnrScriptSubgroupToWorkOn = value;
        //        NotifyPropertyChanged(() => PnrScriptSubgroupToWorkOn);
        //    }
        //}

        //public IPnrScriptViewModel PnrScriptToWorkOn
        //{
        //    get
        //    {
        //        return _pnrScriptToWorkOn;
        //    }
        //    set
        //    {
        //        _pnrScriptToWorkOn = value;
        //        NotifyPropertyChanged(() => PnrScriptToWorkOn);
        //    }
        //}

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

        //public IGDSCommandViewModel CurrentLySelectedGdsCmd
        //{
        //    get
        //    {
        //        return _currentlySelectedGdsCmd;
        //    }
        //    set
        //    {
        //        _currentlySelectedGdsCmd = value;
        //        NotifyPropertyChanged(() => CurrentLySelectedGdsCmd);
        //    }
        //}

        public string GeneratedScript
        {
            get
            {
                return _generatedScript;
            }
            set
            {
                _generatedScript = value;
                NotifyPropertyChanged(() => GeneratedScript);
            }
        }

        public ICommand GenerateScriptCommand
        {
            get
            {
                return _generateScriptCommand;
            }
            set
            {
                _generateScriptCommand = value;
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

        //public ICommand SavePnrScriptCommand
        //{
        //    get
        //    {
        //        return _savePnrScriptCommand;
        //    }
        //    set
        //    {
        //        _savePnrScriptCommand = value;
        //    }
        //}

        public ICommand DeletePnrScriptCommand
        {
            get
            {
                return _deletePnrScriptCommand;
            }
            set
            {
                _deletePnrScriptCommand = value;
            }
        }

        public ICommand CutPnrScriptCommand
        {
            get
            {
                return _cutPnrScriptCommand;
            }
            set
            {
                _cutPnrScriptCommand = value;
            }
        }

        public ICommand PastePnrScriptCommand
        {
            get
            {
                return _pastePnrScriptCommand;
            }
            set
            {
                _pastePnrScriptCommand = value;
            }
        }

        //public ICommand RemoveGDSCmdCommand
        //{
        //    get
        //    {
        //        return _removeGDSCmdCommand;
        //    }
        //    set
        //    {
        //        _removeGDSCmdCommand = value;
        //    }
        //}

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

        //public ICommand MouseDoubleClickCommand
        //{
        //    get
        //    {
        //        return _mouseDoubleClickCommand;
        //    }
        //    set
        //    {
        //        _mouseDoubleClickCommand = value;
        //    }
        //}

        private void SortByDescription(IPnrScriptBaseItemViewModel parent)
        {
            parent.Children.Sort(k => k.Description);
        }

        //public void SaveSubgroup_Executed(object obj)
        //{
        //    if (PnrScriptSubgroupToWorkOn != null)
        //    {
        //        if (_currentlySelectedItem != null)
        //        {
        //            if (PnrScriptSubgroupToWorkOn.Parent == null)
        //            {
        //                // this creates a new item
        //                IPnrScriptSubgroupViewModel newItem = new PnrScriptSubgroupViewModel(_currentlySelectedItem, PnrScriptSubgroupToWorkOn.Description);
        //                _currentlySelectedItem.AddChildItem(newItem);
        //                IsDirty = true;
        //                CloseSubgroupWindow();
        //                SortByDescription(_currentlySelectedItem);
        //            }
        //            else
        //            {
        //                // this renames an item
        //                _currentlySelectedItem.Description = PnrScriptSubgroupToWorkOn.Description;
        //                IsDirty = true;
        //                CloseSubgroupWindow();
        //                if (CurrentlySelectedItem.Parent != null)
        //                    SortByDescription(_currentlySelectedItem.Parent);
        //                else
        //                    SortByDescription(_currentlySelectedItem);
        //            }
        //        }
        //    }
        //}

        //public bool SaveSubgroup_CanExecute(object obj)
        //{
        //    bool result = false;

        //    if (_pnrScriptSubgroupToWorkOn != null)
        //    {
        //        result = !(_pnrScriptSubgroupToWorkOn.HasErrors);
        //        if (result == true)
        //        {
        //            if (PnrScriptSubgroupToWorkOn.Parent != null)
        //                result = !(_currentlySelectedItem.Parent == null);
        //        }
        //    }

        //    return result;
        //}

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
                            IsDirty = true;
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

        //public void MouseDoubleClick_Executed(object obj)
        //{
        //    GDSCommandTreeViewModel tvm = obj as GDSCommandTreeViewModel;
        //    if (tvm != null)
        //    {
        //        IGDSCommandViewModel clickedItem = tvm.CurrentlySelectedItem as IGDSCommandViewModel;

        //        if (clickedItem != null)
        //        {
        //            if (clickedItem.Parent != null)
        //            {
        //                if (_pnrScriptToWorkOn != null)
        //                {
        //                    IGDSCommandViewModel newItem = new GDSCommandViewModel(clickedItem.Parent, clickedItem.Description, clickedItem.CommandLines, clickedItem.Guid);
        //                    _pnrScriptToWorkOn.GDSCommands.Add(newItem);
        //                }
        //            }
        //        }
        //    }
        //}

        //public bool MouseDoubleClick_CanExecute(object obj)
        //{
        //    // todo: this method does not get called
        //    bool result = false;
        //    IGDSCommandViewModel clickedItem = obj as IGDSCommandViewModel;

        //    if (clickedItem != null)
        //    {
        //        if (clickedItem.Parent != null)
        //        {
        //            if (clickedItem.Children != null)
        //            {
        //                result = (clickedItem.Children.Count == 0);
        //            }
        //        }
        //    }

        //    return result;
        //}

        //public void SavePnrScript_Executed(object obj)
        //{
        //    if (PnrScriptToWorkOn != null)
        //    {
        //        if (_currentlySelectedItem != null)
        //        {
        //            if (PnrScriptToWorkOn.Parent == null)
        //            {
        //                // this creates a new item
        //                IPnrScriptViewModel newItem = new PnrScriptViewModel(_currentlySelectedItem, PnrScriptToWorkOn.Description, PnrScriptToWorkOn.GDSCommands);
        //                _currentlySelectedItem.AddChildItem(newItem);
        //                IsDirty = true;
        //                ClosePnrScriptWindow();
        //                SortByDescription(_currentlySelectedItem);
        //            }
        //            else
        //            {
        //                var existingItem = _currentlySelectedItem as PnrScriptViewModel;
        //                if (existingItem != null)
        //                {
        //                    existingItem.Description = _pnrScriptToWorkOn.Description;
        //                    existingItem.GDSCommands = _pnrScriptToWorkOn.GDSCommands;
        //                    IsDirty = true;
        //                    ClosePnrScriptWindow();
        //                    if (existingItem.Parent != null)
        //                        SortByDescription(existingItem.Parent);
        //                    else
        //                        SortByDescription(existingItem);
        //                }
        //            }
        //        }
        //    }
        //}

        public void SaveNotification(IPnrScriptBaseItemViewModel obj, Constants.WindowMode wm)
        {
            // obj should be the parent of the item added or changed
            if (obj != null)
            {
                IsDirty = true;
                if (wm == Constants.WindowMode.Add)
                {
                    SortByDescription(obj);
                }
                else if (wm == Constants.WindowMode.Change)
                {
                    SortByDescription(obj);
                }
            }
        }

        public void DeletePnrScript_Executed(object obj)
        {
            IPnrScriptViewModel itemToBeDeleted = obj as IPnrScriptViewModel;

            if (itemToBeDeleted != null)
            {
                if (itemToBeDeleted.Parent != null)
                {
                    _currentlySelectedItem = itemToBeDeleted.Parent;
                    _currentlySelectedItem.Children.Remove(itemToBeDeleted);
                    IsDirty = true;
                }
            }
        }

        public bool DeletePnrScript_CanExecute(object obj)
        {
            bool result = true;

            return result;
        }

        public void CutPnrScript_Executed(object obj)
        {
            IPnrScriptViewModel itemToBeCut = obj as IPnrScriptViewModel;

            if (itemToBeCut != null && itemToBeCut.Parent != null)
            {
                _itemToCut = itemToBeCut;
            }
        }

        public bool CutPnrScript_CanExecute(object obj)
        {
            bool result = true;

            return result;
        }

        public void PastePnrScript_Executed(object obj)
        {
            IPnrScriptSubgroupViewModel itemToPasteInto = obj as IPnrScriptSubgroupViewModel;

            if (itemToPasteInto != null)
            {
                if (_itemToCut != null)
                {
                    IPnrScriptViewModel newItem = new PnrScriptViewModel(Constants.WindowMode.None, itemToPasteInto, _itemToCut.Description, null, _itemToCut.GDSCommands, null, null);
                    itemToPasteInto.AddChildItem(newItem);
                    IsDirty = true;
                    SortByDescription(itemToPasteInto);

                    var parent = _itemToCut.Parent;
                    if (parent != null)
                    {
                        parent.Children.Remove(_itemToCut);
                        _itemToCut = null;
                    }
                }
            }
        }

        public bool PastePnrScript_CanExecute(object obj)
        {
            bool result = false;

            if (_itemToCut != null)
                result = true;

            return result;
        }

        //public void RemoveGDSCmd_Executed(object obj)
        //{
        //    if (_currentlySelectedGdsCmd != null)
        //    {
        //        if (PnrScriptToWorkOn != null)
        //        {
        //            if (PnrScriptToWorkOn.GDSCommands != null)
        //            {
        //                PnrScriptToWorkOn.GDSCommands.Remove(_currentlySelectedGdsCmd);
        //                _currentlySelectedGdsCmd = null;
        //            }
        //        }
        //    }
        //}

        public void SaveTree_Executed(object obj)
        {
            if (IsDirty)
            {
                IPnrScriptTreeModel model = PnrScriptTreeModelFactory.GetModel("001");
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

        public void GenerateScript_Executed(object obj)
        {
            IPnrScriptViewModel selectedItem = obj as IPnrScriptViewModel;

            if (selectedItem != null)
            {
                string scriptText = string.Join(Environment.NewLine, selectedItem.GDSCommands.Select(cmd => cmd.CommandLines));
                GeneratedScript = scriptText;
                OpenScriptGenerationWindow();
            }
        }

        public void UpdateGDSCmdToPnrScriptTVM(IGDSCommandViewModel itemUsedForUpdating)
        {
            if (itemUsedForUpdating.Guid.Length > 0)
            {
                IPnrScriptTreeModel pnrScriptsModel = PnrScriptTreeModelFactory.GetModel("001");
                pnrScriptsModel.UpdateTree(this, itemUsedForUpdating);
            }
        }
    }
}
