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
            ProjectModel projectModel = new ProjectModel();
            ProjectResource projectAttive = new ProjectResource();
            List<Project> projects = new List<Project>();
            List<ActiveProject> ProgettiActivi = new List<ActiveProject>();

        

            bool flag = false;

            string EMail = ((System.Security.Claims.ClaimsIdentity)HttpContext.GetOwinContext().Authentication.User.Identity).Name;
            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {

                var d = db.Person.Where(l => l.EMail == EMail).FirstOrDefault();
                activeProjects = db.ActiveProject.ToList();

            

                

                    foreach (ActiveProject a in activeProjects)
                    {
                        flag = false;
                        if (IDPerson == a.IDPerson)
                        {
                           
                                projectResource.Percentage += a.Percentage;
                            if (projectResource.Percentage < 100)
                            {


                                if (flag == false)
                                {


                                ProjectResource.projects = db.Project.Where(m => m.IDPerson == d.IDPerson).ToList();

                                ProjectResource.IDPerson = IDPerson;

                            }
                           

                        }
                        return PartialView(ProjectResource);
                    }
                                                       
                        else
                        {
                            //TempData["msg"] = "<script>alert('Impossibile to add project at this resource because has got more 100% of percentage');</script>";
                            return RedirectToAction("NonAssegnabile");

                        }


                    }     

            }

            return PartialView();

        }

        public ActionResult NonAssegnabile()
        {
            return PartialView();
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

                    projectResource.ProjectResource = IDProject;
                    projectResource.IDPerson = IDPerson;
                }

            }
            return PartialView(projectResource);
        }

        public ActionResult DoAddProjectResource(ActiveProjectModel data)
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
                    if (data.IDPerson == a.IDPerson && a.Percentage != 0)
                    {
                        percentage += a.Percentage;

                    }
                    else if (data.IDPerson != a.IDPerson && flag == false)
                    {
                        percentage += data.ActiveProject.Percentage;
                        flag = true;
                    }
                }
                    if (percentage <= 100 && percentage > 0)
                    {

                        data.IDProject = data.ProjectResource;

                        projectToAdd.IDPerson = data.IDPerson;
                        projectToAdd.IDProject = data.IDProject;
                        projectToAdd.Percentage = data.ActiveProject.Percentage;
                        projectToAdd.StartActiveDate = data.ActiveProject.StartActiveDate;
                        projectToAdd.EndActiveDate = data.ActiveProject.EndActiveDate;
                      
                        db.ActiveProject.Add(projectToAdd);
                        db.SaveChanges();
                    }

                    else
                    {
                    return Json(new
                    {
                        messaggio = $"The percentage is bigger than 100% or is less than 0%, insert another percentage!"
                        , flag = true,
                        JsonRequestBehavior.AllowGet
                    });
                }   
              }
            return Json(new
            {
                messaggio = $"The resource is now active in this project with a {projectToAdd.Percentage}%"
                    ,
                flag = false,
                JsonRequestBehavior.AllowGet
            });
        }
        
    }

}

