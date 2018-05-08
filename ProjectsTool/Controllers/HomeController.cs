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
            List<ProjectResource> resources = new List<ProjectResource>();
            Person ManagerName = null;
            Client ClientName = null;
            List<ActiveProject> activeProjects = new List<ActiveProject>();
            Project projects = null;
            List<SingleProjectModel> Proj = new List<SingleProjectModel>();
            List<ProjectResource> projectResources = new List<ProjectResource>();

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
                        ManagerName = ManagerName.Name,
                        IDProject = l.IDProject,

                    }).ToList();

                    //projectResources = db.ActiveProject.Include(m => m.Person).Where(l => l.IDPerson == a.IDPerson).Select(l => new ProjectResource()
                    //{
                    //    Serial = l.Person.Serial,
                    //    Name = l.Person.Name,
                    //    Surname = l.Person.Surname,
                    //    Email = l.Person.EMail,
                    //    Percentage = l.Percentage
                    //}).ToList();

                    Proj.Add(Projects[0]);

                    //resources.Add(projectResources[0]);
            }

            }
                

            projectModel.Projects = Proj;
            //projectModel.ProjectResources = resources;
                return View(projectModel);
        }

        public ActionResult GetResources(int IDProject)
        {
            List<ProjectResource> projectResources = new List<ProjectResource>();
            List<ActiveProject> activeProjects = new List<ActiveProject>();
            List<ProjectResource> resources = new List<ProjectResource>();
            ProjectModel projectModel = new ProjectModel();

            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                activeProjects = db.ActiveProject.Where(l => l.IDProject == IDProject).ToList();

                foreach (ActiveProject a in activeProjects)
                {
                    projectResources = db.ActiveProject.Include(m => m.Person).Where(l => l.IDPerson == a.IDPerson).Select(l => new ProjectResource()
                    {
                        Serial = l.Person.Serial,
                        Name = l.Person.Name,
                        Surname = l.Person.Surname,
                        Email = l.Person.EMail,
                        Percentage = l.Percentage
                    }).ToList();

                    resources.Add(projectResources[0]);
                }
                projectModel.ProjectResources = resources;
                return Json(new { projectModel });
            }
        }

        public ActionResult Resources()
        {

            return View();
        }

     

    }
}