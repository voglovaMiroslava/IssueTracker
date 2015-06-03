using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Models
{
    interface IDiskuseManager
    {
        void AddComment();
        List<Comment> GetAllComments(int issueID);
    }
}
