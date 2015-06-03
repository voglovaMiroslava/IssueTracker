using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace IssueTracker.Models
{
    class OsobaManager : IOsobaManager
    {
        private XDocument _osobyXML;
        private string _pathToXML;

        public OsobaManager()
        {
            _pathToXML = HttpContext.Current.Server.MapPath("~/App_Data/Osoby.xml");
            _osobyXML = XDocument.Load(_pathToXML);
        }

        public void changeSubscription(string personName, bool subscribe)
        {
            IEnumerable<XElement> persons = _osobyXML.Root.Descendants("osoba").Where(a => a.Attribute("name").Value == personName);
            if (persons.Count() == 0)
            {
                return;
            }

            XElement person = persons.First();
            if (subscribe)
            {
                person.SetAttributeValue("zasilat", "ano");
            }
            else
            {
                person.SetAttributeValue("zasilat", "ne");
            }

            _osobyXML.Save(_pathToXML);

        }

        public void Add(Person person)
        {
            throw new NotImplementedException();
        }

        public void Update(Person person)
        {
            throw new NotImplementedException();
        }

        private List<Person> GetThemAll(string statut)
        {
            IEnumerable<XElement> persons;
            if (statut == null)
            {
                persons = _osobyXML.Root.Descendants("osoba");
            }
            else 
            {
                persons = _osobyXML.Root.Descendants("osoba").Where(a => a.Attribute("statut").Value == statut);
            }

            List<Person> personsList = new List<Person>();

            foreach (var item in persons)
            {
                
            }

        }

        public List<Person> GetAllEmplo()
        {
            throw new NotImplementedException();
            IEnumerable<XElement> employees = _osobyXML.Root.Descendants("osoba").Where(a => a.Attribute("statut").Value == "zamestnanec");
            List<Person> lidi = new List<Person>();
            lidi.Add(new Employee());
        }

        public List<Person> GetAllCusto()
        {
            throw new NotImplementedException();
        }

        public List<Person> GetAll()
        {
            throw new NotImplementedException();
        }

        public Person GetByName()
        {
            throw new NotImplementedException();
        }


        public bool NameExists(string name)
        {

            IEnumerable<XElement> persons = _osobyXML.Root.Descendants("osoba").Where(a => a.Attribute("name").Value == name);
            if (persons.Count() == 0)
            {
                return false;
            }
            return true;

        }
    }
}
