using Testing.Common;

namespace Testing.UI
{
    static class Issues
    {
        private static volatile IIssueDb _issueDb;
        private static readonly object _issueDbLock = new object();

        public static IIssueDb IssueDb
        {
            get
            {
                if (_issueDb == null)
                {
                    lock (_issueDbLock)
                    {
                        if (_issueDb == null)
                        {
                            _issueDb = IssueDbAssistant.LoadIssueDb();
                        }
                    }
                }

                return _issueDb;
            }
        }
    }
}
