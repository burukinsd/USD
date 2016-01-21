using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
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
            IsCanalsExpanded = false;
            DiffuseChanges = DiffuseChanges.Moderate;
            VisualizatioNippleArea = VisualizatioNippleArea.Good;
            FocalFormations = new ObservableCollection<FocalFormation> {new FocalFormation() {Test = "Test"}};

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
        private bool _isCanalsExpanded;
        private string _canalsExpandingDesc;
        private DiffuseChanges _diffuseChanges;
        private string _diffuseChangesFeatures;
        private VisualizatioNippleArea _visualizatioNippleArea;
        private ObservableCollection<FocalFormation> _focalFormations;

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

        public bool IsCanalsExpanded
        {
            get { return _isCanalsExpanded; }
            set
            {
                if (value == _isCanalsExpanded) return;
                _isCanalsExpanded = value;
                OnPropertyChanged(nameof(IsCanalsExpanded));
            }
        }

        public string CanalsExpandingDesc
        {
            get { return _canalsExpandingDesc; }
            set
            {
                if (value == _canalsExpandingDesc) return;
                _canalsExpandingDesc = value;
                OnPropertyChanged(nameof(CanalsExpandingDesc));
            }
        }

        public DiffuseChanges DiffuseChanges
        {
            get { return _diffuseChanges; }
            set
            {
                if (value == _diffuseChanges) return;
                _diffuseChanges = value;
                OnPropertyChanged(nameof(DiffuseChanges));
            }
        }

        public string DiffuseChangesFeatures
        {
            get { return _diffuseChangesFeatures; }
            set
            {
                if (value == _diffuseChangesFeatures) return;
                _diffuseChangesFeatures = value;
                OnPropertyChanged(nameof(DiffuseChangesFeatures));
            }
        }

        public VisualizatioNippleArea VisualizatioNippleArea
        {
            get { return _visualizatioNippleArea; }
            set
            {
                if (value == _visualizatioNippleArea) return;
                _visualizatioNippleArea = value;
                OnPropertyChanged(nameof(VisualizatioNippleArea));
            }
        }

        public ObservableCollection<FocalFormation> FocalFormations
        {
            get { return _focalFormations; }
            set
            {
                if (Equals(value, _focalFormations)) return;
                _focalFormations = value;
                OnPropertyChanged(nameof(FocalFormations));
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

    public class FocalFormation
    {
        public string Test { get; set; }
    }
}