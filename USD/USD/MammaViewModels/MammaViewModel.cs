using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using LiteDB;
using USD.Annotations;
using USD.DAL;
using USD.MammaModels;
using USD.ViewTools;
using USD.WordExport;

namespace USD.MammaViewModels
{
    public class MammaViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private readonly BackgroundWorker _autoSaveBackgroundWorker;
        private readonly IMammaRepository _mammaRepository;
        private bool _actualToPhase;
        private string _additionalDesc;
        private TissueQuanity _adipose;
        private bool _areCysts;
        private bool _areFocalFormations;
        private string _birthYear;
        private string _canalsExpandingDesc;
        private string _cystConslusionDesc;
        private ObservableCollection<CystViewModel> _cysts;
        private DiffuseChanges _diffuseChanges;
        private string _diffuseChangesFeatures;
        private string _fio;
        private DateTime _firstDayOfLastMenstrualCycle;
        private FocalFormationConclusionPosition _focalFormationConclusionPosition;
        private ObservableCollection<FocalFormationViewModel> _focalFormations;
        private TissueQuanity _grandular;
        private bool _isAdenosisConclusion;
        private bool _isCanalsExpanded;

        private bool _isChanged;
        private bool _isCystsConclusion;
        private bool _isDeterminateLymphNodes;
        private bool _isFocalFormationConclusion;
        private bool _isInvolutionConclusion;
        private bool _isNotPatalogyConclusion;
        private bool _isSkinChanged;
        private bool _isSpecificConclusion;
        private bool _isValid;
        private DateTime _lastSaved = DateTime.Now;
        private string _lymphNodesDesc;
        private MammaSpecialists _mammaSpecialistsRecomendation;
        private decimal? _maxThicknessGlandularLayer;
        private string _menopauseText;

        private MammaModel _model;
        private PhisiologicalStatus _phisiologicalStatus;
        private CystViewModel _selectedCyst;
        private FocalFormationViewModel _selectedFocalFormation;
        private string _skinChangedDesc;
        private string _specificConclusionDesc;

        private DateTime _visitDate;
        private VisualizatioNippleArea _visualizatioNippleArea;

        public MammaViewModel(IMammaRepository mammaRepository)
        {
            _mammaRepository = mammaRepository;

            _autoSaveBackgroundWorker = new BackgroundWorker();
            _autoSaveBackgroundWorker.DoWork += AutoSaveBackgroundWorkerOnDoWork;
            _autoSaveBackgroundWorker.RunWorkerCompleted += (sender, args) => _lastSaved = DateTime.Now;

            DefaultInitialize();

            InitializeCommand();

            _isChanged = false;
        }

        public ObservableCollection<CystViewModel> Cysts
        {
            get { return _cysts; }
            set
            {
                if (Equals(value, _cysts)) return;
                _cysts = value;
                OnPropertyChanged(nameof(Cysts));
            }
        }

        public ICommand CopyCystComand { get; set; }

        public CystViewModel SelectedCyst
        {
            get { return _selectedCyst; }
            set
            {
                if (Equals(value, _selectedCyst)) return;
                _selectedCyst = value;
                OnPropertyChanged(nameof(SelectedCyst));
            }
        }

        public ICommand DeleteCystComand { get; set; }

        public ICommand AddCystCommnad { get; set; }

        public ICommand CopyFFComand { get; set; }

        public ICommand DeleteFFComand { get; set; }

        public ICommand AddFFCommnad { get; set; }

        public ICommand ExportCommand { get; set; }

        public ICommand GotoListCommand { get; set; }

        public bool IsFocalFormationConclusion
        {
            get { return _isFocalFormationConclusion; }
            set
            {
                if (value == _isFocalFormationConclusion) return;
                _isFocalFormationConclusion = value;
                OnPropertyChanged(nameof(IsFocalFormationConclusion));
                if (value)
                    IsNotPatalogyConclusion = false;
            }
        }

