using Testing.Common;

namespace Testing.Admin.UI
{
    static class Issues
    {
        private static volatile IIssueDb _issueDb;
        private static readonly object _issueDbLock = new object();
        private static readonly object _issueDbSaveLock = new object();

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

        public static void SaveIssueDb()
        {
            lock (_issueDbSaveLock)
            {
                IssueDbAssistant.SaveIssueDb(IssueDb);
            }
        }
    }
}
