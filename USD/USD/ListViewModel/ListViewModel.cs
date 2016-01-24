using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using USD.Annotations;
using USD.DAL;

namespace USD.ListViewModel
{
    public class ListViewModel:INotifyPropertyChanged
    {
        private readonly IMammaRepository _mammaRepository;
        private List<ItemListViewModel> _screaningList;
        private string _searchPattern;
        private ObservableCollection<ItemListViewModel> _list;

        public ListViewModel(IMammaRepository mammaRepository)
        {
            _mammaRepository = mammaRepository;

            LoadData();
        }

        private void LoadData()
        {
            _screaningList = _mammaRepository.GetAll().Select(x => new ItemListViewModel(x)).ToList();

            FilterData();
        }

        private void FilterData()
        {
            List = !String.IsNullOrEmpty(SearchPattern) 
                ? new ObservableCollection<ItemListViewModel>(_screaningList.Where(x => IsGood(x, SearchPattern))) 
                : new ObservableCollection<ItemListViewModel>(_screaningList);

        }

        private bool IsGood(ItemListViewModel item, string serchPattern)
        {
            return (item.FIO?.ToLower().Contains(serchPattern.ToLower()) ?? true) ||
                   (item.BirthYear?.Contains(serchPattern) ?? true) ||
                   (item.Conclusion?.ToLower().Contains(serchPattern.ToLower()) ?? true);

        }


        public string SearchPattern
        {
            get { return _searchPattern; }
            set
            {
                if (value == _searchPattern) return;
                _searchPattern = value;
                FilterData();
                OnPropertyChanged(nameof(SearchPattern));
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}