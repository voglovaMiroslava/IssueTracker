using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IssueTracker.Models;

namespace IssueTracker.Controllers
{
    public class DiscussionController : Controller
    {
        private IssueManager _issueManager = new IssueManager();
        private DiscussionManager _discusionManager = new DiscussionManager();

        // GET: Discussion
        public ActionResult Discuss(int id)
        {
            ViewBag.Issue = _issueManager.GetById(id);
            return View(_discusionManager.GetAllComments(id));
        }

        public ActionResult Add(int id)
        {
            ViewBag.IssueID = id;
            return View();
        }

        [HttpPost]
        public ActionResult Add(FormCollection form)
        {
            string name = form["Diskuter"];
            string content = form["Content"];
            int id = Convert.ToInt32(form["IssueId"]);
            if (name == "")
            {
                ViewBag.NotValidName = "Don't be shy. Tell us who you are.";
            }
            if (content == "")
            {
                ViewBag.NotValidContent = "Didn't you want to write something?";
            }
            if(name == "" || content == "")
            {
                return Add(id);
            }

            _discusionManager.AddComment(id, content, name);

            return RedirectToAction("Discuss", new { ID = id});
        }
    }
}