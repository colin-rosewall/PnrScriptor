﻿using System;
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
using ICSharpCode.AvalonEdit;

namespace TestSortableObservableCollection.Views
{
    /// <summary>
    /// Interaction logic for ScriptGenerationWindow.xaml
    /// </summary>
    /// 

    public partial class ScriptGenerationWindow : Window
    {
        
        public ScriptGenerationWindow(ScriptGenerationViewModel vm)
        {
            InitializeComponent();

            DataContext = vm;
            textEditor.Options.AllowToggleOverstrikeMode = true;
            textEditor.TextArea.OverstrikeMode = true;

            if (vm.ScriptUpdatedDelegate == null)
                vm.ScriptUpdatedDelegate = ScriptUpdatedNotification;
        }
        
        public void ScriptUpdatedNotification(string updatedScript)
        {
            if (updatedScript != null)
                textEditor.Document.Text = updatedScript;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var vm = DataContext as ScriptGenerationViewModel;

            if (vm != null)
                vm.ScriptUpdatedDelegate = null;
        }

    }
}
