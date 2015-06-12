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

            DiscussionManager discusion = new DiscussionManager();
            Comment comm = new Comment();
            comm.Content = "Šel pes do lesa";
            comm.Diskuter = "<br></br>";
            comm.IDissue = 6;

            discusion.AddComment(comm);
            discusion.AddComment(1, "brr", "ja");

            ViewBag.SearchComments1 = discusion.GetAllComments(1);
            ViewBag.SearchComments6 = discusion.GetAllComments(6);
            return View();
        }
    }
}