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
                    projects = db.ActiveProject.Include(m => m.Project).Where(l => l.IDPerson == IDPerson && l.EndActiveDate < DateTime.Now).Select(l => new SingleProjectModel()
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

        public ActionResult AssegnaProject(int IDPerson)
        {
            ProjectResourceView ProjectResource = new ProjectResourceView();
            ActiveProjectModel projectResource = new ActiveProjectModel();
            List<ActiveProject> activeProjects = new List<ActiveProject>();
            List<Project> projects = new List<Project>();
            bool flag = false;

            string EMail = ((System.Security.Claims.ClaimsIdentity)HttpContext.GetOwinContext().Authentication.User.Identity).Name;
            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {

                var d = db.Person.Where(l => l.EMail == EMail).FirstOrDefault();
                activeProjects = db.ActiveProject.ToList();
                foreach (ActiveProject a in activeProjects)
                {
                    if (IDPerson == a.IDPerson)
                    {
                        projectResource.Percentage += a.Percentage;
                        if (projectResource.Percentage >= 100)
                        {
                            TempData["msg"] = "<script>alert('Impossibile to add project at this resource because has got more 100% of percentage');</script>";
                            return RedirectToAction("Resources");


                        }
                        else
                        {
                            if (IDPerson == a.IDPerson)
                            {
                                flag = true;
                            }
                            if (flag == false)
                            {

                                ProjectResource.projects = db.Project.Where(m => m.IDPerson == d.IDPerson).ToList();

                            }

                            //projects = db.Project.Where(m => m.IDPerson == IDPerson).ToList();

                            //ProjectResource.projects = db.Project.ToList();
                        }
                    }
                }




            }
            return PartialView(ProjectResource);

        }



        public ActionResult AddProjectResource(int IDPerson, int IDProject)
        {
            ActiveProjectModel projectResource = new ActiveProjectModel();
            ActiveProject activeProject = null;
            Project projects = null;
            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {

                activeProject = db.ActiveProject.Where(l => l.IDPerson == l.IDPerson).FirstOrDefault();
                if (projectResource.Percentage >= 100)
                {

                    TempData["msg"] = "<script>alert('Impossibile to add project at this resource');</script>";

                }
                else
                {
                    projects = db.Project.Where(l => l.IDPerson == IDPerson).FirstOrDefault();

                    projectResource.ProjectResource = projects.IDProject;
                    projectResource.IDPerson = IDPerson;
                }

            }
            return PartialView(projectResource);
        }

        public ActionResult DoAddProjectResource(ActiveResourceModel data)
        {
            List<ActiveProject> activeProject = new List<ActiveProject>();
            ActiveProject projectToAdd = new ActiveProject();
            bool flag = false;
            int percentage = 0;

            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                activeProject = db.ActiveProject.ToList();
                foreach (ActiveProject a in activeProject)
                {
                    if (data.ProjectResource == a.IDPerson)
                    {
                        percentage += a.Percentage;
                        flag = true;
                    }
                    else if (data.ProjectResource != a.IDPerson && flag == false)
                    {
                        percentage += a.Percentage;
                    }

                }
                if ((data.ActiveProject.Percentage + percentage) <= 100)
                {
                    projectToAdd.IDPerson = data.ProjectResource;
                    projectToAdd.IDProject = data.IDProject;
                    projectToAdd.Percentage = data.ActiveProject.Percentage;
                    projectToAdd.StartActiveDate = data.ActiveProject.StartActiveDate;
                    projectToAdd.EndActiveDate = data.ActiveProject.EndActiveDate;
                    db.ActiveProject.Add(projectToAdd);
                    db.SaveChanges();
                }
                else
                {
                    TempData["msg"] = "<script>alert('The percentage is bigger than 100%!');</script>";
                }
            }
            return RedirectToAction("AssegnaProject");
        }

    }
}
