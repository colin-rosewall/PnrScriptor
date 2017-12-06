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
        public GDSCommandTreeViewModel()
        {
            _addSubgroupCommand = new RelayCommand<object>(ShowMessage, p => this.CanExecute());

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
        public void ShowMessage(object obj)
        {
            MessageBox.Show("You did it");
        }

        public bool CanExecute()
        {
            return true;
        }
    }
}
