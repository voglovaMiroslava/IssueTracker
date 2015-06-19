using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IssueTracker.Models
{
    public static class MySelectLists
    {
        public static List<SelectListItem> GetStates()
        {
            List<SelectListItem> AllState = new List<SelectListItem>();
            for (int i = 0; i < 4; i++)
            {
                SelectListItem state = new SelectListItem();
                state.Selected = false;
                switch (i)
                {
                    case 0:
                        state.Value = "isnew";
                        state.Text = "new";
                        break;
                    case 1:
                        state.Value = "inprogress";
                        state.Text = "inprogress";
                        break;
                    case 2:
                        state.Value = "solved";
                        state.Text = "solved";
                        break;
                    case 3:
                        state.Value = "denied";
                        state.Text = "denied";
                        break;
                }
                AllState.Add(state);
            }
            return AllState;
        }

        public static List<SelectListItem> GetTypes()
        {
            List<SelectListItem> AllType = new List<SelectListItem>();
            for (int i = 0; i < 2; i++)
            {
                SelectListItem type = new SelectListItem();
                type.Selected = false;
                switch (i)
                {
                    case 0:
                        type.Value = "error";
                        type.Text = "error";
                        break;
                    case 1:
                        type.Value = "request";
                        type.Text = "request";
                        break;

                }
                AllType.Add(type);
            }
            return AllType;
        }

        public static List<SelectListItem> GetAllProjectNames()
        {
            ProjectManager projectManager = new ProjectManager();
            List<Project> allProjects = projectManager.GetAll();
            List<SelectListItem> listProj = new List<SelectListItem>();

            foreach (var item in allProjects)
            {
                SelectListItem project = new SelectListItem();
                project.Selected = false;
                project.Text = item.Name;
                project.Value = item.Name;
                listProj.Add(project);
            }
            return listProj;
        }

        public static List<SelectListItem> GetAllProjectIDs()
        {
            ProjectManager projectManager = new ProjectManager();
            List<Project> allProjects = projectManager.GetAll();
            List<SelectListItem> listProj = new List<SelectListItem>();

            foreach (var item in allProjects)
            {
                SelectListItem project = new SelectListItem();
                project.Selected = false;
                project.Text = item.Name;
                project.Value = item.ID.ToString();
                listProj.Add(project);
            }
            return listProj;
        }

        public static List<SelectListItem> GetAllClientWithProject()
        {
            ProjectManager projectManager = new ProjectManager();
                        List<Project> allCustomers = projectManager.GetAll();
            List<SelectListItem> listClients = new List<SelectListItem>();

            foreach (var custo in allCustomers)
            {
                SelectListItem client = new SelectListItem();
                client.Selected = false;
                client.Value = custo.Client;
                client.Text = custo.Client;
                listClients.Add(client);
            }
            return listClients;
        }

        public static List<SelectListItem> GetAllClients()
        {
            PersonManager personManager = new PersonManager();
            List<Person> fromPersonXml = personManager.GetAllCusto();

            List<SelectListItem> listClients = new List<SelectListItem>();

            foreach (var custo in fromPersonXml)
            {
                SelectListItem client = new SelectListItem();
                client.Selected = false;
                client.Value = custo.Name;
                client.Text = custo.Name;
                listClients.Add(client);
            }

            return listClients;
        }

        public static List<SelectListItem> GetAllEmplo()
        {
            PersonManager personManager = new PersonManager();
            List<Person> fromPersonXml = personManager.GetAllEmplo();

            List<SelectListItem> listEmplo = new List<SelectListItem>();

            foreach (var emplo in fromPersonXml)
            {
                SelectListItem employee = new SelectListItem();
                employee.Selected = false;
                employee.Value = emplo.Name;
                employee.Text = emplo.Name;
                listEmplo.Add(employee);
            }

            return listEmplo;
        }

        public static List<SelectListItem> GetYesNo()
        {
            List<SelectListItem> yesNo = new List<SelectListItem>();
            for (int i = 0; i < 2; i++)
            {
                SelectListItem boolien = new SelectListItem();
                boolien.Selected = false;
                switch (i)
                {
                    case 0:
                        boolien.Value = "YES";
                        boolien.Text = "YES";
                        break;
                    case 1:
                        boolien.Value = "NO";
                        boolien.Text = "NO";
                        break;

                }
                yesNo.Add(boolien);
            }
            return yesNo;
        }
    }
}
