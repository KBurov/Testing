using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Testing.Admin.UI")]

namespace Testing.Common
{
    internal class Answer : IAnswer
    {
        public bool IsCorrect { get; set; }

        public uint OrderNo { get; set; }

        public string ContentUA { get; set; }

        public string ContentRU { get; set; }

        public bool Selected { get; set; }

        public string ImageFileName { get; set; }
    }
}
