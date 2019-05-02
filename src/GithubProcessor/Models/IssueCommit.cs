using System;

namespace GithubProcessor.Models
{
    [Serializable]
    public class IssueCommit
    {
        public int IssueNumber { get; set; }

        public string CommitSha { get; set; }
    }
}
