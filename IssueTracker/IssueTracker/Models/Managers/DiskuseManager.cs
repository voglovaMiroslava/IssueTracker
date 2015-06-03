using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace IssueTracker.Models
{
    class DiskuseManager : IDiskuseManager
    {
        private XDocument _diskuseXML;
        private string _pathToXML;

        public DiskuseManager()
        {
            _pathToXML = HttpContext.Current.Server.MapPath("~/App_Data/Diskuse.xml");
            _diskuseXML = XDocument.Load(_pathToXML);
        }

        public void AddComment()
        {
            throw new NotImplementedException();
        }

        public List<Comment> GetAllComments(int issueID)
        {
            throw new NotImplementedException();
        }
    }
}
