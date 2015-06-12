using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace IssueTracker.Models
{
    public class DiscussionManager : IDiscussionManager
    {
        private XDocument _diskuseXML;
        private string _pathToXML;

        public DiscussionManager()
        {
            _pathToXML = HttpContext.Current.Server.MapPath("~/App_Data/Diskuse.xml");
            _diskuseXML = XDocument.Load(_pathToXML);
        }

        /// <summary>
        /// Komentar musi obsahovat minimalne jmeno diskutera, obsah a idIssue, ke ktere se pridava.
        /// Added time nastaven az v teto fci.
        /// </summary>
        /// <param name="comment"></param>
        public void AddComment(Comment comment)
        {
            AddComment(comment.IDissue, comment.Content, comment.Diskuter);
        }

        public void AddComment(int issueID, string comment, string name)
        {
            IEnumerable<XElement> discusions = _diskuseXML.Root.Descendants("diskuse").
                Where(a => Convert.ToInt32(a.Attribute("idpozadavku").Value) == issueID);

            XElement myDiscusion;
            int poradi;
            if (discusions.Count() == 0)
            {
                myDiscusion = new XElement("diskuse", new XAttribute("idpozadavku", issueID));
                poradi = 1;
            }
            else
            {
                myDiscusion = discusions.First();
                poradi = Convert.ToInt32(myDiscusion.Descendants("zaznam").Last().Attribute("poradi").Value) + 1;
            }

            XElement myComment = new XElement("zaznam", new XAttribute("poradi", poradi),
                new XElement("diskuter", HttpUtility.HtmlEncode(name)),
                new XElement("obsah", HttpUtility.HtmlEncode(comment)),
                new XElement("datum", string.Format("{0:yyyy-MM-ddThh:mm:ss}", DateTime.Now)));

            myDiscusion.Add(myComment);

            if (discusions.Count() == 0)
            {
                _diskuseXML.Root.Add(myDiscusion);
            }

            _diskuseXML.Save(_pathToXML);
        }

        /// <summary>
        /// Vrati null pokud nebudou zadne komenty pod danym issue
        /// </summary>
        /// <param name="issueID"></param>
        /// <returns></returns>
        public List<Comment> GetAllComments(int issueID)
        {
            IEnumerable<XElement> discusion = _diskuseXML.Root.Descendants("diskuse").
                Where(a => Convert.ToInt32(a.Attribute("idpozadavku").Value) == issueID);
            if (discusion.Count() == 0)
            {
                return null;
            }
            IEnumerable<XElement> commnetsIe = discusion.First().Descendants("zaznam");
            List<Comment> comments = new List<Comment>();

            foreach (var item in commnetsIe)
            {
                Comment newComment = new Comment();
                newComment.Content = HttpUtility.HtmlDecode(item.Descendants("obsah").First().Value);
                newComment.Added = Convert.ToDateTime(item.Descendants("datum").First().Value);
                newComment.Diskuter = HttpUtility.HtmlDecode(item.Descendants("diskuter").First().Value);
                newComment.IDissue = issueID;
                newComment.Order = Convert.ToInt32(item.Attribute("poradi").Value);
                comments.Add(newComment);
            }

            return comments;
        }
    }
}
