using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSortableObservableCollection.ViewModels
{
    public class ScriptGenerationViewModel : Base.BaseViewModel
    {
        private string _scriptInput = string.Empty;

        public ScriptGenerationViewModel()
        {

        }

        public string ScriptInput
        {
            get { return _scriptInput; }
            set
            {
                if (_scriptInput != value)
                {
                    _scriptInput = value;
                    NotifyPropertyChanged(() => ScriptInput);
                }
            }
        }
    }
}
