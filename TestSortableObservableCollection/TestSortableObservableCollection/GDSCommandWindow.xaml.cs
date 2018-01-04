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

namespace TestSortableObservableCollection
{
    /// <summary>
    /// Interaction logic for GDSCommandWindow.xaml
    /// </summary>
    public partial class GDSCommandWindow : Window
    {
        public GDSCommandWindow(GDSCommandTreeViewModel tvm)
        {
            InitializeComponent();
            DataContext = tvm;
            if (tvm.CloseGDSCommandWindow == null)
                tvm.CloseGDSCommandWindow = new Action(this.Hide);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
