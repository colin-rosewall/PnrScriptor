using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSortableObservableCollection.Models
{
    public class Flight
    {
        private string _origin = null;
        private string _destination = null;
        private string _travelDate = null;

        public Flight()
        {
            _origin = string.Empty;
            _destination = string.Empty;
            _travelDate = string.Empty;
        }

        public string Origin
        {
            get
            {
                return _origin;
            }
            set
            {
                _origin = value;
            }
        }

        public string Destination
        {
            get
            {
                return _destination;
            }
            set
            {
                _destination = value;
            }
        }

        public string TravelDate
        {
            get
            {
                return _travelDate;
            }
            set
            {
                _travelDate = value;
            }
        }
    }
}
