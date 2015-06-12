using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Models
{
    public class Issue
    {
        public int ID { get; set; }
        public string AddedBy { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string AssignedTo { get; set; }
        public int IDproject { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? FinishedDate { get; set; }
        public Type IssueType { get; set; }
        public State IssueState { get; set; }

        public Issue() { }

        public override string ToString()
        {
            return String.Format("ID: {2} Name: {0}, Assigned by: {1}", Name, AddedBy, ID);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;


            Issue s = obj as Issue;
            if (s == null)
                return false;

            return ID == s.ID;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}
