﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSortableObservableCollection.Interfaces;

namespace TestSortableObservableCollection.ViewModels
{
    public class PnrScriptViewModel : Base.BaseViewModel, IPnrScriptViewModel
    {
        private UInt64 _uniqueID;
        private string _description = null;
        private ObservableCollection<IGDSCommandViewModel> _gdsCmds = null;
        private IPnrScriptBaseItemViewModel _parent;
        private SortableObservableCollection<IPnrScriptBaseItemViewModel> _children = null;
        private bool _IsItemExpanded = false;
        private bool _IsItemSelected = false;
        private Dictionary<string, List<string>> _validationErrors = null;

        // public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public PnrScriptViewModel()
        {
            _description = string.Empty;
            _gdsCmds = new ObservableCollection<IGDSCommandViewModel>();
            _children = new SortableObservableCollection<IPnrScriptBaseItemViewModel>();
            _validationErrors = new Dictionary<string, List<string>>();
        }

        public PnrScriptViewModel(IPnrScriptBaseItemViewModel parent, string theDescription) : this()
        {
            _parent = parent;
            _description = theDescription;
            // todo: set _gdsCmds here
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
                    // ValidateDescription(_description, () => Description);
                    NotifyPropertyChanged(() => Description);
                }
            }
        }

        public ObservableCollection<IGDSCommandViewModel> GDSCommands
        {
            get
            {
                return _gdsCmds;
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

        //public bool HasErrors
        //{
        //    get
        //    {
        //        return _validationErrors.Any();
        //    }
        //}

        public void AddChildItem(IPnrScriptBaseItemViewModel item)
        {
            _children.Add(item);
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        //public IEnumerable GetErrors(string propertyName)
        //{
        //    List<string> errorMessages = new List<string>();
        //    if (propertyName != null)
        //    {
        //        _validationErrors.TryGetValue(propertyName, out errorMessages);
        //        return errorMessages;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //private void RaiseErrorsChanged(string propertyName)
        //{
        //    if (ErrorsChanged != null)
        //        ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        //}

        //private void ValidateDescription(string newValue, Expression<Func<string>> propName)
        //{
        //    const string descriptionMissing = "GDS Command Description cannot be empty.";

        //    var lambda = (LambdaExpression)propName;
        //    MemberExpression memberExpression;
        //    string memberName = null;

        //    if (lambda.Body is UnaryExpression)
        //    {
        //        var unaryExpression = (UnaryExpression)lambda.Body;
        //        memberExpression = (MemberExpression)unaryExpression.Operand;
        //    }
        //    else
        //    {
        //        memberExpression = (MemberExpression)lambda.Body;
        //    }
        //    memberName = memberExpression.Member.Name;

        //    newValue = newValue.Trim();
        //    if (string.IsNullOrEmpty(newValue))
        //    {
        //        if (_validationErrors.ContainsKey(memberName))
        //        {
        //            List<string> existingMessages = null;
        //            if (_validationErrors.TryGetValue(memberName, out existingMessages))
        //            {
        //                if (existingMessages != null)
        //                {
        //                    if (!existingMessages.Exists(msg => msg.Equals(descriptionMissing)))
        //                    {
        //                        _validationErrors[memberName].Add(descriptionMissing);
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            _validationErrors.Add(memberName, new List<string> { descriptionMissing });
        //            RaiseErrorsChanged(memberName);
        //        }
        //    }
        //    else if (_validationErrors.ContainsKey(memberName))
        //    {
        //        _validationErrors.Remove(memberName);
        //        RaiseErrorsChanged(memberName);
        //    }
        //}

    }
}