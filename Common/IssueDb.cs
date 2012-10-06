using System.Collections.Generic;
using System.Linq;

namespace Testing.Common
{
    internal class IssueDb : IIssueDb
    {
        private readonly IUserInfoSettings _userInfoSettings;
        private readonly IIssuesSettings _issuesSettings;
        private readonly IList<IIssue> _issues;

        public IssueDb()
        {
            _userInfoSettings = new UserInfoSettings();
            _issuesSettings = new IssuesSettings();
            _issues = new List<IIssue>();
        }

        #region IIssueDb implementation
        public IUserInfoSettings UserInfoSettings { get { return _userInfoSettings; } }

        public IIssuesSettings IssuesSettings { get { return _issuesSettings; } }

        public IList<IIssue> Issues { get { return _issues; } }

        public IList<IIssue> GetIssuesBySet(IssueSets set)
        {
            return (from i in Issues
                    where i.Set == set
                    select i).ToList();
        }

        public int GetIssueNumber(DistributionChannels channel, Regions region)
        {
            return (from i in Issues
                    where (i.DistributionChannel & channel) == channel
                          && (i.Region & region) == region
                    select i).Count();
        }

        public IList<IIssue> GetIssues(DistributionChannels channel, Regions region)
        {
            return (from i in Issues
                    where (i.DistributionChannel & channel) == channel
                          && (i.Region & region) == region
                    orderby i.Set
                    select i).ToList();
        }

        public IList<IIssue> GetIssues(DistributionChannels channel, Regions region, IssueSets set)
        {
            return (from i in Issues
                    where i.Set == set
                          && (i.DistributionChannel & channel) == channel
                          && (i.Region & region) == region
                    select i).ToList();
        }
        #endregion
    }
}