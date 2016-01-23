using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using LiteDB;
using USD.Annotations;
using USD.DAL;
using USD.MammaModels;
using USD.ViewTools;

namespace USD.MammaViewModels
{
    public class MammaViewModel:INotifyPropertyChanged
    {
        private readonly IMammaRepository _mammaRepository;

        private MammaModel _model;

        private bool _isChanged;
        private DateTime _lastSaved = DateTime.Now;
        private readonly BackgroundWorker _autoSaveBackgroundWorker;

        public MammaViewModel(IMammaRepository mammaRepository)
        {
            _mammaRepository = mammaRepository;

            _autoSaveBackgroundWorker = new BackgroundWorker();
            _autoSaveBackgroundWorker.DoWork += AutoSaveBackgroundWorkerOnDoWork;
            _autoSaveBackgroundWorker.RunWorkerCompleted += (sender, args) => _lastSaved = DateTime.Now;

            DefaultInitialize();
            
            InitializeCommand();
        }

        private void AutoSaveBackgroundWorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            BaseSave();
        }

        public void ApplyModel(ObjectId id)
        {
            _model = _mammaRepository.GetById(id);

            VisitDate = _model.VisitDate;
            FIO = _model.FIO;
            BirthYear = _model.BirthYear;
            PhisiologicalStatus = _model.PhisiologicalStatus;
            FirstDayOfLastMenstrualCycle = _model.FirstDayOfLastMenstrualCycle;
            MenopauseText = _model.MenopauseText;
            IsSkinChanged = _model.IsSkinChanged;
            SkinChangedDesc = _model.SkinChangedDesc;
            TissueRatio = _model.TissueRatio;
            LeftThicknessGlandularLayer = _model.LeftThicknessGlandularLayer;
            RightThicknessGlandularLayer = _model.RightThicknessGlandularLayer;
            IsCanalsExpanded = _model.IsCanalsExpanded;
            CanalsExpandingDesc = _model.CanalsExpandingDesc;
            DiffuseChanges = _model.DiffuseChanges;
            DiffuseChangesFeatures = _model.DiffuseChangesFeatures;
            VisualizatioNippleArea = _model.VisualizatioNippleArea;
            AreCysts = _model.AreCysts;
            CystsDesc = _model.CystsDesc;
            AreFocalFormations = _model.AreFocalFormations;
            FocalFormations = new ObservableCollection<FocalFormationViewModel>(_model.FocalFormations.Select(x => new FocalFormationViewModel(x)));
            IsDeterminateLymphNodes = _model.IsDeterminateLymphNodes;
            AdditionalDesc = _model.AdditionalDesc;
            IsNotPatalogyConclusion = _model.IsNotPatalogyConclusion;
            IsCystsConclusion = _model.IsCystsConclusion;
            CystConslusionDesc = _model.CystConslusionDesc;
            IsInvolutionConclusion = _model.IsInvolutionConclusion;
            IsSpecificConclusion = _model.IsSpecificConclusion;
            SpecificConclusionDesc = _model.SpecificConclusionDesc;
            MammaSpecialistsRecomendation = _model.Recomendation;
        }

        private void InitializeCommand()
        {
            SaveCommand = new RelayCommand(x => ManualSave(), x => _isChanged);
            GotoListCommand = new RelayCommand(x => ShowList());
        }

        private void ShowList()
        {
            var listViewModel = new ListViewModel(_mammaRepository);
            var listView = new ListView(listViewModel);
            listView.Show();
        }

        public ICommand GotoListCommand { get; set; }

