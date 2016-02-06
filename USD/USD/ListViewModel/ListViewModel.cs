using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using USD.Annotations;
using USD.DAL;
using USD.ViewTools;

namespace USD.ListViewModel
{
    public class ListViewModel:INotifyPropertyChanged
    {
        private readonly IMammaRepository _mammaRepository;
        private List<ItemListViewModel> _screaningList;
        private string _searchPattern;
        private ObservableCollection<ItemListViewModel> _list;
        private ItemListViewModel _selectedItem;

        public ListViewModel(IMammaRepository mammaRepository)
        {
            _mammaRepository = mammaRepository;

            DeleteCommand = new RelayCommand(x => Delete(), x => SelectedItem != null);
            ExportCommand = new RelayCommand(x => Export(), x => SelectedItem != null);

            LoadData();
        }

        public ICommand EditCommand { get; set; }

        private void Export()
        {
            SelectedItem.Export();
        }

        public ICommand ExportCommand { get; set; }

        private void Delete()
        {
            var dilogResult =
                MessageBox.Show(
                    $"Вы уверены, что хотите удалить исследование \"{SelectedItem.FIO} {SelectedItem.BirthYear} г.р.\" от {SelectedItem.VisitDate.ToShortDateString()}?",
                    "Список исследований", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

            if (dilogResult == MessageBoxResult.Yes)
            {
                _mammaRepository.Delete(SelectedItem.Id);

                _screaningList.Remove(SelectedItem);

                SelectedItem = null;

                FilterData();
            }
        }

        public ICommand DeleteCommand { get; set; }

        public void LoadData()
        {
            _screaningList = _mammaRepository.GetAll().Select(x => new ItemListViewModel(x)).ToList();

            FilterData();
        }

        private void FilterData()
        {
            if (SelectedItem != null && !IsGood(SelectedItem, SearchPattern))
            {
                SelectedItem = null;
            }

            List = !String.IsNullOrEmpty(SearchPattern) 
                ? new ObservableCollection<ItemListViewModel>(_screaningList.Where(x => IsGood(x, SearchPattern))) 
                : new ObservableCollection<ItemListViewModel>(_screaningList);

        }

        private bool IsGood(ItemListViewModel item, string serchPattern)
        {
            return (item.FIO?.ToLower().Contains(serchPattern.ToLower()) ?? false)
                    || (item.BirthYear?.Contains(serchPattern) ?? false) 
                    || (item.Conclusion?.ToLower().Contains(serchPattern.ToLower()) ?? false)
                    ;

        }


        public string SearchPattern
        {
            get { return _searchPattern; }
            set
            {
                if (value == _searchPattern) return;
                _searchPattern = value;
                OnPropertyChanged(nameof(SearchPattern));
                FilterData();
            }
        }

        public ObservableCollection<ItemListViewModel> List
        {
            get { return _list; }
            set
            {
                if (Equals(value, _list)) return;
                _list = value;
                OnPropertyChanged(nameof(List));
            }
        }

        public ItemListViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (Equals(value, _selectedItem)) return;
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}