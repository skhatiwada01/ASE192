using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace GithubProcessor.Models
{
    [Serializable]
    public class Issue
    {
        public int Id { get; set; }

        public int IssueNumber { get; set; }

        public int? AssigneeId { get; set; }

        [XmlArray]
        public List<int> AssigneeIds { get; set; }

        public int? ClosedBy { get; set; }

        public int? CreatedBy { get; set; }

        public string EventsUrl { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public int CommentsCount { get; set; }

        public string CommentsUrl { get; set; }

        [XmlArray]
        public List<string> Labels { get; set; }
    }
}
