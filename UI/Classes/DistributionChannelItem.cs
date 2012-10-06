using System.Windows;

using Testing.Common;

using Wanderer.Library.Common;

namespace Testing.UI.Classes
{
    public class DistributionChannelItem : BindableObject
    {
        private readonly DistributionChannels _distributionChannel;

        private InterfaceLanguages _interfaceLanguage;

        internal DistributionChannelItem(DistributionChannels channel)
        {
            _distributionChannel = channel;

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

        public DistributionChannels Channel { get { return _distributionChannel; } }

        private void SetDescription()
        {
            switch (_distributionChannel)
            {
                case DistributionChannels.KA:
                    Description = Application.Current.FindResource("KATitle").ToString();
                    break;
                case DistributionChannels.OnTrade:
                    Description = Application.Current.FindResource("OnTradeTitle").ToString();
                    break;
                case DistributionChannels.OffTrade:
                    Description = Application.Current.FindResource("OffTradeTitle").ToString();
                    break;
            }
        }
    }
}
