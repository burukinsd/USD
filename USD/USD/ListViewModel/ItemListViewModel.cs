using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LiteDB;
using USD.Annotations;
using USD.MammaModels;
using USD.WordExport;

namespace USD.ListViewModel
{
    public class ItemListViewModel : INotifyPropertyChanged
    {
        private readonly MammaModel _model;
        private string _birthYear;
        private string _conclusion;
        private string _fio;
        private DateTime _visitDate;

        public ItemListViewModel(MammaModel mammaModel)
        {
            VisitDate = mammaModel.VisitDate;
            FIO = mammaModel.FIO;
            BirthYear = mammaModel.BirthYear;
            Conclusion = ConclusionMaker.MakeConclusion(mammaModel);
            Id = mammaModel.Id;
            _model = mammaModel;
        }

        public string Conclusion
        {
            get { return _conclusion; }
            set
            {
                if (value == _conclusion) return;
                _conclusion = value;
                OnPropertyChanged(nameof(Conclusion));
            }
        }

        public string BirthYear
        {
            get { return _birthYear; }
            set
            {
                if (value == _birthYear) return;
                _birthYear = value;
                OnPropertyChanged(nameof(BirthYear));
            }
        }

        public string FIO
        {
            get { return _fio; }
            set
            {
                if (value == _fio) return;
                _fio = value;
                OnPropertyChanged(nameof(FIO));
            }
        }

        public DateTime VisitDate
        {
            get { return _visitDate; }
            set
            {
                if (value.Equals(_visitDate)) return;
                _visitDate = value;
                OnPropertyChanged(nameof(VisitDate));
            }
        }

        public ObjectId Id { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Export()
        {
            MammaExporter.Export(_model);
        }
    }
}