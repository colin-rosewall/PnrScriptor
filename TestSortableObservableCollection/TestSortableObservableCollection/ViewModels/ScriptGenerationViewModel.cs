using Pidgin;
using static Pidgin.Parser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TestSortableObservableCollection.Models;
using TestSortableObservableCollection.ViewModels.Base;

namespace TestSortableObservableCollection.ViewModels
{
    public class ScriptGenerationViewModel : Base.BaseViewModel
    {
        private string _scriptInput = null;
        private string _scriptOutput = null;
        private ObservableCollection<Models.Flight> _flights = null;
        private ICommand _applyReplacementsCommand = null;

        public ScriptGenerationViewModel()
        {
            _scriptInput = string.Empty;
            _flights = new ObservableCollection<Models.Flight>();

            _applyReplacementsCommand = new RelayCommand<object>(ApplyReplacements_Executed, ApplyReplacements_CanExecute);

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

        public string ScriptOutput
        {
            get { return _scriptOutput; }
            set
            {
                _scriptOutput = value;
                NotifyPropertyChanged(() => ScriptOutput);
            }
        }

        public ICommand ApplyReplacementsCommand
        {
            get { return _applyReplacementsCommand; }
            set
            {
                _applyReplacementsCommand = value;
            }
        }

        public void ApplyReplacements_Executed(object obj)
        {
            StringBuilder sb = new StringBuilder();
            int availabilityCount = 0;

            string[] lines = _scriptInput.Split(new [] { Environment.NewLine, "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                string aLine = line;
                bool replacementsMade = false;

                replacementsMade = ReplaceAmaAvail(ref aLine, ref availabilityCount);
                if (!replacementsMade)
                {
                    sb.AppendLine(line);
                }
                else
                {
                    sb.AppendLine(aLine);
                }
            }
            ScriptOutput = sb.ToString();
        }

        private bool ReplaceAmaAvail(ref string line, ref int availabilityCount)
        {
            bool result = false;

            var transactionCode = from gap1 in Parser.SkipWhitespaces
                                  from firstTransactionCode in Parser.CIChar('a')
                                  from secondTransactionCode in OneOf(Try(Parser.CIChar('n')), Try(Parser.CIChar('d')), Try(Parser.CIChar('a')))
                                  select new string(new char[] { firstTransactionCode, secondTransactionCode }).ToUpper();

            var OptionalDayDigit = from secondDigit in Parser.Digit.Optional()
                                   select (secondDigit.HasValue ? new string(new char[] { secondDigit.Value }) : string.Empty);

            var dayPart = from gap1 in Parser.SkipWhitespaces
                          from firstDigit in Parser.Digit
                          from secondOptionalDigit in OptionalDayDigit
                          select new string(new char[] { firstDigit }) + secondOptionalDigit;

            var monthPart = from gap1 in Parser.SkipWhitespaces
                            from monthShortName in Parser.OneOf(
                                Try(Parser.CIString("JAN")), Try(Parser.CIString("FEB")), Try(Parser.CIString("MAR")), Try(Parser.CIString("APR")),
                                Try(Parser.CIString("MAY")), Try(Parser.CIString("JUN")), Try(Parser.CIString("JUL")), Try(Parser.CIString("AUG")),
                                Try(Parser.CIString("SEP")), Try(Parser.CIString("OCT")), Try(Parser.CIString("NOV")), Try(Parser.CIString("DEC")))
                            select monthShortName.ToUpper();

            var cityPart = from gap1 in Parser.SkipWhitespaces
                           from city in Parser.Letter.Repeat(3)
                           select new string(city.ToArray()).ToUpper();

            var test1 = from a in transactionCode
                        from b in dayPart
                        from c in monthPart
                        from origin in cityPart
                        from destination in cityPart
                        select new { a, b, c, origin, destination };

            var presult = test1.Parse(line);
            if (presult.Success)
            {
                var flt = _flights.ElementAtOrDefault(availabilityCount);
                if (flt != null)
                {
                    var tc = presult.Value.a;
                    var dt = string.IsNullOrEmpty(flt.TravelDate) ? presult.Value.b + presult.Value.c : flt.TravelDate;  
                    var or = string.IsNullOrEmpty(flt.Origin) ? presult.Value.origin : flt.Origin;
                    var ds = string.IsNullOrEmpty(flt.Destination) ? presult.Value.destination : flt.Destination;
                    line = string.Concat(tc," ", dt, " ", or, " ", ds);
                }
                availabilityCount++;
                result = true;
            }
                

            return result;
        }

        public bool ApplyReplacements_CanExecute(object obj)
        {
            return !string.IsNullOrEmpty(_scriptInput);
        }
    }
}
