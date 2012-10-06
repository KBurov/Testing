using System.Collections.Generic;

namespace Testing.Common
{
    public interface IIssue
    {
        DistributionChannels DistributionChannel { get; }

        Regions Region { get; }

        IssueTypes Type { get; }

        IssueSets Set { get; }
// ReSharper disable InconsistentNaming
        string ContentUA { get; }

        string ContentRU { get; }
// ReSharper restore InconsistentNaming
        uint CorrectAnswerPoints { get; }

        IList<IAnswer> Answers { get; }

        uint NumberOfShelves { get; }

        uint PlacesOnShelf { get; }

        IList<string> PlacementCorrectAnswer1 { get; }

        IList<string> PlacementCorrectAnswer2 { get; }

        IList<string> PlacementCorrectAnswer3 { get; }

        IList<string> PlacementCorrectAnswer4 { get; }

        IList<string> PlacementCorrectAnswer5 { get; }

        bool IsAnswered { get; set; }
    }
}
