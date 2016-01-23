using System.ComponentModel;
using System.Runtime.CompilerServices;
using USD.Annotations;
using USD.MammaModel;

namespace USD.MammaViewModel
{
    public class FocalFormationViewModel:INotifyPropertyChanged
    {
        public FocalFormationViewModel()
        {
            Outlines = OutlinesType.SmothClear;
            Echogenicity = Echogenicity.Hypo;
            Structure = Structure.Homogenous;
        }

        private OutlinesType _outlines;
        private string _localization;
        private decimal? _size;
        private Echogenicity _echogenicity;
        private Structure _structure;

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

        public decimal? Size
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