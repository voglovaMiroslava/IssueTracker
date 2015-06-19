using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Models
{
    public class Project
    {
        public int ID { get; set; }
        public string Client { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Project() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="zadavatel">Can be null.</param>
        /// <param name="nazev">Can be null.</param>
        public Project(int id, string zadavatel, string nazev, string content)
        {
            ID = id;
            if (zadavatel != null)
            {
                Client = zadavatel;
            }
            if (nazev != null)
            {
                Name = nazev;
            }
            if (content != null)
            {
                Description = content;
            }
        }

        public override string ToString()
        {
            return String.Format("ID: {2} Name: {0}, Assigned by: {1}", Name, Client, ID);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;


            Project s = obj as Project;
            if (s == null)
                return false;

            return ID == s.ID;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}
