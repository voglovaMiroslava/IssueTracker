using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace IssueTracker.Models
{
    //POUZIT HTTPUTILITY.HTMLENCODE A HTTPUTILITY.DECODE
    public class IssueManager : IIssueManager
    {
        private XDocument _projektyXML;
        private string _pathToXML;

        public IssueManager()
        {
            _pathToXML = HttpContext.Current.Server.MapPath("~/App_Data/Projekty.xml");
            _projektyXML = XDocument.Load(_pathToXML);
        }

        /// <summary>
        /// Jedine povolene null hodnoty jsou jmeno resitele a datum ukonceni.
        /// </summary>
        /// <param name="issue"></param>
        public bool Create(ref Issue issue)
        {
            int projectID = issue.IDproject;
            IEnumerable<XElement> projects = _projektyXML.Root.Descendants("projekt").
                Where(a => Convert.ToInt32(a.Attribute("pid").Value) == projectID);
            if (projects.Count() == 0)
            {
                return false;
            }
            XElement elemPozadavky = projects.Last().Descendants("pozadavky").First();

            IEnumerable<XElement> allIssues = _projektyXML.Root.Descendants("pozadavek");
            int issueID;
            if (allIssues.Count() == 0)
            {
                issueID = 1;
            }
            else
            {
                issueID = Convert.ToInt32(allIssues.Last().Attribute("pozadavekid").Value) + 1;
            }
            issue.ID = issueID;

            XElement newIssue = new XElement("pozadavek", new XAttribute("pozadavekid", issueID),
                new XAttribute("projectid", projectID),
                new XAttribute("typ", issue.IssueType.ToString()),
                new XAttribute("stav", issue.IssueState.ToString()),
                new XElement("zadavatel", HttpUtility.HtmlEncode(issue.AddedBy)),
                new XElement("nazev-pozadavku", HttpUtility.HtmlEncode(issue.Name)),
                new XElement("obsah-pozadavku", HttpUtility.HtmlEncode(issue.Content)),
                new XElement("name-resitele"),
                new XElement("datum-zadani", String.Format("{0:YYYY-MM-DD}", issue.AddedDate)),
                new XElement("datum-ukonceni", new XAttribute("xsi:nil", "true")));

            if (issue.FinishedDate.HasValue)
            {
                newIssue.SetElementValue("datum-ukonceni", String.Format("{0:YYYY-MM-DD}", issue.FinishedDate));
            }
            if (issue.AssignedTo != null)
            {
                newIssue.SetElementValue("name-resitele", HttpUtility.HtmlEncode(issue.AssignedTo));
            }

            elemPozadavky.Add(newIssue);
            _projektyXML.Save(_pathToXML);

            return true;
        }

        /// <summary>
        /// Meni jen nazev, obsah, resitele, stav a datum ukonceni. Vraci false pokud neni mozny update.
        /// </summary>
        /// <param name="issue"></param>
        /// <returns></returns>
        public bool Update(Issue issue)
        {
            IEnumerable<XElement> issues = _projektyXML.Root.Descendants("pozadavek").
                Where(a => Convert.ToInt32(a.Attribute("pozadavekid").Value) == issue.ID);

            if (issues.Count() == 0)
            {
                return false;
            }

            XElement updateEl = issues.First();
            updateEl.SetAttributeValue("stav", issue.IssueState.ToString());
            updateEl.SetElementValue("nazev-pozadavku", HttpUtility.HtmlEncode(issue.Name));
            updateEl.SetElementValue("obsah-pozadavku", HttpUtility.HtmlEncode(issue.Content));
            if (issue.AssignedTo != null)
            {
                updateEl.SetElementValue("name-resitele", HttpUtility.HtmlEncode(issue.AssignedTo));
            }
            if (issue.FinishedDate.HasValue)
            {
                updateEl.SetElementValue("datum-ukonceni", String.Format("{0:YYYY-MM-DD}", issue.FinishedDate));
            }

            _projektyXML.Save(_pathToXML);
            return true;
        }

        /// <summary>
        /// Vrati vechny issues, nebo null pokud zadne nejsou.
        /// </summary>
        /// <returns></returns>
        public List<Issue> GetAll()
        {
            return MakeMeList(_projektyXML.Root.Descendants("pozadavek"));
        }

        /// <summary>
        /// Vraci vsechny issue odpovidajici zadanemu project, type a state (cokoli z toho muze byt null)
        /// pak se podle toho issues netridi. Vrati NULL pokud neodpovida hledane kombinaci zadne issue.
        /// </summary>
        /// <param name="project">pokud není, nehledá se podle toho</param>
        /// <param name="idIssue">Nullable, pokud není, nehledá se podle toho.</param>
        /// <param name="type">pokud není, nehledá se podle toho</param>
        /// <param name="state">pokud není, nehledá se podle toho</param>
        /// <returns></returns>
        public List<Issue> GetAll(Project project, Type? type, State? state)
        {
            if (project == null && (!type.HasValue) && (!state.HasValue))
            {
                return GetAll();
            }

            IEnumerable<XElement> allIssues = _projektyXML.Root.Descendants("pozadavek");

            if (project != null)
            {
                allIssues = GetAllFromProject(project, allIssues);
            }
            if (type.HasValue)
            {
                allIssues = GetAllByType((Type)type, allIssues);
            }
            if (state.HasValue)
            {
                allIssues = GetAllByState((State)state, allIssues);
            }

            return MakeMeList(allIssues);
        }

        /// <summary>
        /// Vraci vsechna issues u daneho projektu. Nema-li zadne, vraci null.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public List<Issue> GetAllFromProject(Project project)
        {
            return MakeMeList(GetAllFromProject(project, _projektyXML.Root.Descendants("pozadavek")));
        }

        /// <summary>
        /// Vrati issue se zadanym typem. Pokud Issues daneho typu neexistují vraci NULL
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<Issue> GetAllByType(Type type)
        {
            return MakeMeList(GetAllByType(type, _projektyXML.Root.Descendants("pozadavek")));
        }

        /// <summary>
        /// Vrati issue se zadanym stavem. Pokud Issues daneho stavu neexistují vraci NULL
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public List<Issue> GetAllByState(State state)
        {
            return MakeMeList(GetAllByState(state, _projektyXML.Root.Descendants("pozadavek")));
        }

        /// <summary>
        /// Vrati issue se zadanym ID. Pokud Issue neexistuje vraci NULL.
        /// </summary>
        /// <param name="issueID"></param>
        /// <returns></returns>
        public Issue GetById(int issueID)
        {
            IEnumerable<XElement> issues = _projektyXML.Root.Descendants("pozadavek").Where(a => Convert.ToInt32(a.Attribute("pozadavekid")) == issueID);
            List<Issue> myIssues = MakeMeList(issues);
            if (myIssues == null)
            {
                return null;
            }
            else
            {
                return myIssues.First();
            }
        }

        private IEnumerable<XElement> GetAllFromProject(Project project, IEnumerable<XElement> issues)
        {
            List<XElement> findedIsues = new List<XElement>();
            foreach (var item in issues)
            {
                if (Convert.ToInt32(item.Attribute("projectid").Value) == project.ID)
                {
                    findedIsues.Add(item);
                }
            }
            return findedIsues;
        }

        private IEnumerable<XElement> GetAllByType(Type type, IEnumerable<XElement> issues)
        {
            List<XElement> findedIsues = new List<XElement>();
            foreach (var item in issues)
            {
                if (item.Attribute("typ").Value == type.ToString())
                {
                    findedIsues.Add(item);
                }
            }
            return findedIsues;
        }

        private IEnumerable<XElement> GetAllByState(State state, IEnumerable<XElement> issues)
        {
            List<XElement> findedIsues = new List<XElement>();
            foreach (var item in issues)
            {
                if (item.Attribute("stav").Value == state.ToString())
                {
                    findedIsues.Add(item);
                }
            }
            return findedIsues;
        }

        /// <summary>
        /// Udela z XElementu Issues. Pokud nema XElement, vraci NULL.
        /// </summary>
        /// <param name="issues"></param>
        /// <returns>null pokud neni zadny XElement, jinak list Issues</returns>
        private List<Issue> MakeMeList(IEnumerable<XElement> issues)
        {
            if (issues.Count() == 0)
            {
                return null;
            }
            List<Issue> issueList = new List<Issue>();

            foreach (var item in issues)
            {
                Issue newIssue = new Issue();
                newIssue.AddedBy = HttpUtility.HtmlDecode(item.Descendants("zadavatel").First().Value);
                newIssue.AddedDate = Convert.ToDateTime(item.Descendants("datum-zadani").First().Value);
                newIssue.AssignedTo = HttpUtility.HtmlDecode(item.Descendants("name-resitele").First().Value);
                newIssue.Content = HttpUtility.HtmlDecode(item.Descendants("obsah-pozadavku").First().Value);
                newIssue.ID = Convert.ToInt32(item.Attribute("pozadavekid").Value);
                newIssue.IDproject = Convert.ToInt32(item.Attribute("projectid").Value);
                newIssue.IssueState = (State)Enum.Parse(typeof(State), item.Attribute("stav").Value);
                newIssue.IssueType = (Type)Enum.Parse(typeof(Type), item.Attribute("typ").Value);
                newIssue.Name = HttpUtility.HtmlDecode(item.Descendants("nazev-pozadavku").First().Value);
                string endDate = item.Descendants("datum-ukonceni").First().Value;
                if (endDate != "")
                {
                    newIssue.FinishedDate = Convert.ToDateTime(endDate);
                }
                issueList.Add(newIssue);
            }
            return issueList;
        }

    }
}
