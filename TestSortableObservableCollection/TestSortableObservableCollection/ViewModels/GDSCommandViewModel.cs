using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TestSortableObservableCollection.Interfaces;

namespace TestSortableObservableCollection.ViewModels
{
    public class GDSCommandViewModel : Base.BaseViewModel, IGDSCommandViewModel
    {
        private UInt64 _uniqueID;
        private string _description = null;
        private string _commandLines = null;
        private IGDSCommandItemViewModel _parent = null;
        private SortableObservableCollection<IGDSCommandItemViewModel> _children = null;
        private bool _IsItemExpanded = false;
        private bool _IsItemSelected = false;
        private Dictionary<string, List<string>> _validationErrors = null;
        private string _guid = string.Empty;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public GDSCommandViewModel()
        {
            _description = string.Empty;
            _commandLines = string.Empty;
            _children = new SortableObservableCollection<IGDSCommandItemViewModel>();
            _validationErrors = new Dictionary<string, List<string>>();
        }

        public GDSCommandViewModel(IGDSCommandItemViewModel parent, string theDescription, string theCommandLines) : this()
        {
            _parent = parent;
            _description = theDescription;
            _commandLines = theCommandLines;
        }

        public GDSCommandViewModel(IGDSCommandItemViewModel parent, string theDescription, string theCommandLines, string guid) : this(parent, theDescription, theCommandLines)
        {
            _guid = guid;
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

    }
}
