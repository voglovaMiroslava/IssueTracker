using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Models
{
    interface IIssueManager
    {
        bool Create(Issue issue);
        bool Update(Issue issue);
        List<Issue> GetAll();
        List<Issue> GetAll(Project project, IssueType? type, State? state);
        List<Issue> GetAllFromProject(Project project);
        List<Issue> GetAllByType(IssueType type);
        List<Issue> GetAllByState(State state);
        Issue GetById(int issueID);
    }
}
