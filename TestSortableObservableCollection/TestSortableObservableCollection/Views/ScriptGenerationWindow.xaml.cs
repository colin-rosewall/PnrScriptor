using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TestSortableObservableCollection.ViewModels;
using System.Text.RegularExpressions;

namespace TestSortableObservableCollection.Views
{
    /// <summary>
    /// Interaction logic for ScriptGenerationWindow.xaml
    /// </summary>
    /// 

    public class Fields
    {
        public int fieldOffset = 0;
        public string field = null;
        public string fieldValue = null;
    }

    public partial class ScriptGenerationWindow : Window
    {
        
        public ScriptGenerationWindow(ScriptGenerationViewModel vm)
        {
            InitializeComponent();

            DataContext = vm;
        }

        private void attempt1()
        {
            //string maskRegEx = @"([a-zA-Z\d\/ -]+\*\*)([\da-zA-Z ]*[.\d-]*)";
            //Regex test1 = new Regex(maskRegEx);
            //if (test1.IsMatch(vm.ScriptInput))
            //{
            //    MatchCollection mc = test1.Matches(vm.ScriptInput);
            //    foreach (Match m in mc)
            //    {

            //        GroupCollection groups = m.Groups;
            //        if (groups.Count > 2)
            //        {
            //            MessageBox.Show(string.Format("{0}:{1}", groups[1].Value, groups[2].Value));
            //        }
            //    }
            //}

        }

        private int FindStartOfNextField(string line)
        {
            int index = line.IndexOf("**") - 1;
            bool okToContinue = true;
            while (index > 0 && okToContinue)
            {
                char preceeding = line.ElementAt(index);
                if (char.IsDigit(preceeding) | char.IsLetter(preceeding) | char.IsLower(preceeding) | preceeding == ' ' | preceeding == '/')
                {
                    index--;
                }
                else
                {
                    okToContinue = false;
                }
            }

            return index;
        }

        private void ProcessLine(int lineCount, SortedList<int, List<Fields>> processedLines, string line)
        {
            int fieldOffset = 0;
            string fld;
            string val;
            int maxFieldCount = 2;
            List<Fields> lineFields = new List<Fields>();
            bool timeToExit = false;

            if (line.Contains("**"))
            {
                do
                {
                    fld = line.Substring(0, line.IndexOf("**") + "**".Length);
                    line = line.Substring(line.IndexOf("**") + "**".Length);
                    if (line.Contains("**"))
                    {
                        int index = FindStartOfNextField(line);
                        if (index > 0)
                        {
                            index++;
                            val = line.Substring(0, index);
                            line = line.Substring(index);
                            Fields newItem = new Fields();
                            newItem.fieldOffset = fieldOffset;
                            newItem.field = fld;
                            newItem.fieldValue = val;
                            lineFields.Add(newItem);
                            fieldOffset++;
                        }
                    }
                    else
                    {
                        Fields newItem = new Fields();
                        newItem.fieldOffset = fieldOffset;
                        newItem.field = fld;
                        newItem.fieldValue = line;
                        lineFields.Add(newItem);
                        timeToExit = true;
                    }
                    if (fieldOffset >= maxFieldCount)
                        timeToExit = true;
                } while (!timeToExit);

            }
            else
            {
                Fields newItem = new Fields();
                newItem.fieldOffset = fieldOffset;
                newItem.field = line;
                newItem.fieldValue = null;
                lineFields.Add(newItem);
            }
            if (lineFields.Any())
            {
                processedLines.Add(lineCount, lineFields);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ScriptGenerationViewModel;

            if (vm != null)
            {
                char[] sep = Environment.NewLine.ToCharArray();

                string[] lines = vm.ScriptInput.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                SortedList<int, List<Fields>> processedLines = new SortedList<int, List<Fields>>();
                int lineCount = 0;

                foreach (string line in lines)
                {
                    ProcessLine(lineCount, processedLines, line);
                    lineCount++;
                }
                ShowProcessedLines(processedLines);

            }
        }

        private void ShowProcessedLines(SortedList<int, List<Fields>> processedLines)
        {
            foreach (var line in processedLines)
            {
                string msg = string.Empty;
                foreach (var f in line.Value)
                {
                    msg += string.Format("field = {0} value = {1}", f.field, f.fieldValue);
                }
                MessageBox.Show(msg);
            }
        }
    }
}
