using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Models
{
    public class Comment
    {
        public int IDissue { get; set; }
        public int Order { get; set; }
        public DateTime Added { get; set; }
        public string Diskuter { get; set; }
        public string Content { get; set; }

        public Comment() { }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;


            Comment s = obj as Comment;
            if (s == null)
                return false;

            return IDissue == s.IDissue && Added == s.Added;
        }

        public override int GetHashCode()
        {
            return IDissue.GetHashCode()^Added.GetHashCode();
        }
    }
}
