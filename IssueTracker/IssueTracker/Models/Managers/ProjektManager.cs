using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace IssueTracker.Models
{
    class ProjectManager : IProjectManager
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

            XElement novyProj = new XElement("projekt", new XAttribute("zakaznik", project.Zakaznik), new XAttribute("pid", idProjektu));
            novyProj.Add(new XElement("nazev-projektu", project.Nazev), new XElement("popis-projektu", project.Popis), new XElement("pozadavky"));
            _projektyXML.Save(_pathToXML);
        }

        /// <summary>
        /// K upraveni projektu je nutne znat jeho ID. Upraveni projektu pouze meni jmeno a popis projektu.
        /// </summary>
        /// <param name="project"></param>
        public void Update(Project project)
        {
            IEnumerable<XElement> projekty = _projektyXML.Root.Descendants("projekt").Where(a => Convert.ToInt32(a.Attribute("pid").Value) == project.ID);
            if (projekty.Count() == 0)
            {
                return;
            }

            XElement toUpdateProject = projekty.First();

            toUpdateProject.Descendants("nazev-projektu").First().SetValue(project.Nazev);
            toUpdateProject.Descendants("popis-projektu").First().SetValue(project.Nazev);
        }

        private List<Project> GetList(IEnumerable<XElement> projekty)
        {
            List<Project> projectList = new List<Project>();

            foreach (var item in projekty)
            {
                Project newPro = new Project(Convert.ToInt32(item.Attribute("pid").Value),
                    item.Attribute("zakaznik").Value,
                    item.Descendants("nazev-projektu").First().Value);
                newPro.Popis = item.Descendants("popis-projektu").First().Value;
                projectList.Add(newPro);
            }

            return projectList;
        }

        public List<Project> GetByClient(Client client)
        {
            IEnumerable<XElement> projekty = _projektyXML.Root.Descendants("projekt").Where(a => a.Attribute("zakaznik").Value == client.Name);
            return GetList(projekty);
        }

        public List<Project> GetAll()
        {
            return GetList(_projektyXML.Root.Descendants("projekt"));
        }

        public List<Project> GetById(int id)
        {
            IEnumerable<XElement> projekty = _projektyXML.Root.Descendants("projekt").Where(a => Convert.ToInt32(a.Attribute("pid").Value) == id);
            return GetList(projekty);
        }

    }
}
