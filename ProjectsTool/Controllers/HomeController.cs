using ProjectsTool.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectsTool.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ProjectModel projectModel = new ProjectModel();
            List<SingleProjectModel> Projects = new List<SingleProjectModel>();
            List<Person> resources = new List<Person>();
            Person ManagerName = null;
            Client ClientName = null;
            List<ActiveProject> activeProjects = new List<ActiveProject>();
            Project projects = null;
            List<SingleProjectModel> Proj = new List<SingleProjectModel>();

            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                activeProjects = db.ActiveProject.ToList();
                
                foreach (ActiveProject a in activeProjects)
                {
                    projects = db.Project.Where(l => l.IDProject == a.IDProject).FirstOrDefault();
                    ClientName = db.Client.Where(l => l.IDClient == projects.IDClient).FirstOrDefault();
                    ManagerName = db.Person.Where(l => l.IDPerson == projects.IDPerson).FirstOrDefault();
                    Projects = db.ActiveProject.Include(m => m.Project).Where(l => l.IDProject == a.IDProject).Select(l => new SingleProjectModel()
                    {
                        ProjectName = l.Project.ProjectName,
                        StartDate = l.Project.StartDate,
                        EndDate = l.Project.EndDate,
                        IsFinish = l.Project.IsFinish,
                        ClientName = ClientName.Name,
                        ManagerName = ManagerName.Name

                    }).ToList();

                    Proj.Add(Projects[0]);

                    resources = db.Person.Where(l => l.IDPerson == a.IDPerson).ToList();
            }

            }
                

            projectModel.Projects = Proj;
            projectModel.Resources = resources;
                return View(projectModel);
        }

        public ActionResult Resources()
        {

            return View();
        }

     

    }
}