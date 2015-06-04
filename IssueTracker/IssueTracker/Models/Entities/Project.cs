using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Models
{
    class Project
    {
        public int ID { get; set; }
        public string Zakaznik { get; set; }
        public string Nazev { get; set; }
        public string Popis { get; set; }

        public Project() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="zadavatel">Can be null.</param>
        /// <param name="nazev">Can be null.</param>
        public Project(int id, string zadavatel, string nazev)
        {
            ID = id;
            if (zadavatel != null)
            {
                Zakaznik = zadavatel;
            }
            if (nazev != null)
            {
                Nazev = nazev;
            }
        }

        public override string ToString()
        {
            return String.Format("ID: {2} Name: {0}, Assigned by: {1}", Nazev, Zakaznik, ID);
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
