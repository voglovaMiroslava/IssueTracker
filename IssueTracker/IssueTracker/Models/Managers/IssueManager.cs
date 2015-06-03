using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace IssueTracker.Models
{
    class IssueManager : IIssueManager
    {
        private XDocument _projektyXML;
        private string _pathToXML;

        public IssueManager()
        {
            _pathToXML = HttpContext.Current.Server.MapPath("~/App_Data/Projekty.xml");
            _projektyXML = XDocument.Load(_pathToXML);
        }

        public void Create(Issue issue)
        {
            throw new NotImplementedException();
        }

        public void Update(Issue issue)
        {
            throw new NotImplementedException();
        }

        public List<Issue> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<Issue> GetAll(Project project, int? idIssue, Type? type, State? state)
        {
            throw new NotImplementedException();
        }

        public List<Issue> GetAllFromProject(Project project)
        {
            throw new NotImplementedException();
        }

        public List<Issue> GetAllByType(Type type)
        {
            throw new NotImplementedException();
        }

        public List<Issue> GetAllByState(State state)
        {
            throw new NotImplementedException();
        }
    }
}
