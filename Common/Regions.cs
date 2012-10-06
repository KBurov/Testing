using System;
using System.ComponentModel;

namespace Testing.Common
{
    [Flags]
    public enum Regions
    {
        [Description("Західна Україна")]
        WestUkraine = 0x01,
        [Description("Україна")]
        EastUkraine = 0x02
    }
}