        private void DefaultInitialize()
        {
            _model = new MammaModel();
            VisitDate = DateTime.Today;
            PhisiologicalStatus = PhisiologicalStatus.Normal;
            FirstDayOfLastMenstrualCycle = DateTime.Today;
            IsSkinChanged = false;
            TissueRatio = TissueRatio.EnoughGlandularMoreAdipose;
            IsCanalsExpanded = false;
            DiffuseChanges = DiffuseChanges.Moderate;
            VisualizatioNippleArea = VisualizatioNippleArea.Good;
            AreCysts = false;
            AreFocalFormations = false;
            IsDeterminateLymphNodes = false;
            IsNotPatalogyConclusion = true;
            IsInvolutionConclusion = false;
            IsCystsConclusion = false;
            IsSpecificConclusion = false;
            FocalFormations = new ObservableCollection<FocalFormationViewModel>();
        }

        private void ManualSave()
        {
            BaseSave();

            _lastSaved = DateTime.Now;
            _isChanged = false;
        }

        private void BaseSave()
        {
            ApplyChangesToModel();

            if (_model.Id != null)
            {
                _mammaRepository.Update(_model);
            }
            else
            {
                _model.Id = _mammaRepository.Add(_model);
            }
        }

        private void ApplyChangesToModel()
        {
            _model.VisitDate = VisitDate;
            _model.FIO = FIO;
            _model.BirthYear = BirthYear;
            _model.PhisiologicalStatus = PhisiologicalStatus;
            _model.FirstDayOfLastMenstrualCycle = FirstDayOfLastMenstrualCycle;
            _model.MenopauseText = MenopauseText;
            _model.IsSkinChanged = IsSkinChanged;
            _model.SkinChangedDesc = SkinChangedDesc;
            _model.TissueRatio = TissueRatio;
            _model.LeftThicknessGlandularLayer = LeftThicknessGlandularLayer;
            _model.RightThicknessGlandularLayer = RightThicknessGlandularLayer;
            _model.IsCanalsExpanded = IsCanalsExpanded;
            _model.CanalsExpandingDesc = CanalsExpandingDesc;
            _model.DiffuseChanges = DiffuseChanges;
            _model.DiffuseChangesFeatures = DiffuseChangesFeatures;
            _model.VisualizatioNippleArea = VisualizatioNippleArea;
            _model.AreCysts = AreCysts;
            _model.CystsDesc = CystsDesc;
            _model.AreFocalFormations = AreFocalFormations;

            if (_model.FocalFormations == null)
            {
                _model.FocalFormations = new List<FocalFormationModel>();
            }

            _model.FocalFormations.Clear();
            if (FocalFormations != null && FocalFormations.Any())
            {
                _model.FocalFormations.AddRange(FocalFormations.Select(x => new FocalFormationModel()
                {
                    Localization = x.Localization,
                    Outlines = x.Outlines,
                    Echogenicity = x.Echogenicity,
                    Structure = x.Structure,
                    Size = x.Size
                }));
            }

            _model.IsDeterminateLymphNodes = IsDeterminateLymphNodes;
            _model.AdditionalDesc = AdditionalDesc;
            _model.IsNotPatalogyConclusion = IsNotPatalogyConclusion;
            _model.IsCystsConclusion = IsCystsConclusion;
            _model.CystConslusionDesc = CystConslusionDesc;
            _model.IsInvolutionConclusion = IsInvolutionConclusion;
            _model.IsSpecificConclusion = IsSpecificConclusion;
            _model.SpecificConclusionDesc = SpecificConclusionDesc;
            _model.Recomendation = MammaSpecialistsRecomendation;
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
        private ObservableCollection<FocalFormationViewModel> _focalFormations;
        private bool _areCysts;
        private string _cystsDesc;
        private bool _areFocalFormations;
        private bool _isDeterminateLymphNodes;
        private string _lymphNodesDesc;
        private string _additionalDesc;
        private bool _isNotPatalogyConclusion;
        private bool _isCystsConclusion;
        private string _cystConslusionDesc;
        private bool _isInvolutionConclusion;
        private bool _isSpecificConclusion;
        private string _specificConclusionDesc;
        private MammaSpecialists _mammaSpecialistsRecomendation;
        


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

        public bool AreCysts
        {
            get { return _areCysts; }
            set
            {
                if (value == _areCysts) return;
                _areCysts = value;
                OnPropertyChanged(nameof(AreCysts));
            }
        }

        public string CystsDesc
        {
            get { return _cystsDesc; }
            set
            {
                if (value == _cystsDesc) return;
                _cystsDesc = value;
                OnPropertyChanged(nameof(CystsDesc));
            }
        }

        public bool AreFocalFormations
        {
            get { return _areFocalFormations; }
            set
            {
                if (value == _areFocalFormations) return;
                _areFocalFormations = value;
                OnPropertyChanged(nameof(AreFocalFormations));
            }
        }

        public ObservableCollection<FocalFormationViewModel> FocalFormations
        {
            get { return _focalFormations; }
            set
            {
                if (Equals(value, _focalFormations)) return;
                _focalFormations = value;
                OnPropertyChanged(nameof(FocalFormations));
            }
        }

        public bool IsDeterminateLymphNodes
        {
            get { return _isDeterminateLymphNodes; }
            set
            {
                if (value == _isDeterminateLymphNodes) return;
                _isDeterminateLymphNodes = value;
                OnPropertyChanged(nameof(IsDeterminateLymphNodes));
            }
        }

        public string LymphNodesDesc
        {
            get { return _lymphNodesDesc; }
            set
            {
                if (value == _lymphNodesDesc) return;
                _lymphNodesDesc = value;
                OnPropertyChanged(nameof(LymphNodesDesc));
            }
        }

        public string AdditionalDesc
        {
            get { return _additionalDesc; }
            set
            {
                if (value == _additionalDesc) return;
                _additionalDesc = value;
                OnPropertyChanged(nameof(AdditionalDesc));
            }
        }

        public bool IsNotPatalogyConclusion
        {
            get { return _isNotPatalogyConclusion; }
            set
            {
                if (value == _isNotPatalogyConclusion) return;
                _isNotPatalogyConclusion = value;
                OnPropertyChanged(nameof(IsNotPatalogyConclusion));
            }
        }

        public bool IsCystsConclusion
        {
            get { return _isCystsConclusion; }
            set
            {
                if (value == _isCystsConclusion) return;
                _isCystsConclusion = value;
                OnPropertyChanged(nameof(IsCystsConclusion));
            }
        }

        public string CystConslusionDesc
        {
            get { return _cystConslusionDesc; }
            set
            {
                if (value == _cystConslusionDesc) return;
                _cystConslusionDesc = value;
                OnPropertyChanged(nameof(CystConslusionDesc));
            }
        }

        public bool IsInvolutionConclusion
        {
            get { return _isInvolutionConclusion; }
            set
            {
                if (value == _isInvolutionConclusion) return;
                _isInvolutionConclusion = value;
                OnPropertyChanged(nameof(IsInvolutionConclusion));
            }
        }

        public bool IsSpecificConclusion
        {
            get { return _isSpecificConclusion; }
            set
            {
                if (value == _isSpecificConclusion) return;
                _isSpecificConclusion = value;
                OnPropertyChanged(nameof(IsSpecificConclusion));
            }
        }

        public string SpecificConclusionDesc
        {
            get { return _specificConclusionDesc; }
            set
            {
                if (value == _specificConclusionDesc) return;
                _specificConclusionDesc = value;
                OnPropertyChanged(nameof(SpecificConclusionDesc));
            }
        }

        public MammaSpecialists MammaSpecialistsRecomendation
        {
            get { return _mammaSpecialistsRecomendation; }
            set
            {
                if (value == _mammaSpecialistsRecomendation) return;
                _mammaSpecialistsRecomendation = value;
                OnPropertyChanged(nameof(MammaSpecialistsRecomendation));
            }
        }

        public ICommand SaveCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            _isChanged = true;

            if ((DateTime.Now - _lastSaved).Seconds >= 10 && !_autoSaveBackgroundWorker.IsBusy)
            {
                _autoSaveBackgroundWorker.RunWorkerAsync();
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}