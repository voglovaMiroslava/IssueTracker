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

            ProjectManager manager = new ProjectManager();

            ViewBag.Project = manager.GetById(1);
            ViewBag.ClientProject = manager.GetByClient("fj");
            ViewBag.AllProjects = manager.GetAll();

            Project project = new Project(3, "jana", "sbirani Jahod");
            manager.Add(ref project);
            project.Popis = "Jak sbirat jahody ve farmiville";
            ViewBag.ShoudBeTrue = manager.Update(project);
            ViewBag.ShoudBeFalse = manager.Update(new Project(10,"nnn","JJJ"));

            return View();
        }
    }
}