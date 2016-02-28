using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using USD.Annotations;
using USD.MammaModels;

namespace USD.MammaViewModels
{
    public class CystViewModel:INotifyPropertyChanged
    {
        public CystViewModel()
        {
            Outlines = OutlinesType.SmothClear;
            Echogenicity = Echogenicity.Hypo;
            Structure = Structure.Homogenous;
            CDK = CDK.Avascular;
        }

        

        public CystViewModel(CystModel model)
        {
            Localization = model.Localization;
            Size = model.Size;
            Outlines = model.Outlines;
            Echogenicity = model.Echogenicity;
            Structure = model.Structure;
            CDK = model.CDK;
        }

        private OutlinesType _outlines;
        private string _localization;
        private string _size;
        private Echogenicity _echogenicity;
        private Structure _structure;
        private CDK _cdk;

        public CDK CDK
        {
            get { return _cdk; }
            set
            {
                if (value == _cdk) return;
                _cdk = value;
                OnPropertyChanged(nameof(CDK));
            }
        }

        public string Localization
        {
            get { return _localization; }
            set
            {
                if (value == _localization) return;
                _localization = value;
                OnPropertyChanged(nameof(Localization));
            }
        }

        public string Size
        {
            get { return _size; }
            set
            {
                if (value == _size) return;
                _size = value;
                OnPropertyChanged(nameof(Size));
            }
        }

        public OutlinesType Outlines
        {
            get { return _outlines; }
            set
            {
                if (value == _outlines) return;
                _outlines = value;
                OnPropertyChanged(nameof(Outlines));
            }
        }

        public Echogenicity Echogenicity
        {
            get { return _echogenicity; }
            set
            {
                if (value == _echogenicity) return;
                _echogenicity = value;
                OnPropertyChanged(nameof(Echogenicity));
            }
        }

        public Structure Structure
        {
            get { return _structure; }
            set
            {
                if (value == _structure) return;
                _structure = value;
                OnPropertyChanged(nameof(Structure));
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
