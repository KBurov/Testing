using System.Windows;

using Testing.Common;

using Wanderer.Library.Common;

namespace Testing.UI.Classes
{
    public class RegionItem : BindableObject
    {
        private readonly Regions _region;

        private InterfaceLanguages _interfaceLanguage;

        internal RegionItem(Regions region)
        {
            _region = region;

            SetDescription();
        }

        public InterfaceLanguages InterfaceLanguage
        {
            get { return _interfaceLanguage; }
            set
            {
                _interfaceLanguage = value;

                SetDescription();

                RaisePropertyChanged("Description");
            }
        }

        public string Description { get; private set; }

        public Regions Region { get { return _region; } }

        private void SetDescription()
        {
            switch (_region)
            {
                case Regions.EastUkraine:
                    Description = Application.Current.FindResource("EastUkraineTitle").ToString();
                    break;
                case Regions.WestUkraine:
                    Description = Application.Current.FindResource("WestUkraineTitle").ToString();
                    break;
            }
        }
    }
}