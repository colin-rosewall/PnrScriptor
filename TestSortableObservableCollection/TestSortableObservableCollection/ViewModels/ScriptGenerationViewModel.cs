using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSortableObservableCollection.Models;

namespace TestSortableObservableCollection.ViewModels
{
    public class ScriptGenerationViewModel : Base.BaseViewModel
    {
        private string _scriptInput = null;
        private ObservableCollection<Models.Flight> _flights = null;

        public ScriptGenerationViewModel()
        {
            _scriptInput = string.Empty;
            _flights = new ObservableCollection<Flight>();

            // add dummy data for testing
            _flights.Add(new Models.Flight() { Origin = "ABC", Destination = "XYZ", TravelDate = "31JAN" });
        }

        public ObservableCollection<Models.Flight> Flights
        {
            get
            {
                return _flights;
            }
            set
            {
                _flights = value;
                NotifyPropertyChanged(() => Flights);
            }
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
