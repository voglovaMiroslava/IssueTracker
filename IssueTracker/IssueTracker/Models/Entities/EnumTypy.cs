using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Models
{
    public enum IssueType
    {
        error, request
    }

    public enum State
    {
        isnew, inprogress, solved, denied
    }
}
