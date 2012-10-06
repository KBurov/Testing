using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Testing.Admin.UI")]

namespace Testing.Common
{
    internal class UserInfoSettings : IUserInfoSettings
    {
        public bool IsPositionVisible { get; set; }

        public bool IsLevelVisible { get; set; }

        public bool IsExperienceVisible { get; set; }
    }
}
