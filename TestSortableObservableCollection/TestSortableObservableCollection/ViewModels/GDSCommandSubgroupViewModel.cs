using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Input;
using TestSortableObservableCollection.Interfaces;
using TestSortableObservableCollection.AppConstants;
using TestSortableObservableCollection.ViewModels.Base;

namespace TestSortableObservableCollection.ViewModels
{
    public class GDSCommandSubgroupViewModel : Base.BaseViewModel, IGDSCommandSubgroupViewModel
    {
        private UInt64 _uniqueID;
        private string _description = null;
        private IGDSCommandItemViewModel _parent = null;
        private GDSCommandSubgroupViewModel _originalItem; // only used when changing an existing subgroup
        private SortableObservableCollection<IGDSCommandItemViewModel> _children = null;
        private bool _IsItemExpanded = false;
        private bool _IsItemSelected = false;
        private Dictionary<string, List<string>> _validationErrors = null;
        private string _guid = string.Empty;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public delegate void CallBackDelegate(IGDSCommandItemViewModel obj, Constants.WindowMode wm);
        private CallBackDelegate _myCallBack = null;
        private Constants.WindowMode _currentWindowMode;
        private ICommand _saveSubgroupCommand = null;
        public Action CloseSubgroupWindow { get; set; }

        public GDSCommandSubgroupViewModel()
        {
            _description = string.Empty;
            _children = new SortableObservableCollection<IGDSCommandItemViewModel>();
            _validationErrors = new Dictionary<string, List<string>>();
            _saveSubgroupCommand = new RelayCommand<object>(SaveSubgroup_Executed, SaveSubgroup_CanExecute);
            _originalItem = null;
        }

        public GDSCommandSubgroupViewModel(Constants.WindowMode mode, IGDSCommandItemViewModel parent, string theDescription, CallBackDelegate saveNotification, GDSCommandSubgroupViewModel originalItem) : this()
        {
            _currentWindowMode = mode;
            _parent = parent;
            _description = theDescription;
            _myCallBack = saveNotification;
            _originalItem = originalItem;
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

        public Constants.WindowMode CurrentWindowMode
        {
            get { return _currentWindowMode; }
        }

        public string Guid
        {
            get
            {
                return _guid;
            }
            set
            {
                _guid = value;
            }
        }

        public UInt64 UniqueID
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

        [XmlIgnore]
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
                    ValidateDescription(_description, () => Description);
                    NotifyPropertyChanged(() => Description);
                }
            }
        }

        [XmlIgnore]
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

        [XmlIgnore]
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

        [XmlIgnore]
        public IGDSCommandItemViewModel Parent
        {
            get
            {
                return _parent;
            }
        }

        [XmlIgnore]
        public bool HasErrors
        {
            get
            {
                return _validationErrors.Any();
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
            const string descriptionMissing = "Subgroup Description cannot be empty.";

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

        public void SaveSubgroup_Executed(object obj)
        {
            if (_currentWindowMode == Constants.WindowMode.Add)
            {
                _parent.AddChildItem(this);
                if (_myCallBack != null)
                    _myCallBack(_parent, _currentWindowMode);
                if (CloseSubgroupWindow != null)
                    CloseSubgroupWindow();
            }
            else if (_currentWindowMode == Constants.WindowMode.Change)
            {
                if (_originalItem != null)
                {
                    _originalItem.Description = Description;
                    if (_myCallBack != null)
                        _myCallBack(_parent, _currentWindowMode);
                    if (CloseSubgroupWindow != null)
                        CloseSubgroupWindow();
                }
            }
        }

        public bool SaveSubgroup_CanExecute(object obj)
        {
            bool result = false;

            result = !HasErrors;

            return result;
        }

    }
}
