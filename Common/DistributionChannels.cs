using System;
using System.ComponentModel;

namespace Testing.Common
{
    [Flags]
    public enum DistributionChannels
    {
// ReSharper disable InconsistentNaming
        [Description("Стандартний роздріб (CР)")]
        KA = 0x01,
// ReSharper restore InconsistentNaming
        [Description("Horeca")]
        OnTrade = 0x02,
        [Description("Сучасна торгівля (МТ)")]
        OffTrade = 0x04
    }
}