using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IssueTracker.Models;

namespace IssueTracker.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your test page.";

            IssueManager manage = new IssueManager();
            ViewBag.OneIssue = manage.GetById(2);
            ViewBag.IsNewIssue = manage.GetAllByState(State.isnew);
            ViewBag.ErrorIssue = manage.GetAllByType(IssueType.error);
            ViewBag.ProjectIssue = manage.GetAllFromProject(new Project(3, "", ""));
            ViewBag.StateProject = manage.GetAll(new Project(3, "", ""), null, State.isnew);
            return View();
        }
    }
}