        public FocalFormationConclusionPosition FocalFormationConclusionPosition
        {
            get { return _focalFormationConclusionPosition; }
            set
            {
                if (value == _focalFormationConclusionPosition) return;
                _focalFormationConclusionPosition = value;
                OnPropertyChanged(nameof(FocalFormationConclusionPosition));
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
                ActualToPhase = _phisiologicalStatus == PhisiologicalStatus.Normal;
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

        public TissueQuanity Grandular
        {
            get { return _grandular; }
            set
            {
                if (value == _grandular) return;
                _grandular = value;
                OnPropertyChanged(nameof(Grandular));
            }
        }

        public TissueQuanity Adipose
        {
            get { return _adipose; }
            set
            {
                if (value == _adipose) return;
                _adipose = value;
                OnPropertyChanged(nameof(Adipose));
            }
        }

        public decimal? MaxThicknessGlandularLayer
        {
            get { return _maxThicknessGlandularLayer; }
            set
            {
                if (value == _maxThicknessGlandularLayer) return;
                _maxThicknessGlandularLayer = value;
                OnPropertyChanged(nameof(MaxThicknessGlandularLayer));
            }
        }

        public bool ActualToPhase
        {
            get { return _actualToPhase; }
            set
            {
                if (value == _actualToPhase) return;
                _actualToPhase = value;
                OnPropertyChanged(nameof(ActualToPhase));
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
                if (!Cysts.Any())
                {
                    Cysts.Add(new CystViewModel());
                }
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
                if (!FocalFormations.Any())
                {
                    FocalFormations.Add(new FocalFormationViewModel());
                }
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

        public FocalFormationViewModel SelectedFocalFormation
        {
            get { return _selectedFocalFormation; }
            set
            {
                if (Equals(value, _selectedFocalFormation)) return;
                _selectedFocalFormation = value;
                OnPropertyChanged(nameof(SelectedFocalFormation));
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
                if (value)
                {
                    IsCystsConclusion = false;
                    IsAdenosisConclusion = false;
                    IsInvolutionConclusion = false;
                    IsSpecificConclusion = false;
                    IsFocalFormationConclusion = false;
                }
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
                if (value)
                    IsNotPatalogyConclusion = false;
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
                if (value)
                    IsNotPatalogyConclusion = false;
            }
        }

        public bool IsAdenosisConclusion
        {
            get { return _isAdenosisConclusion; }
            set
            {
                if (value == _isAdenosisConclusion) return;
                _isAdenosisConclusion = value;
                OnPropertyChanged(nameof(IsAdenosisConclusion));
                if (value)
                    IsNotPatalogyConclusion = false;
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
                if (value)
                    IsNotPatalogyConclusion = false;
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

        public string this[string columnName]
        {
            get
            {
                var error = string.Empty;
                switch (columnName)
                {
                    case nameof(BirthYear):
                        if (string.IsNullOrEmpty(BirthYear) || Convert.ToInt32(BirthYear) < 1900 ||
                            Convert.ToInt32(BirthYear) > DateTime.Now.Year)
                            error = "Нужно указать реальный год рождения.";
                        break;
                }
                _isValid = string.IsNullOrEmpty(error);
                return error;
            }
        }

        public string Error { get; }

        public event PropertyChangedEventHandler PropertyChanged;

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
            MaxThicknessGlandularLayer = _model.MaxThicknessGlandularLayer;
            ActualToPhase = _model.ActualToPhase;
            IsCanalsExpanded = _model.IsCanalsExpanded;
            CanalsExpandingDesc = _model.CanalsExpandingDesc;
            DiffuseChanges = _model.DiffuseChanges;
            DiffuseChangesFeatures = _model.DiffuseChangesFeatures;
            VisualizatioNippleArea = _model.VisualizatioNippleArea;
            AreCysts = _model.AreCysts;
            AreFocalFormations = _model.AreFocalFormations;
            FocalFormations =
                new ObservableCollection<FocalFormationViewModel>(
                    _model.FocalFormations.Select(x => new FocalFormationViewModel(x)));
            FocalFormations.CollectionChanged += FocalFormationsOnCollectionChanged;
            Cysts = new ObservableCollection<CystViewModel>(_model.Cysts.Select(x => new CystViewModel(x)));
            Cysts.CollectionChanged += CystsOnCollectionChanged;
            IsDeterminateLymphNodes = _model.IsDeterminateLymphNodes;
            AdditionalDesc = _model.AdditionalDesc;
            IsNotPatalogyConclusion = _model.IsNotPatalogyConclusion;
            IsCystsConclusion = _model.IsCystsConclusion;
            CystConslusionDesc = _model.CystConslusionDesc;
            IsInvolutionConclusion = _model.IsInvolutionConclusion;
            IsSpecificConclusion = _model.IsSpecificConclusion;
            IsFocalFormationConclusion = _model.IsFocalFormationConclusion;
            IsAdenosisConclusion = _model.IsAdenosisConclusion;
            SpecificConclusionDesc = _model.SpecificConclusionDesc;
            FocalFormationConclusionPosition = _model.FocalFormationConclusionPosition;
            MammaSpecialistsRecomendation = _model.Recomendation;
            Grandular = _model.Grandular;
            Adipose = _model.Adipose;

            _isChanged = false;
        }

        private void InitializeCommand()
        {
            SaveCommand = new RelayCommand(x => ManualSave(), x => _isChanged && _isValid);
            GotoListCommand = new RelayCommand(x => ShowList());
            ExportCommand = new RelayCommand(x => Export(), x => _isValid);

            AddFFCommnad = new RelayCommand(x => AddFocalFormation(), x => AreFocalFormations);
            DeleteFFComand = new RelayCommand(x => DeleteFocalFormation(),
                x => AreFocalFormations && SelectedFocalFormation != null);
            CopyFFComand = new RelayCommand(x => CopyFocalFormation(),
                x => AreFocalFormations && SelectedFocalFormation != null);

            AddCystCommnad = new RelayCommand(x => AddCyst(), x => AreCysts);
            DeleteCystComand = new RelayCommand(x => DeleteCyst(), x => AreCysts && SelectedCyst != null);
            CopyCystComand = new RelayCommand(x => CopyCyst(), x => AreCysts && SelectedCyst != null);
        }

        private void CopyCyst()
        {
            var newCyst = new CystViewModel
            {
                Localization = SelectedCyst.Localization,
                Structure = SelectedCyst.Structure,
                Outlines = SelectedCyst.Outlines,
                Echogenicity = SelectedCyst.Echogenicity,
                Size = SelectedCyst.Size,
                CDK = SelectedCyst.CDK
            };
            Cysts.Add(newCyst);
        }

        private void DeleteCyst()
        {
            var index = Cysts.IndexOf(SelectedCyst);
            Cysts.Remove(SelectedCyst);
            SelectedCyst = Cysts[index < Cysts.Count ? index : 0];
        }

        private void AddCyst()
        {
            Cysts.Add(new CystViewModel());
        }

        private void CopyFocalFormation()
        {
            var newFocalFormation = new FocalFormationViewModel
            {
                Localization = SelectedFocalFormation.Localization,
                Structure = SelectedFocalFormation.Structure,
                Outlines = SelectedFocalFormation.Outlines,
                Echogenicity = SelectedFocalFormation.Echogenicity,
                Size = SelectedFocalFormation.Size
            };
            FocalFormations.Add(newFocalFormation);
        }

        private void DeleteFocalFormation()
        {
            var index = FocalFormations.IndexOf(SelectedFocalFormation);
            FocalFormations.Remove(SelectedFocalFormation);
            SelectedFocalFormation = FocalFormations[index < FocalFormations.Count ? index : 0];
        }

        private void AddFocalFormation()
        {
            FocalFormations.Add(new FocalFormationViewModel());
        }

        private void Export()
        {
            if (_isChanged)
            {
                ManualSave();
            }
            MammaExporter.Export(_model);
        }

        private void ShowList()
        {
            var listViewModel = new ListViewModel.ListViewModel(_mammaRepository);
            var listView = new ListView(listViewModel);
            listView.Show();
        }

        private void DefaultInitialize()
        {
            _model = new MammaModel();
            VisitDate = DateTime.Today;
            PhisiologicalStatus = PhisiologicalStatus.Normal;
            FirstDayOfLastMenstrualCycle = DateTime.Today;
            IsSkinChanged = false;
            Grandular = TissueQuanity.Enogh;
            Adipose = TissueQuanity.Many;
            ActualToPhase = true;
            IsCanalsExpanded = false;
            DiffuseChanges = DiffuseChanges.Moderate;
            VisualizatioNippleArea = VisualizatioNippleArea.ObliqueProjection;
            AreCysts = false;
            AreFocalFormations = false;
            IsDeterminateLymphNodes = false;
            IsNotPatalogyConclusion = true;
            IsInvolutionConclusion = false;
            IsCystsConclusion = false;
            IsSpecificConclusion = false;
            IsFocalFormationConclusion = false;
            FocalFormationConclusionPosition = FocalFormationConclusionPosition.Left;
            FocalFormations = new ObservableCollection<FocalFormationViewModel>();
            FocalFormations.CollectionChanged += FocalFormationsOnCollectionChanged;
            Cysts = new ObservableCollection<CystViewModel>();
            Cysts.CollectionChanged += CystsOnCollectionChanged;
        }

        private void FocalFormationsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    (item as INotifyPropertyChanged).PropertyChanged += FocalFormationChanged;
                }
            }
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    (item as INotifyPropertyChanged).PropertyChanged -= FocalFormationChanged;
                }
            }
        }

