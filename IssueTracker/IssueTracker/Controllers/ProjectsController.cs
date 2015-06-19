using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IssueTracker.Models;

namespace IssueTracker.Controllers
{
    public class ProjectsController : Controller
    {
        private ProjectManager _projectManager = new ProjectManager();
        private IssueManager _issueManager = new IssueManager();
        private PersonManager _personManager = new PersonManager();

        // GET: Projects
        public ActionResult Index()
        {
            Dictionary<Project, dynamic> projectInfors = MakeMeDictionary(_projectManager.GetAll());

            ViewBag.AllClient = MySelectLists.GetAllClientWithProject();
            ViewBag.AllNames = MySelectLists.GetAllProjectNames();

            return View(projectInfors);
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            string clientName = form["ClientFilter"];
            string projectName = form["NameFilter"];

            ViewBag.AllClient = MySelectLists.GetAllClientWithProject();
            ViewBag.AllNames = MySelectLists.GetAllProjectNames();

            List<Project> projects = _projectManager.GetAll(clientName, projectName);
            if (projects == null)
            {
                projects = new List<Project>();
            }

            Dictionary<Project, dynamic> projectInfors = MakeMeDictionary(projects);

            return View(projectInfors);
        }

        public ActionResult Edit(int id)
        {
            Project project = _projectManager.GetById(id);

            return View(project);
        }

        [HttpPost]
        public ActionResult Edit(FormCollection form, int id)
        {
            Project project = _projectManager.GetById(id);
            string projectName = form["Name"];
            string content = form["Description"];

            if (projectName == "")
            {
                ViewBag.NotValidName = "Can not be empty!";
                return View(project);
            }

            _projectManager.Update(new Project(id, project.Client, projectName, content));
            return RedirectToAction("Index");
        }

        public ActionResult CreateProject()
        {
            ViewBag.AllClients = MySelectLists.GetAllClients();

            return View();
        }

        [HttpPost]
        public ActionResult CreateProject(FormCollection form)
        {
            string projectName = form["Name"];
            string content = form["Description"];
            string clientName = form["Client"];

            if (projectName == "")
            {
                ViewBag.NotValidName = "Can not be empty!";
            }
            if (clientName == "")
            {
                ViewBag.NotValidClient = "You must select client!";
            }
            if (projectName == "" || clientName == "")
            {
                return CreateProject();
            }

            Project project = new Project(0,clientName,projectName,content);
            _projectManager.Add(ref project);
            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            ViewBag.Project = _projectManager.GetById(id);

            ViewBag.AllState = MySelectLists.GetStates();
            ViewBag.AllType = MySelectLists.GetTypes();
            List<Issue> issues = _issueManager.GetAllFromProject(ViewBag.Project);
            if(issues == null)
            {
                issues = new List<Issue>();
            }

            return View(issues);
        }

        [HttpPost]
        public ActionResult Details(FormCollection form)
        {
            ViewBag.AllState = MySelectLists.GetStates();
            ViewBag.AllType = MySelectLists.GetTypes();
            string state = form["StateFilter"];
            string type = form["TypeFilter"];
            int projectID = Convert.ToInt32(form["Project"]);
            ViewBag.Project = _projectManager.GetById(projectID);

            IssueType? myType = null;
            if (type != "")
                myType = (IssueType?)Enum.Parse(typeof(IssueType), type);

            State? myState = null;
            if (state != "")
                myState = (State?)Enum.Parse(typeof(State), state);

            IEnumerable<Issue> allIssues = _issueManager.GetAll(ViewBag.Project, myType, myState);
            if (allIssues == null)
            {
                allIssues = new List<Issue>();
            }

            return View(allIssues);
        }

        public ActionResult Discuss(int id)
        {
            return RedirectToAction("Discuss", "Discussion", new { ID = id });
        }

        public ActionResult EditIssue(int id)
        {
            return RedirectToAction("Edit", "Issues", new { ID = id });
        }

        public ActionResult CreateIssue(int projectID)
        {
            return RedirectToAction("Create", "Issues", new { projectId = projectID });
        }

        private Dictionary<Project, dynamic> MakeMeDictionary(List<Project> projects)
        {
            Dictionary<Project, dynamic> myDic = new Dictionary<Project, dynamic>();
            if (projects != null)
            {
                foreach (var item in projects)
                {
                    myDic.Add(item, Infos(item));
                }
            }
            return myDic;
        }

        private dynamic Infos(Project project)
        {
            List<Issue> issues = _issueManager.GetAllFromProject(project);
            if (issues == null)
            {
                issues = new List<Issue>();
            }

            int all = issues.Count;
            int newone = 0;
            int progresone = 0;
            int solved = 0;
            int denied = 0;
            int errors = 0;
            int req = 0;

            foreach (var item in issues)
            {
                if (item.IssueState == State.denied)
                {
                    denied++;
                }
                if (item.IssueState == State.inprogress)
                {
                    progresone++;
                }
                if (item.IssueState == State.isnew)
                {
                    newone++;
                }
                if (item.IssueState == State.solved)
                {
                    solved++;
                }
                if (item.IssueType == IssueType.error)
                {
                    errors++;
                }
                if (item.IssueType == IssueType.request)
                {
                    req++;
                }
            }

            return new
            {
                ALL = all,
                NEWONE = newone,
                PROGRESSONE = progresone,
                SOLVED = solved,
                DENIED = denied,
                ERRORS = errors,
                REQ = req
            };
        }
    }
}