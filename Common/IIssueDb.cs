using System.Collections.Generic;

namespace Testing.Common
{
    public interface IIssueDb
    {
        IUserInfoSettings UserInfoSettings { get; }

        IIssuesSettings IssuesSettings { get; }

        IList<IIssue> Issues { get; }

        IList<IIssue> GetIssuesBySet(IssueSets set);

        int GetIssueNumber(DistributionChannels channel, Regions region);

        IList<IIssue> GetIssues(DistributionChannels channel, Regions region);

        IList<IIssue> GetIssues(DistributionChannels channel, Regions region, IssueSets set);
    }
}
