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
using TestSortableObservableCollection.Helpers;

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
            int availabilityCounter = 0;

            string[] lines = _scriptInput.Split(new [] { Environment.NewLine, "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string lineOfInput in lines)
            {
                string lineReplacement = lineOfInput;
                bool replacementsMade = false;

                replacementsMade = ReplacementsHelper.ReplaceAmaAvail(ref lineReplacement, ref availabilityCounter, _flights);
                if (!replacementsMade)
                {
                    replacementsMade = ReplacementsHelper.ReplaceSabreAvail(ref lineReplacement, ref availabilityCounter, _flights);
                    if (!replacementsMade)
                    {
                        replacementsMade = ReplacementsHelper.ReplaceGalAvail(ref lineReplacement, ref availabilityCounter, _flights);
                        if (!replacementsMade)
                        {
                            replacementsMade = ReplacementsHelper.ReplaceGalBuySeats(ref lineReplacement, availabilityCounter, _flights);
                            if (!replacementsMade)
                            {
                                replacementsMade = ReplacementsHelper.ReplaceSabreBuySeats(ref lineReplacement, availabilityCounter, _flights);
                                if (!replacementsMade)
                                {
                                    replacementsMade = ReplacementsHelper.ReplaceAmaBuySeats(ref lineReplacement, availabilityCounter, _flights);
                                    if (!replacementsMade)
                                    {
                                        sb.AppendLine(lineOfInput);
                                    }
                                    else
                                    {
                                        sb.AppendLine(lineReplacement);
                                    }
                                }
                                else
                                {
                                    sb.AppendLine(lineReplacement);
                                }
                            }
                            else
                            {
                                sb.AppendLine(lineReplacement);
                            }
                        }
                        else
                        {
                            sb.AppendLine(lineReplacement);
                        }
                    }
                    else
                    {
                        sb.AppendLine(lineReplacement);
                    }
                }
                else
                {
                    sb.AppendLine(lineReplacement);
                }
            }
            ScriptOutput = sb.ToString();
        }

        public bool ApplyReplacements_CanExecute(object obj)
        {
            return !string.IsNullOrEmpty(_scriptInput);
        }
    }
}
