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
                new XAttribute("zakaznik", HttpUtility.HtmlEncode(project.Client)),
                new XAttribute("pid", HttpUtility.HtmlEncode(idProjektu)));

            novyProj.Add(new XElement("nazev-projektu", HttpUtility.HtmlEncode(project.Name)),
                new XElement("popis-projektu", HttpUtility.HtmlEncode(project.Description)),
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

            toUpdateProject.Descendants("nazev-projektu").First().SetValue(HttpUtility.HtmlEncode(project.Name));
            toUpdateProject.Descendants("popis-projektu").First().SetValue(HttpUtility.HtmlEncode(project.Description));

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
            return GetAll(client, "");
        }

        public List<Project> GetByName(string projectName)
        {
            return GetAll("", projectName);
        }

        /// <summary>
        /// Vraci null pokud nejsou zadne projekty.
        /// </summary>
        /// <returns></returns>
        public List<Project> GetAll()
        {
            return MakeMeList(_projektyXML.Root.Descendants("projekt"));
        }

        /// <summary>
        /// Vraci null kdyz nesedi! Prazdne a null retezce bere jakoze podle toho nechceme vybirat.
        /// </summary>
        /// <param name="clientName"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public List<Project> GetAll(string clientName, string projectName)
        {
            IEnumerable<XElement> allProjects = _projektyXML.Root.Descendants("projekt");
            if (!(clientName == "" || clientName == null))
            {
                allProjects = GiveMeByClients(clientName, allProjects);
            }
            if (!(projectName == "" || projectName == null))
            {
                allProjects = GiveMeByName(projectName, allProjects);
            }
            return MakeMeList(allProjects);

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

            List<Project> projectList = MakeMeList(projects);
            if (projectList == null)
            {
                return null;
            }
            return projectList.First();
        }

        private IEnumerable<XElement> GiveMeByClients(string client, IEnumerable<XElement> projects)
        {
            return projects.Where(a => HttpUtility.HtmlDecode(a.Attribute("zakaznik").Value) == client);
        }

        private IEnumerable<XElement> GiveMeByName(string name, IEnumerable<XElement> projects)
        {
            return projects.Where(a => HttpUtility.HtmlDecode(a.Descendants("nazev-projektu").First().Value) == name);
        }

        private List<Project> MakeMeList(IEnumerable<XElement> projekty)
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
                    HttpUtility.HtmlDecode(item.Descendants("nazev-projektu").First().Value),
                    HttpUtility.HtmlDecode(item.Descendants("popis-projektu").First().Value));

                
                projectList.Add(newPro);
            }

            return projectList;
        }


    }
}