        private void FocalFormationChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(FocalFormations));
        }

        private void CystsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    (item as INotifyPropertyChanged).PropertyChanged += CystChanged;
                }
            }
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    (item as INotifyPropertyChanged).PropertyChanged -= CystChanged;
                }
            }
        }

        private void CystChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Cysts));
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
            _model.Grandular = Grandular;
            _model.Adipose = Adipose;
            _model.MaxThicknessGlandularLayer = MaxThicknessGlandularLayer;
            _model.ActualToPhase = ActualToPhase;
            _model.IsCanalsExpanded = IsCanalsExpanded;
            _model.CanalsExpandingDesc = CanalsExpandingDesc;
            _model.DiffuseChanges = DiffuseChanges;
            _model.DiffuseChangesFeatures = DiffuseChangesFeatures;
            _model.VisualizatioNippleArea = VisualizatioNippleArea;
            _model.AreCysts = AreCysts;

            if (_model.Cysts == null)
            {
                _model.Cysts = new List<CystModel>();
            }

            _model.Cysts.Clear();
            if (Cysts != null && Cysts.Any())
            {
                _model.Cysts.AddRange(Cysts.Select(x => new CystModel
                {
                    Localization = x.Localization,
                    Outlines = x.Outlines,
                    Echogenicity = x.Echogenicity,
                    Structure = x.Structure,
                    Size = x.Size,
                    CDK = x.CDK,
                    Form = x.Form
                }));
            }


            _model.AreFocalFormations = AreFocalFormations;

            if (_model.FocalFormations == null)
            {
                _model.FocalFormations = new List<FocalFormationModel>();
            }

            _model.FocalFormations.Clear();
            if (FocalFormations != null && FocalFormations.Any())
            {
                _model.FocalFormations.AddRange(FocalFormations.Select(x => new FocalFormationModel
                {
                    Localization = x.Localization,
                    Outlines = x.Outlines,
                    Echogenicity = x.Echogenicity,
                    Structure = x.Structure,
                    Size = x.Size,
                    CDK = x.CDK,
                    Form = x.Form
                }));
            }

            _model.IsDeterminateLymphNodes = IsDeterminateLymphNodes;
            _model.AdditionalDesc = AdditionalDesc;
            _model.IsNotPatalogyConclusion = IsNotPatalogyConclusion;
            _model.IsCystsConclusion = IsCystsConclusion;
            _model.CystConslusionDesc = CystConslusionDesc;
            _model.IsInvolutionConclusion = IsInvolutionConclusion;
            _model.IsSpecificConclusion = IsSpecificConclusion;
            _model.IsFocalFormationConclusion = IsFocalFormationConclusion;
            _model.FocalFormationConclusionPosition = FocalFormationConclusionPosition;
            _model.IsAdenosisConclusion = IsAdenosisConclusion;
            _model.SpecificConclusionDesc = SpecificConclusionDesc;
            _model.Recomendation = MammaSpecialistsRecomendation;
        }

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