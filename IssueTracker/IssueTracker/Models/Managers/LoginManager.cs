using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace IssueTracker.Models
{
    public class LoginManager
    {
        private XDocument _diskuseXML;
        private string _pathToXML;

        public LoginManager()
        {
            _pathToXML = HttpContext.Current.Server.MapPath("~/App_Data/LoginInfo.xml");
            _diskuseXML = XDocument.Load(_pathToXML);
        }

        public void AddLogin(string name, string cookie)
        {
            XElement newUser = new XElement("user", new XAttribute("name", name), new XAttribute("cookie", cookie));
            _diskuseXML.Root.Add(newUser);
            _diskuseXML.Save(_pathToXML);
        }

        public void DeleteLogin(string cookie)
        {
            IEnumerable<XElement> logins = _diskuseXML.Root.Descendants("user").Where(a => a.Attribute("cookie").Value == cookie);
            if (logins.Count() == 0)
            {
                return;
            }

            logins.First().Remove();
            _diskuseXML.Save(_pathToXML);
        }

        public bool IsLoged(string cookie)
        {
            IEnumerable<XElement> logins = _diskuseXML.Root.Descendants("user").Where(a => a.Attribute("cookie").Value == cookie);
            if (logins.Count() == 0)
            {
                return false;
            }
            else return true;
        }

        public string GiveMeLoged(string cookie)
        {
            IEnumerable<XElement> logins = _diskuseXML.Root.Descendants("user").Where(a => a.Attribute("cookie").Value == cookie);
            if (logins.Count() == 0)
            {
                return null;
            }
            return logins.First().Attribute("name").Value;
        }


    }
}
