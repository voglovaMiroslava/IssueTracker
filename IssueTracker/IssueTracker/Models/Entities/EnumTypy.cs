using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Models
{
    enum Type
    {
        error, request
    }

    enum State
    { 
        isnew, inprogress, solved, denied
    }
}
