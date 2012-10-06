using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Testing.Admin.UI")]

namespace Testing.Common
{
    internal class Issue : IIssue
    {
        public DistributionChannels DistributionChannel { get; set; }

        public Regions Region { get; set; }

        public IssueTypes Type { get; set; }

        public IssueSets Set { get; set; }

        public string ContentUA { get; set; }

        public string ContentRU { get; set; }

        public uint CorrectAnswerPoints { get; set; }

        public IList<IAnswer> Answers { get; private set; }

        public uint NumberOfShelves { get; set; }

        public uint PlacesOnShelf { get; set; }

        public IList<string> PlacementCorrectAnswer1 { get; private set; }

        public IList<string> PlacementCorrectAnswer2 { get; private set; }

        public IList<string> PlacementCorrectAnswer3 { get; private set; }

        public IList<string> PlacementCorrectAnswer4 { get; private set; }

        public IList<string> PlacementCorrectAnswer5 { get; private set; }

        public bool IsAnswered { get; set; }

        public Issue()
        {
            Answers = new List<IAnswer>();
            PlacementCorrectAnswer1 = new List<string>();
            PlacementCorrectAnswer2 = new List<string>();
            PlacementCorrectAnswer3 = new List<string>();
            PlacementCorrectAnswer4 = new List<string>();
            PlacementCorrectAnswer5 = new List<string>();
        }
    }
}
