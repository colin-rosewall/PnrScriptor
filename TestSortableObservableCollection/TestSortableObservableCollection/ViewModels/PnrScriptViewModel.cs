using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TestSortableObservableCollection.Interfaces;
using TestSortableObservableCollection.ViewModels;
using TestSortableObservableCollection.AppConstants;
using System.Windows.Input;
using TestSortableObservableCollection.ViewModels.Base;

namespace TestSortableObservableCollection.ViewModels
{
    public class PnrScriptViewModel : Base.BaseViewModel, IPnrScriptViewModel
    {
        private UInt64 _uniqueID;
        private string _description = null;
        private ObservableCollection<IGDSCommandViewModel> _scriptOfGDSCmds = null;
        private IPnrScriptBaseItemViewModel _parent;
        private SortableObservableCollection<IPnrScriptBaseItemViewModel> _children = null;
        private bool _IsItemExpanded = false;
        private bool _IsItemSelected = false;
        private Dictionary<string, List<string>> _validationErrors = null;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public delegate void CallBackDelegate(IPnrScriptBaseItemViewModel obj );
        public event CallBackDelegate RaiseCallBackDelegate = null;
        private CallBackDelegate _myCallBack = null;

        private Constants.WindowMode _currentWindowMode;

        private GDSCommandTreeViewModel _availableGDSCmdTVM = null;

        private ICommand _savePnrScriptCommand = null;

        private ICommand _mouseDoubleClickCommand = null;

        public PnrScriptViewModel()
        {
            _description = string.Empty;
            _scriptOfGDSCmds = new ObservableCollection<IGDSCommandViewModel>();
            _children = new SortableObservableCollection<IPnrScriptBaseItemViewModel>();
            _validationErrors = new Dictionary<string, List<string>>();
            RaiseCallBackDelegate = null;
            _savePnrScriptCommand = new RelayCommand<object>(SavePnrScript_Executed);
            _mouseDoubleClickCommand = new RelayCommand<object>(MouseDoubleClick_Executed);
        }

        public PnrScriptViewModel(Constants.WindowMode mode, IPnrScriptBaseItemViewModel parent, string theDescription, GDSCommandTreeViewModel gdsCmdsTVM, ObservableCollection<IGDSCommandViewModel> gdsCmds, CallBackDelegate saveNotification) : this()
        {
            _currentWindowMode = mode;
            _parent = parent;
            _description = theDescription;
            _availableGDSCmdTVM = gdsCmdsTVM;
            _scriptOfGDSCmds = CloningExtensions.DeepCopy(gdsCmds);
            _myCallBack = saveNotification;
        }

        public SortableObservableCollection<IPnrScriptBaseItemViewModel> Children
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
                    ValidateDescription(_description, () => Description);
                    NotifyPropertyChanged(() => Description);
                }
            }
        }

        public GDSCommandTreeViewModel AvailableGDSCmdsTVM
        {
            get
            {
                return _availableGDSCmdTVM;
            }
        }

        public ObservableCollection<IGDSCommandViewModel> GDSCommands
        {
            get
            {
                return _scriptOfGDSCmds;
            }
            set
            {
                if (_scriptOfGDSCmds != value)
                {
                    _scriptOfGDSCmds = value;
                    NotifyPropertyChanged(() => GDSCommands);
                }
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

        public IPnrScriptBaseItemViewModel Parent
        {
            get
            {
                return _parent;
            }
        }

        public ulong UniqueID
        {
            get
            {
                return _uniqueID;
            }

            set
            {
                _uniqueID = value;
            }
        }

        public bool HasErrors
        {
            get
            {
                return _validationErrors.Any();
            }
        }

        public void AddChildItem(IPnrScriptBaseItemViewModel item)
        {
            _children.Add(item);
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public IEnumerable GetErrors(string propertyName)
        {
            List<string> errorMessages = new List<string>();
            if (propertyName != null)
            {
                _validationErrors.TryGetValue(propertyName, out errorMessages);
                return errorMessages;
            }
            else
            {
                return null;
            }
        }

        private void RaiseErrorsChanged(string propertyName)
        {
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void ValidateDescription(string newValue, Expression<Func<string>> propName)
        {
            const string descriptionMissing = "Pnr Script Description cannot be empty.";

            var lambda = (LambdaExpression)propName;
            MemberExpression memberExpression;
            string memberName = null;

            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)lambda.Body;
            }
            memberName = memberExpression.Member.Name;

            newValue = newValue.Trim();
            if (string.IsNullOrEmpty(newValue))
            {
                if (_validationErrors.ContainsKey(memberName))
                {
                    List<string> existingMessages = null;
                    if (_validationErrors.TryGetValue(memberName, out existingMessages))
                    {
                        if (existingMessages != null)
                        {
                            if (!existingMessages.Exists(msg => msg.Equals(descriptionMissing)))
                            {
                                _validationErrors[memberName].Add(descriptionMissing);
                            }
                        }
                    }
                }
                else
                {
                    _validationErrors.Add(memberName, new List<string> { descriptionMissing });
                    RaiseErrorsChanged(memberName);
                }
            }
            else if (_validationErrors.ContainsKey(memberName))
            {
                _validationErrors.Remove(memberName);
                RaiseErrorsChanged(memberName);
            }
        }

        public void SavePnrScript_Executed(object obj)
        {
            if (_currentWindowMode == Constants.WindowMode.Add)
            {
                _parent.AddChildItem(this);
                if (_myCallBack != null)
                    _myCallBack(_parent);
                // close this window
            }
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
                        IGDSCommandViewModel newItem = new GDSCommandViewModel(clickedItem.Parent, clickedItem.Description, clickedItem.CommandLines, clickedItem.Guid);
                        GDSCommands.Add(newItem);
                    }
                }
            }
        }

    }
}
