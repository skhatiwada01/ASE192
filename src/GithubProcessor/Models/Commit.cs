using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace GithubProcessor.Models
{
    [Serializable]
    public class Commit
    {
        public string Sha { get; set; }

        public DateTime CommitDateTime { get; set; }

        public int? UserId { get; set; }
    }

    public class CommitChangeFileInfo
    {
        public string Sha { get; set; }

        [XmlArray]
        public List<CommitFile> CommitFiles { get; set; }

        public string Url { get; set; }
    }

    [Serializable]
    public class CommitFile
    {
        public string Filename { get; set; }

        public string PreviousFileName { get; set; }

        public string Sha { get; set; }

        // File status, like modified, added, deleted.
        public string Status { get; set; }

        public string RawUrl { get; set; }
    }
}
