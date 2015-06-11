using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Models
{
    interface IProjectManager
    {
        void Add(ref Project project);
        bool Update(Project project);
        List<Project> GetByClient(Client client);
        List<Project> GetAll();
        Project GetById(int id);

    }
}
