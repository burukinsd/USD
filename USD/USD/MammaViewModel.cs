using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using USD.Annotations;

namespace USD
{
    public class MammaViewModel:INotifyPropertyChanged
    {
        public MammaViewModel()
        {
            VisitDate = DateTime.Today;
            PhisiologicalStatus = PhisiologicalStatus.Normal;
            FirstDayOfLastMenstrualCycle = DateTime.Today;
            IsSkinChanged = false;
            TissueRatio = TissueRatio.EnoughGlandularMoreAdipose;

            SaveCommand = new RelayCommand(x => Save());
        }

        private void Save()
        {
            var a = 1;
        }

        public MammaViewModel(MammaModel model)
        {
            throw new NotImplementedException();
        }

        private DateTime _visitDate;
        private string _fio;
        private string _birthYear;
        private PhisiologicalStatus _phisiologicalStatus;
        private DateTime _firstDayOfLastMenstrualCycle;
        private string _menopauseText;
        private bool _isSkinChanged;
        private string _skinChangedDesc;
        private TissueRatio _tissueRatio;
        private decimal? _leftThicknessGlandularLayer;
        private decimal? _rightThicknessGlandularLayer;

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

        public PhisiologicalStatus PhisiologicalStatus
        {
            get { return _phisiologicalStatus; }
            set
            {
                if (value == _phisiologicalStatus) return;
                _phisiologicalStatus = value;
                OnPropertyChanged(nameof(PhisiologicalStatus));
            }
        }

        public DateTime FirstDayOfLastMenstrualCycle
        {
            get { return _firstDayOfLastMenstrualCycle; }
            set
            {
                if (value.Equals(_firstDayOfLastMenstrualCycle)) return;
                _firstDayOfLastMenstrualCycle = value;
                OnPropertyChanged(nameof(FirstDayOfLastMenstrualCycle));
            }
        }

        public string MenopauseText
        {
            get { return _menopauseText; }
            set
            {
                if (value == _menopauseText) return;
                _menopauseText = value;
                OnPropertyChanged(nameof(MenopauseText));
            }
        }

        public bool IsSkinChanged
        {
            get { return _isSkinChanged; }
            set
            {
                if (value == _isSkinChanged) return;
                _isSkinChanged = value;
                OnPropertyChanged(nameof(IsSkinChanged));
            }
        }

        public string SkinChangedDesc
        {
            get { return _skinChangedDesc; }
            set
            {
                if (value == _skinChangedDesc) return;
                _skinChangedDesc = value;
                OnPropertyChanged(nameof(SkinChangedDesc));
            }
        }

        public TissueRatio TissueRatio
        {
            get { return _tissueRatio; }
            set
            {
                if (value == _tissueRatio) return;
                _tissueRatio = value;
                OnPropertyChanged(nameof(TissueRatio));
            }
        }

        public decimal? LeftThicknessGlandularLayer
        {
            get { return _leftThicknessGlandularLayer; }
            set
            {
                if (value == _leftThicknessGlandularLayer) return;
                _leftThicknessGlandularLayer = value;
                OnPropertyChanged(nameof(LeftThicknessGlandularLayer));
            }
        }

        public decimal? RightThicknessGlandularLayer
        {
            get { return _rightThicknessGlandularLayer; }
            set
            {
                if (value == _rightThicknessGlandularLayer) return;
                _rightThicknessGlandularLayer = value;
                OnPropertyChanged(nameof(RightThicknessGlandularLayer));
            }
        }

        public ICommand SaveCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}