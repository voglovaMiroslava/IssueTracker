using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IssueTracker.Models;

namespace IssueTracker.Controllers
{
    public class IssuesController : Controller
    {
        private ProjectManager _projectManager = new ProjectManager();
        private IssueManager _issueManager = new IssueManager();
        private PersonManager _personManager = new PersonManager();

        // GET: Issues
        public ActionResult All()
        {
            ViewBag.AllState = MySelectLists.GetStates();
            ViewBag.AllType = MySelectLists.GetTypes();
            ViewBag.AllProject = MySelectLists.GetAllProjectNames();
            ViewBag.Dictionary = MakeMeDict(_projectManager.GetAll());

            return View(_issueManager.GetAll());
        }

        [HttpPost]
        public ActionResult All(FormCollection form)
        {
            All();

            string state = form["StateFilter"];
            string type = form["TypeFilter"];
            string project = form["ProjectFilter"];

            State? enState = null;
            if (state != "")
            {
                enState = (State?)Enum.Parse(typeof(State), state);
            }

            IssueType? enType = null;
            if (type != "")
            {
                enType = (IssueType?)Enum.Parse(typeof(IssueType), type);
            }

            List<Issue> findedIssues = new List<Issue>();
            List<Project> projectSort = _projectManager.GetAll(null, project);
            foreach (var item in projectSort)
            {
                List<Issue> projectIssues = _issueManager.GetAll(item, enType, enState);
                if (projectIssues != null)
                {
                    findedIssues = findedIssues.Union(projectIssues).ToList();
                }
            }

            return View(findedIssues);
        }

        public ActionResult Create(int? projectID)
        {
            if (projectID == null)
            {
                ViewBag.AllProjects = MySelectLists.GetAllProjectIDs();
            }
            else
            {
                Project proj = _projectManager.GetById(projectID.Value);
                List<SelectListItem> items = new List<SelectListItem>();
                SelectListItem newSel = new SelectListItem();
                newSel.Selected = true;
                newSel.Value = proj.ID.ToString();
                newSel.Text = proj.Name;
                items.Add(newSel);
                ViewBag.AllProjects = items;
            }

            ViewBag.AllTypes = MySelectLists.GetTypes();

            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            string name = form["Name"];
            string addedBy = form["AddedBy"];
            int projectID = Convert.ToInt32(form["Project"]);
            string type = form["Type"];
            string description = form["Content"];

            if (name == "")
            {
                ViewBag.NotValidName = "Issue needs name!";
            }
            if (addedBy == "")
            {
                ViewBag.NotValidAdded = "Don't be shy. Tell us who you are.";
            }
            if (name == "" || addedBy == "")
            {
                return Create((int?)null);
            }
            IssueType typo = IssueType.request;
            if (type == "error")
            {
                typo = IssueType.error;
            }

            _issueManager.Create(new Issue(description, name, addedBy, projectID, typo, State.isnew));
            return RedirectToAction("All");
        }

        public ActionResult Edit(int id)
        {
            ViewBag.AllStates = MySelectLists.GetStates();
            ViewBag.AllEmplo = MySelectLists.GetAllEmplo();
            return View(_issueManager.GetById(id));
        }

        [HttpPost]
        public ActionResult Edit(FormCollection form)
        {
            bool okImput = true;
            int issueId = Convert.ToInt32(form["IssueID"]);
            string name = form["Name"];
            string issueState = form["State"];
            string assignedTo = form["AssignedTo"];
            string finished = form["FinishedDate"];
            string content = form["Content"];

            DateTime date = default(DateTime);

            if (name == "")
            {
                ViewBag.NotValidName = "Name can not be empty!";
                okImput = false;
            }

            if (finished != "dd-MM-yyyy")
            {
                try
                {
                    date = Convert.ToDateTime(finished);
                }
                catch (System.FormatException ex)
                {
                    ViewBag.NotValidDate = "This date doesn't seem alright. Try date in format dd-MM-yyyy.";
                    okImput = false;
                }
            }

            if (date != default(DateTime) && date < DateTime.Today)
            {
                ViewBag.NotValidDate = "This date doesn't seem alright. It's too small.";
                okImput = false;
            }

            if (okImput)
            {
                Issue issue = new Issue();
                issue.ID = issueId;
                issue.Name = name;
                issue.IssueState = (State) Enum.Parse(typeof(State), issueState);
                issue.AssignedTo = assignedTo;
                if (date != default(DateTime))
                {
                    issue.FinishedDate = date;
                }
                issue.Content = content;
                _issueManager.Update(issue);
                return RedirectToAction("All");
            }

            ViewBag.AllStates = MySelectLists.GetStates();
            ViewBag.AllEmplo = MySelectLists.GetAllEmplo();

            return View(_issueManager.GetById(issueId));
        }

        private Dictionary<int, string> MakeMeDict(List<Project> projects)
        {
            Dictionary<int, string> myDic = new Dictionary<int, string>();

            foreach (var item in projects)
            {
                myDic.Add(item.ID, item.Name);
            }
            return myDic;
        }
    }
}