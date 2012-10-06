using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Testing.Admin.UI")]

namespace Testing.Common
{
    internal class IssuesSettings : IIssuesSettings
    {
        public bool IsTimeLimited { get; set; }

        public uint TimeLimit { get; set; }
    }
}
