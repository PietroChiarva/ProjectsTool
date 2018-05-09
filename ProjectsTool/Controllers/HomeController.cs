using ProjectsTool.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using System;

namespace ProjectsTool.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //int IDPerson = 0;
            //int IDRole = 1;
            string EMail = ((System.Security.Claims.ClaimsIdentity)HttpContext.GetOwinContext().Authentication.User.Identity).Name;
            ProjectModel projectModel = new ProjectModel();
            List<SingleProjectModel> Projects = new List<SingleProjectModel>();
            List<ProjectResource> resources = new List<ProjectResource>();
            Person ManagerName = null;
            Client ClientName = null;
            List<ActiveProject> activeProjects = new List<ActiveProject>();
            List<Project> projects = new List<Project>();
            List<SingleProjectModel> Proj = new List<SingleProjectModel>();
            List<ProjectResource> projectResources = new List<ProjectResource>();

            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                //var d = db.Person.Where(l => l.EMail == EMail).FirstOrDefault();
                //if (d != null)
                //{
                //    Session["IDPerson"] = d.IDPerson;
                //    IDRole = d.IDRole;
                //    IDPerson = d.IDPerson;
                //}

                    projects = db.Project.ToList();
                foreach (Project a in projects)
                {
                    ClientName = db.Client.Where(l => l.IDClient == a.IDClient).FirstOrDefault();
                    ManagerName = db.Person.Where(l => l.IDPerson == a.IDPerson).FirstOrDefault();
                    Projects = db.Project.Where(l => l.IDProject == a.IDProject).Select(l => new SingleProjectModel()
                    {
                        ProjectName = l.ProjectName,
                        StartDate = l.StartDate,
                        EndDate = l.EndDate,
                        IsFinish = l.IsFinish,
                        ClientName = ClientName.Name,
                        ManagerName = ManagerName.Name,
                        IDProject = l.IDProject,

                    }).ToList();
                    Proj.Add(Projects[0]);
                }
                activeProjects = db.ActiveProject.ToList();
                foreach (ActiveProject a in activeProjects)
                {
                    //projects = db.Project.Where(l => l.IDProject == a.IDProject).FirstOrDefault();
                    //ClientName = db.Client.Where(l => l.IDClient == projects.IDClient).FirstOrDefault();
                    //ManagerName = db.Person.Where(l => l.IDPerson == projects.IDPerson).FirstOrDefault();
                    //Projects = db.ActiveProject.Include(m => m.Project).Where(l => l.IDProject == a.IDProject).Select(l => new SingleProjectModel()
                    //{
                    //    ProjectName = l.Project.ProjectName,
                    //    StartDate = l.Project.StartDate,
                    //    EndDate = l.Project.EndDate,
                    //    IsFinish = l.Project.IsFinish,
                    //    ClientName = ClientName.Name,
                    //    ManagerName = ManagerName.Name,
                    //    IDProject = l.IDProject,

                    //}).ToList();

                    //projectResources = db.ActiveProject.Include(m => m.Person).Where(l => l.IDPerson == a.IDPerson).Select(l => new ProjectResource()
                    //{
                    //    Serial = l.Person.Serial,
                    //    Name = l.Person.Name,
                    //    Surname = l.Person.Surname,
                    //    Email = l.Person.EMail,
                    //    Percentage = l.Percentage
                    //}).ToList();

                    

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
                return Json(new { projectModel.ProjectResources } , JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SeeResource(int IDProject)
        {
            List<ActiveProject> activeProjects = new List<ActiveProject>();
            List<ProjectResource> projectResources = new List<ProjectResource>();
            List<ProjectResource> freeResources = new List<ProjectResource>();

            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                activeProjects = db.ActiveProject.ToList();
                foreach (ActiveProject a in activeProjects)
                {
                    projectResources = db.ActiveProject.Include(m => m.Person).Select(l => new ProjectResource()
                    {
                        Serial = l.Person.Serial,
                        Name = l.Person.Name,
                        Surname = l.Person.Surname,
                        Email = l.Person.EMail,
                        Percentage = l.Percentage
                    }).ToList();

                    if(projectResources[0].Percentage < 100)
                    {
                        freeResources.Add(projectResources[0]);
                    }
                }
            }
            return PartialView();
        }

        public ActionResult ModifyForm(int IDProject)
        {
            Project project = null;
            List<Client> clientlist = new List<Client>();
            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                project = db.Project.Where(l => l.IDProject == IDProject).FirstOrDefault();
                ViewBag.RoleList = db.Client.Select(r => new SelectListItem() { Value = r.IDClient.ToString(), Text = r.Name }).ToList();



            }
            return PartialView(project);
        }
        
        public ActionResult DoModifyProject(Project data)
        {
            Project project = null;
            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                project = db.Project.Where(l => l.IDProject == data.IDProject).FirstOrDefault();
                project.ProjectName = data.ProjectName;
                project.StartDate = data.StartDate;
                project.EndDate = data.EndDate;
                project.Client = data.Client;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult DeleteModal(int IDProject)
        {
            Project project = null;
            List<ActiveProject> activeProject = new List<ActiveProject>();
            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                project = db.Project.Where(l => l.IDProject == IDProject).FirstOrDefault();
                activeProject = db.ActiveProject.Where(l => l.IDProject == project.IDProject).ToList();
                if (activeProject.Count != 0)
                {
                    TempData["msg"] = "<script>alert('There some active resources ih this project!');</script>";
                }
            }
            return PartialView(project);
        }

        public ActionResult DoDeleteProject(int IDProject)
        {
            Project project = null;
           
            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                project = db.Project.Where(l => l.IDProject == IDProject).FirstOrDefault();
               
                db.Project.Remove(project);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult ConcludeModal(int IDProject)
        {
            Project project = null;
            
            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                project = db.Project.Where(l => l.IDProject == IDProject).FirstOrDefault();
                
            }
            return PartialView(project);
        }

        public ActionResult DoConcludeProject(int IDProject)
        {
            List<ActiveProject> activeProjects = new List<ActiveProject>();
            Project project = null;
            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                project = db.Project.Where(l => l.IDProject == IDProject).FirstOrDefault();
                activeProjects = db.ActiveProject.Where(l => l.IDProject == IDProject).ToList();
                if(activeProjects.Count != 0)
                {
                    project.IsFinish = true;
                    project.EndDate = DateTime.Now;
                    foreach(ActiveProject a in activeProjects)
                    {
                        a.EndActiveDate = DateTime.Now;
                       
                    }
                }
                else
                {
                    TempData["mex"] = "<script>alert('This project is not active, you can't conclude it!');</script>";
                }
            }

                return RedirectToAction("Index");
        }

        public ActionResult AddProject()
        {
            List<Client> clientlist = new List<Client>();
            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                ViewBag.RoleList = db.Client.Select(r => new SelectListItem() { Value = r.IDClient.ToString(), Text = r.Name }).ToList();


                
            }
            return View();
        }

        public ActionResult _JSONAddProject(Project data)
        {
            string EMail = ((System.Security.Claims.ClaimsIdentity)HttpContext.GetOwinContext().Authentication.User.Identity).Name;
            if (!string.IsNullOrEmpty(data.ProjectName) &&  data.StartDate.HasValue && data.EndDate.HasValue /*&& data.Client.IDClient != 0*/)
            {
                using (ProjectToolsEntities db = new ProjectToolsEntities())
                {
                  var d = db.Person.Where(l => l.EMail == EMail).FirstOrDefault();
                  var r = db.Person.Where(l => l.IDRole == 1).FirstOrDefault();
                    if (data != null)
                    {
                        data.IDPerson = d.IDPerson;
                    }

                    //data.IDPerson = 2;
                    db.Project.Add(data);

                    db.SaveChanges();
                
            
        }
            return Json(new { messaggio = $"Project{data.IDProject} add with success", flag = true });

        }else
            {
                return Json(new { messaggio = $"Dati mancanti o non validi", flag = false });
            }
        }
        
    }
}