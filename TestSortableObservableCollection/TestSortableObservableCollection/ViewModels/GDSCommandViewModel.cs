﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TestSortableObservableCollection.Interfaces;
using TestSortableObservableCollection.AppConstants;
using System.Windows.Input;
using TestSortableObservableCollection.ViewModels.Base;
using TestSortableObservableCollection.Models;

namespace TestSortableObservableCollection.ViewModels
{
    public delegate void UpdatePnrScriptTVMDelegate(IGDSCommandViewModel updatedItem);

    public class GDSCommandViewModel : Base.BaseViewModel, IGDSCommandViewModel
    {
        private UInt64 _uniqueID;
        private string _description = null;
        private string _commandLines = null;
        private IGDSCommandItemViewModel _parent = null;
        private GDSCommandViewModel _originalItem; // only used when changing an existing gds command
        private SortableObservableCollection<IGDSCommandItemViewModel> _children = null;
        private bool _IsItemExpanded = false;
        private bool _IsItemSelected = false;
        private Dictionary<string, List<string>> _validationErrors = null;
        private string _guid = string.Empty;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public delegate void CallBackDelegate(IGDSCommandItemViewModel obj, Constants.WindowMode wm);
        private CallBackDelegate _myCallBack = null;

        
        public UpdatePnrScriptTVMDelegate _updatePnrScriptTVM = null;  // only used when changing an existing gds command

        private Constants.WindowMode _currentWindowMode;

        private ICommand _saveGDSCmdCommand = null;

        public Action CloseGDSCommandWindow { get; set; }

        public GDSCommandViewModel()
        {
            _description = string.Empty;
            _commandLines = string.Empty;
            _children = new SortableObservableCollection<IGDSCommandItemViewModel>();
            _validationErrors = new Dictionary<string, List<string>>();
            _saveGDSCmdCommand = new RelayCommand<object>(SaveGDSCmd_Executed);
            _originalItem = null;
        }

        public GDSCommandViewModel(IGDSCommandItemViewModel parent, string theDescription, string theCommandLines) : this()
        {
            _parent = parent;
            _description = theDescription;
            _commandLines = theCommandLines;
        }

        public GDSCommandViewModel(Constants.WindowMode mode, IGDSCommandItemViewModel parent, string theDescription, string theCommandLines, string guid, CallBackDelegate saveNotification, GDSCommandViewModel originalItem) : this(parent, theDescription, theCommandLines)
        {
            _currentWindowMode = mode;
            _guid = guid;
            _myCallBack = saveNotification;
            _originalItem = originalItem;
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

        public Constants.WindowMode CurrentWindowMode
        {
            get
            {
                return _currentWindowMode;
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
            return MemberwiseClone();
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
            const string descriptionMissing = "GDS Command Description cannot be empty.";

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

        public void SaveGDSCmd_Executed(object obj)
        {
            if (_currentWindowMode == Constants.WindowMode.Add)
            {
                _parent.AddChildItem(this);
                if (_myCallBack != null)
                    _myCallBack(_parent, _currentWindowMode);

                GDSCmdCache.AddGDSCmdToCache(this);

                if (CloseGDSCommandWindow != null)
                    CloseGDSCommandWindow();
            }
            else if (_currentWindowMode == Constants.WindowMode.Change)
            {
                if (_originalItem != null)
                {
                    _originalItem.Description = Description;
                    _originalItem.CommandLines = CommandLines;
                    
                    // we need to pass this change to pnrScriptTreeViewModel.UpdateGDSCmdToPnrScriptTVM so we do it via a delegate
                    if (_updatePnrScriptTVM != null)
                        _updatePnrScriptTVM(_originalItem);

                    if (_myCallBack != null)
                        _myCallBack(_parent, _currentWindowMode);

                    GDSCmdCache.UpdateGDSCmdToCache(this);

                    if (CloseGDSCommandWindow != null)
                        CloseGDSCommandWindow();
                }
            }
        }
    }


}
