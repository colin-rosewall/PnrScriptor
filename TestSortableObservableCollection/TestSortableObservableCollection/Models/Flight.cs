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
        private string _airlineCode = null;
        private string _bookingClass = null;

        public Flight()
        {
            _origin = string.Empty;
            _destination = string.Empty;
            _travelDate = string.Empty;
            _airlineCode = string.Empty;
            _bookingClass = string.Empty;
        }

        public string Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        public string Destination
        {
            get { return _destination; }
            set { _destination = value; }
        }

        public string TravelDate
        {
            get { return _travelDate; }
            set { _travelDate = value; }
        }

        public string AirlineCode
        {
            get { return _airlineCode; }
            set { _airlineCode = value; }
        }

        public string BookingClass
        {
            get { return _bookingClass; }
            set { _bookingClass = value; }
        }
    }
}
