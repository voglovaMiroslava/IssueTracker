using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace IssueTracker.Models
{
    public class ProjectManager : IProjectManager
    {
        private XDocument _projektyXML;
        private string _pathToXML;

        public ProjectManager()
        {
            _pathToXML = HttpContext.Current.Server.MapPath("~/App_Data/Projekty.xml");
            _projektyXML = XDocument.Load(_pathToXML);
        }

        public void Add(ref Project project)
        {
            int idProjektu;
            IEnumerable<XElement> projekty = _projektyXML.Root.Descendants("projekt");
            if (projekty.Count() == 0)
            {
                idProjektu = 1;
            }
            else
            {
                idProjektu = Convert.ToInt32(projekty.Last().Attribute("pid").Value) + 1;
            }
            project.ID = idProjektu;

            XElement novyProj = new XElement("projekt", 
                new XAttribute("zakaznik", HttpUtility.HtmlEncode(project.Zakaznik)), 
                new XAttribute("pid", HttpUtility.HtmlEncode(idProjektu)));

            novyProj.Add(new XElement("nazev-projektu", HttpUtility.HtmlEncode(project.Nazev)), 
                new XElement("popis-projektu", HttpUtility.HtmlEncode(project.Popis)), 
                new XElement("pozadavky"));
            _projektyXML.Root.Add(novyProj);
            _projektyXML.Save(_pathToXML);
        }

        /// <summary>
        /// K upraveni projektu je nutne znat jeho ID. Upraveni projektu pouze meni jmeno a popis projektu.
        /// Pokud se nepovede vraci false.
        /// </summary>
        /// <param name="project"></param>
        public bool Update(Project project)
        {
            IEnumerable<XElement> projekty = _projektyXML.Root.Descendants("projekt").
                Where(a => Convert.ToInt32(a.Attribute("pid").Value) == project.ID);

            if (projekty.Count() == 0)
            {
                return false;
            }

            XElement toUpdateProject = projekty.First();

            toUpdateProject.Descendants("nazev-projektu").First().SetValue(HttpUtility.HtmlEncode(project.Nazev));
            toUpdateProject.Descendants("popis-projektu").First().SetValue(HttpUtility.HtmlEncode(project.Popis));

            _projektyXML.Save(_pathToXML);
            return true;
        }

        /// <summary>
        /// Vraci null pokud nema klient zadny projekt.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public List<Project> GetByClient(Client client)
        {
            return GetByClient(client.Name);
        }

        public List<Project> GetByClient(string client)
        {
            IEnumerable<XElement> projekty = _projektyXML.Root.Descendants("projekt").
                Where(a => HttpUtility.HtmlDecode(a.Attribute("zakaznik").Value) == client);
            return GetList(projekty);
        }

        /// <summary>
        /// Vraci null pokud nejsou zadne projekty.
        /// </summary>
        /// <returns></returns>
        public List<Project> GetAll()
        {
            return GetList(_projektyXML.Root.Descendants("projekt"));
        }

        /// <summary>
        /// Vraci null pokud nenalezen zadny projekt daneho id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Project GetById(int id)
        {
            IEnumerable<XElement> projects = _projektyXML.Root.Descendants("projekt").
                Where(a => Convert.ToInt32(a.Attribute("pid").Value) == id);

            List<Project> projectList = GetList(projects);
            if (projectList == null)
            {
                return null;
            }
            return projectList.First();
        }

        private List<Project> GetList(IEnumerable<XElement> projekty)
        {
            if (projekty.Count() == 0)
            {
                return null;
            }
            
            List<Project> projectList = new List<Project>();

            foreach (var item in projekty)
            {
                Project newPro = new Project(Convert.ToInt32(item.Attribute("pid").Value),
                    HttpUtility.HtmlDecode(item.Attribute("zakaznik").Value),
                    HttpUtility.HtmlDecode(item.Descendants("nazev-projektu").First().Value));

                newPro.Popis =  HttpUtility.HtmlDecode(item.Descendants("popis-projektu").First().Value);
                projectList.Add(newPro);
            }

            return projectList;
        }


    }
}
