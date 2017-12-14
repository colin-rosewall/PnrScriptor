using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using TestSortableObservableCollection.Interfaces;
using System.Windows.Input;
using TestSortableObservableCollection.ViewModels.Base;
using System.Windows;

namespace TestSortableObservableCollection.ViewModels
{
    class GDSCommandTreeViewModel : Base.BaseViewModel
    {
        private ObservableCollection<IGDSCommandItemViewModel> _root = null;
        private ICommand _addSubgroupCommand = null;
        private ICommand _selectedItemChangedCommand = null;
        private IGDSCommandItemViewModel _currentlySelectedItem { get; set; }

        public GDSCommandTreeViewModel()
        {
            _addSubgroupCommand = new RelayCommand<object>(AddSubgroup_Executed);
            _selectedItemChangedCommand = new RelayCommand<object>(SelectedItemChanged);

            _root = new ObservableCollection<IGDSCommandItemViewModel>();
            IGDSCommandSubgroupViewModel rootItem = new GDSCommandSubgroupViewModel(null, "Root");

            _root.Add(rootItem);

            IGDSCommandSubgroupViewModel sabreItem = new GDSCommandSubgroupViewModel(rootItem, "Sabre");
            rootItem.AddChildItem(sabreItem);

            IGDSCommandSubgroupViewModel galileoItem = new GDSCommandSubgroupViewModel(rootItem, "Galileo");
            rootItem.AddChildItem(galileoItem);

            IGDSCommandViewModel addAdult = new GDSCommandViewModel(galileoItem, "Add Gal Adult");
            galileoItem.Children.Add(addAdult);

            rootItem.Children.Sort(k => k.Description);
        }

        public ObservableCollection<IGDSCommandItemViewModel> Root
        {
            get
            {
                return _root;
            }
        }

        public ICommand AddSubgroupCommand
        {
            get
            {
                return _addSubgroupCommand;
            }
            set
            {
                _addSubgroupCommand = value;
            }
        }

        public ICommand SelectedItemChangedCommand
        {
            get
            {
                return _selectedItemChangedCommand;
            }
            set
            {
                _selectedItemChangedCommand = value;
            }
        }
        public void AddSubgroup_Executed(object obj)
        {
            IGDSCommandItemViewModel item = obj as IGDSCommandItemViewModel;

            if (item != null)
            {
                SubgroupItemWindow w = new SubgroupItemWindow();
                w.txtField1.Text = item.Description;
                w.ShowDialog();
            }
                
            else
                MessageBox.Show("It is null");
        }

        public void SelectedItemChanged(object obj)
        {
            if (obj is IGDSCommandItemViewModel == false)
                return;

            var item = obj as IGDSCommandItemViewModel;
            _currentlySelectedItem = item;
        }
    }
}
