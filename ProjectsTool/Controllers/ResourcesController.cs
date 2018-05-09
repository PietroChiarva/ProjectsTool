using ProjectsTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace ProjectsTool.Controllers
{
    public class ResourcesController : Controller
    {


        public ActionResult Resources()
        {
            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                List<Person> resources = new List<Person>();
                resources = db.Person.Where(x => x.IDRole == 0).ToList();

                return View(resources);
            }
        }

        public ActionResult ResourceProject(int IDPerson)
        {
            List<SingleProjectModel> projects = new List<SingleProjectModel>();
            List<ActiveProject> activeProjects = new List<ActiveProject>();
            List<SingleProjectModel> proj = new List<SingleProjectModel>();

            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                activeProjects = db.ActiveProject.Where(l => l.IDPerson == IDPerson).ToList();
                foreach (ActiveProject a in activeProjects)
                {
                    projects = db.ActiveProject.Include(m => m.Project).Where(l => l.IDProject == a.IDProject).Select(l => new SingleProjectModel()
                    {
                        ProjectName = l.Project.ProjectName,
                        StartDate = l.Project.StartDate,
                        EndDate = l.Project.EndDate,
                        IsFinish = l.Project.IsFinish,
                        Percentage = l.Percentage,

                    }).ToList();

                    proj.Add(projects[0]);
                }
            }
            return PartialView(proj);
        }

        public ActionResult SeeProject(int IDPerson)
        {

            return PartialView();
        }

        public ActionResult ProjectStoryResource(int IDPerson)
        {
            List<SingleProjectModel> projects = new List<SingleProjectModel>();
            List<ActiveProject> activeProjects = new List<ActiveProject>();
            List<SingleProjectModel> proj = new List<SingleProjectModel>();

            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                activeProjects = db.ActiveProject.Where(l => l.IDPerson == IDPerson).ToList();
                foreach (ActiveProject a in activeProjects)
                {
                    projects = db.ActiveProject.Include(m => m.Project).Where(l=> l.IDPerson == IDPerson && l.EndActiveDate < DateTime.Now).Select(l => new SingleProjectModel()
                    {
                        ProjectName = l.Project.ProjectName,
                        StartDate = l.Project.StartDate,
                        EndDate = l.Project.EndDate,
                        IsFinish = l.Project.IsFinish,
                        Percentage = l.Percentage,

                    }).ToList();
                    



                }

            }
            return PartialView(projects);
        }
    }
}
