using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace IssueTracker.Models
{
    public class PersonManager : IPersonManager
    {
        private XDocument _osobyXML;
        private string _pathToXML;

        public PersonManager()
        {
            _pathToXML = HttpContext.Current.Server.MapPath("~/App_Data/Osoby.xml");
            _osobyXML = XDocument.Load(_pathToXML);
        }

         /// <summary>
         /// Meni subscription. Pokud vrati false, nepodarilo se zmenit.
         /// </summary>
         /// <param name="personName"></param>
         /// <param name="subscribe"></param>
         /// <returns></returns>
        public bool ChangeSubscription(string personName, bool subscribe)
        {
            IEnumerable<XElement> persons = _osobyXML.Root.Descendants("osoba").
                Where(a => HttpUtility.HtmlDecode(a.Attribute("name").Value) == personName);

            if (persons.Count() == 0)
            {
                return false;
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
            return true;
        }

        /// <summary>
        /// Subscription defaultne nastavovana na "ne". Vracene False znaci, ze se nepovedlo pridat osobu.
        /// </summary>
        /// <param name="person"></param>
        public bool Add(Person person)
        {
            if(NameExists(person.Name))
            {
                return false;
            }

            XElement newPerson = new XElement("osoba", new XAttribute("name", HttpUtility.HtmlEncode(person.Name)), 
                new XAttribute("zasilat", "ne"),
                new XElement("email", HttpUtility.HtmlEncode(person.Email)), 
                new XElement("password", HttpUtility.HtmlEncode(person.Password)));
           
            if (person is Employee)
            {
                newPerson.Add(new XAttribute("statut", "zamestnanec"));
            }
            else if (person is Client)
            {
                newPerson.Add(new XAttribute("statut", "zakaznik"));
            }

            _osobyXML.Root.Add(newPerson);
            _osobyXML.Save(_pathToXML);
            return true;
        }

        /// <summary>
        /// Update meni jen email a password. Vrati false pokud se nepovedlo zmenit (osoba neexistuje).
        /// </summary>
        /// <param name="person"></param>
        public bool Update(Person person)
        {
            if (!NameExists(person.Name))
            {
                return false;
            }
            XElement myPerson = _osobyXML.Root.Descendants("osoba").Where(a => a.Attribute("name").Value == person.Name).First();

            myPerson.SetElementValue("email", HttpUtility.HtmlEncode(person.Email));
            myPerson.SetElementValue("password", HttpUtility.HtmlEncode(person.Password));
            _osobyXML.Save(_pathToXML);
            return true;
        }

        private Person MakeMePerson(XElement person)
        {
            Person myPerson;

            if (person.Attribute("statut").Value == "zakaznik")
            {
                myPerson = new Client();
            }
            else
            {
                myPerson = new Employee();
            }

            myPerson.Email = HttpUtility.HtmlDecode(person.Descendants("email").First().Value);
            myPerson.Name = HttpUtility.HtmlDecode(person.Attribute("name").Value);
            myPerson.Password = HttpUtility.HtmlDecode(person.Descendants("password").First().Value);

            return myPerson;
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
                persons = _osobyXML.Root.Descendants("osoba").
                    Where(a => a.Attribute("statut").Value == statut);
            }

            List<Person> personsList = new List<Person>();

            foreach (var item in persons)
            {
                Person person = MakeMePerson(item);
                personsList.Add(person);
            }

            return personsList;
        }

        public List<Person> GetAllEmplo()
        {
            return GetThemAll("zamestnanec");
        }

        public List<Person> GetAllCusto()
        {
            return GetThemAll("zakaznik");
        }

        public List<Person> GetAll()
        {
            return GetThemAll(null);
        }

        /// <summary>
        /// Vraci null pokud nenajde jmeno osoby.
        /// </summary>
        /// <returns></returns>
        public Person GetByName(string name)
        {
            if (!NameExists(name))
            {
                return null;
            }

            XElement person = _osobyXML.Root.Descendants("osoba").
                Where(a => HttpUtility.HtmlDecode(a.Attribute("name").Value) == name).First();
            return MakeMePerson(person);
        }
        
        public bool NameExists(string name)
        {
            IEnumerable<XElement> persons = _osobyXML.Root.Descendants("osoba").
                Where(a => HttpUtility.HtmlDecode(a.Attribute("name").Value) == name);

            if (persons.Count() == 0)
            {
                return false;
            }
            return true;
        }

        public List<Person> GetAllWithSubscribtion()
        {
            IEnumerable<XElement> persons = _osobyXML.Root.Descendants("osoba");
            List<Person> personList = new List<Person>();
            foreach (var item in persons)
            {
                if (item.Attribute("zasilat").Value == "ano")
                {
                    personList.Add(MakeMePerson(item));
                }
            }
            return personList;
        }
    }
}
