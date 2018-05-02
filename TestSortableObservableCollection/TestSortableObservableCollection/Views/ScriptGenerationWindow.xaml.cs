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

    public class ScriptField
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

        private int FindStartOfNextField(string line, string token)
        {
            int index = line.IndexOf(token) - 1;
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

        private void ProcessLine(int lineCount, SortedList<int, List<ScriptField>> processedLines, string line)
        {
            int fieldOffset = 0;
            string fld;
            string val;
            int maxFieldCount = 2;
            List<ScriptField> lineFields = new List<ScriptField>();
            bool timeToExit = false;
            const string token1 = "**;";
            const string token2 = "**";

            if (line.Contains(token1) || line.Contains(token2))
            {
                do
                {
                    if (line.Contains(token1))
                    {
                        fld = line.Substring(0, line.IndexOf(token1) + token1.Length);
                        line = line.Substring(line.IndexOf(token1) + token1.Length);
                    }
                    else
                    {
                        fld = line.Substring(0, line.IndexOf(token2) + token2.Length);
                        line = line.Substring(line.IndexOf(token2) + token2.Length);
                    }
                    if (line.Contains(token1) || line.Contains(token2))
                    {
                        int index;
                        if (line.Contains(token1))
                        {
                            index = FindStartOfNextField(line, token1);
                        }
                        else
                        {
                            index = FindStartOfNextField(line, token2);
                        }
                        
                        if (index > 0)
                        {
                            index++;
                            val = line.Substring(0, index);
                            line = line.Substring(index);
                            ScriptField newItem = new ScriptField();
                            newItem.fieldOffset = fieldOffset;
                            newItem.field = fld;
                            newItem.fieldValue = val;
                            lineFields.Add(newItem);
                            fieldOffset++;
                        }
                    }
                    else
                    {
                        ScriptField newItem = new ScriptField();
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
                ScriptField newItem = new ScriptField();
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
                SortedList<int, List<ScriptField>> processedLines = new SortedList<int, List<ScriptField>>();
                int lineCount = 0;

                foreach (string line in lines)
                {
                    ProcessLine(lineCount, processedLines, line);
                    lineCount++;
                }
                ShowProcessedLines(processedLines);

            }
        }

        private void ShowProcessedLines(SortedList<int, List<ScriptField>> processedLines)
        {
            if (processedLines.Count > 0)
            {
                Grid grd = new Grid();
                grd.ColumnDefinitions.Add(new ColumnDefinition());
                grd.ColumnDefinitions.Add(new ColumnDefinition());

                int rowCount = 0;
                foreach (var line in processedLines.Values)
                {
                    RowDefinition r = new RowDefinition();
                    r.Height = new GridLength(30);
                    grd.RowDefinitions.Add(r);

                    int columnCount = 0;
                    foreach (var item in line)
                    {
                        Label lbl = new Label();
                        lbl.Content = item.field;
                        lbl.Height = 30;
                        lbl.FontSize = 12;

                        TextBox txt = new TextBox();
                        txt.Text = item.fieldValue;
                        txt.Height = 20;
                        txt.FontSize = 12;

                        StackPanel sp = new StackPanel();
                        sp.Orientation = Orientation.Horizontal;
                        sp.SetValue(Grid.RowProperty, rowCount);
                        sp.SetValue(Grid.ColumnProperty, columnCount);

                        sp.Children.Add(lbl);
                        sp.Children.Add(txt);
                        grd.Children.Add(sp);
                        columnCount++;
                    }

                    rowCount++;
                }
                mygrid.Children.Add(grd);
            }
        }
    }
}
