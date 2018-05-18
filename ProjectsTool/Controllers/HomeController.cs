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
        public ActionResult CheckDateProjects(int IDManager)
        {
            Project projects = null;
            var yesterday = DateTime.Now.AddDays(-1);

            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                projects = db.Project.Where(l => l.IDPerson == IDManager && l.EndDate <= yesterday && l.IsFinish == false).FirstOrDefault();
            }
                if(projects != null)
                {
                    return Json(new { flag = true , IDManager}, JsonRequestBehavior.AllowGet);
                }
                else
                {
                return Json(new { flag = false }, JsonRequestBehavior.AllowGet);
                }
                
        }
        public ActionResult DateProjectsModal(int IDManager)
        {
            Project project = null;
            var yesterday = DateTime.Now.AddDays(-1);
            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                project = db.Project.Where(l => l.IDPerson == IDManager && l.EndDate <= yesterday).FirstOrDefault();
            }

            return PartialView(project);
        }

        public ActionResult AJAXConcludeProject(int IDProject)
        {
            List<ActiveProject> activeProjects = new List<ActiveProject>();
            Project project = null;
            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                project = db.Project.Where(l => l.IDProject == IDProject).FirstOrDefault();
                activeProjects = db.ActiveProject.Where(l => l.IDProject == IDProject).ToList();

                project.IsFinish = true;
                project.EndDate = DateTime.Now;
                if (activeProjects != null)
                {
                    foreach (ActiveProject a in activeProjects)
                    {
                        a.EndActiveDate = DateTime.Now;

                    }
                }
                db.SaveChanges();
            }
            return Json(new { messaggio = "The project is concluded" });
        }

        public ActionResult PostponeProject(int IDProject)
        {
            Project project = null;
            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                project = db.Project.Where(l => l.IDProject == IDProject).FirstOrDefault();
            }
            return PartialView(project);
        }

        public ActionResult DoPostponeProject(int IDProject, DateTime EndDate)
        {
            Project project = null;
            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                project = db.Project.Where(l => l.IDProject == IDProject).FirstOrDefault();
                if (EndDate != null && EndDate > DateTime.Now)
                {
                    project.EndDate = EndDate;
                    db.SaveChanges();
                    return Json(new { flag = true, messaggio = "End Date modified with success" });
                }
                else
                {
                    return Json(new { flag = false, messaggio = "The End Date is null or is too low" });
                }
            }
        }

        public ActionResult Index()
        {

            string EMail = ((System.Security.Claims.ClaimsIdentity)HttpContext.GetOwinContext().Authentication.User.Identity).Name;
            int IDManager = 0;
            ProjectModel projectModel = new ProjectModel();
            List<SingleProjectModel> Projects = new List<SingleProjectModel>();
            List<ProjectResource> resources = new List<ProjectResource>();
            Person ManagerName = null;
            Client ClientName = null;
            List<ActiveProject> activeProjects = new List<ActiveProject>();
            List<Project> projects = new List<Project>();
            List<SingleProjectModel> Proj = new List<SingleProjectModel>();
            List<ProjectResource> projectResources = new List<ProjectResource>();
            Person d = null; ;

            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                d = db.Person.Where(l => l.EMail == EMail).FirstOrDefault();
                if (d != null)
                {
                    IDManager = d.IDPerson;


                }

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
                    if(IDManager == a.IDPerson)
                    {
                        Projects[0].IsYourManager = true;
                    }
                    else
                    {
                        Projects[0].IsYourManager = false;
                    }
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
            projectModel.IDManager = IDManager;
            projectModel.ManagerName = d.Name;
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
                    Resources = l.Person,
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
            ProjectResource projectResources = new ProjectResource();
            List<ProjectResource> freeResources = new List<ProjectResource>();
            List<Person> resources = new List<Person>();
            AddResourceModel model = new AddResourceModel();
            List<int> resourcesPercentage = new List<int>();
            List<int> arrayResource = new List<int>();
            bool flag = false;
            bool activeFlag = false;
            bool projectFlag = false;
            bool alreadyActive = false;
            int num = 0;
            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                resources = db.Person.Where(l => l.IDRole == 0).ToList();
                activeProjects = db.ActiveProject.ToList();

                //controllo della percentuale delle risorse, se sono libere, se percentuale < 100
                //se rispettano i controlli vengono visualizzate
                foreach (Person p in resources)
                {
                    activeFlag = false;
                    projectResources = new ProjectResource();
                    projectFlag = false;
                    alreadyActive = false;
                    flag = false;
                    
                    foreach (ActiveProject z in activeProjects)
                    {
                        if(p.IDPerson == z.IDPerson)
                        {
                            activeFlag = true;
                        }
                        if(z.IDProject == IDProject && p.IDPerson == z.IDPerson)
                        {
                            
                            projectFlag = true;
                        }
                      
                    }
                    
                    foreach (ActiveProject a in activeProjects)
                    {
                        for(int i = 0; i< arrayResource.Count; i++)
                        {
                            if (p.IDPerson == arrayResource[i] && a.IDPerson == p.IDPerson)
                            {
                                if ((a.Percentage + resourcesPercentage[i]) == 100)
                                {
                                    flag = true;
                                    freeResources.Remove(freeResources[i]);
                                }
                                else
                                {
                                    flag = false;
                                }
                            }
                        }
                        
                        if (p.IDPerson == a.IDPerson && flag == false && projectFlag ==false && alreadyActive == false)
                        {
                            projectResources.Percentage = a.Percentage;
                            if (projectResources.Percentage < 100)
                            {
                                projectResources.Resources = p;
                                projectResources.Percentage = a.Percentage;
                                activeFlag = true;
                                alreadyActive = true;
                                resourcesPercentage.Add(projectResources.Percentage);
                                arrayResource.Add(projectResources.Resources.IDPerson);
                                freeResources.Add(projectResources);
                                
                                
                                
                            
                            }
                        }
                        else if (p.IDPerson != a.IDPerson && activeFlag == false && projectFlag == false)
                        {
                            projectResources.Resources = p;
                            projectResources.Percentage = 0;
                            resourcesPercentage.Add(projectResources.Percentage);
                            arrayResource.Add(projectResources.Resources.IDPerson);
                            freeResources.Add(projectResources);
                           
                           
                            activeFlag = true;

                        }
                        
                        
                        
                           
                        }
                    num++;
                        
                    }
                }
               
            

            model.ProjectResources = freeResources;
            model.IDProject = IDProject;
            return PartialView(model);
        }
        
        public ActionResult AddResourceProject(int IDPerson, int IDProject)
        {
            ActiveResourceModel projectResource = new ActiveResourceModel();
            Person resource = null;
          
            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                resource = db.Person.Where(l => l.IDPerson == IDPerson).FirstOrDefault();
            }
            projectResource.ProjectResource = resource.IDPerson;
            projectResource.IDProject = IDProject;

            return PartialView(projectResource);
        }

        public ActionResult DoAddResourceProject(ActiveResourceModel data)
        {
            List<ActiveProject> activeProject = new List<ActiveProject>();
            ActiveProject projectToAdd = new ActiveProject();
            bool flag = false;
            int percentage = 0;
            //ActiveResourceModel model = new ActiveResourceModel();

            if (data.ActiveProject.EndActiveDate < data.ActiveProject.StartActiveDate
                || data.ActiveProject.EndActiveDate < DateTime.Now)
            {
                return Json(new { messaggio = "The End Date is not valid!", flag = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                using (ProjectToolsEntities db = new ProjectToolsEntities())
                {
                    activeProject = db.ActiveProject.ToList();
                    foreach (ActiveProject a in activeProject)
                    {
                        if (data.ProjectResource == a.IDPerson)
                        {
                            percentage += a.Percentage;

                        }
                        else if (data.ProjectResource != a.IDPerson && flag == false)
                        {
                            percentage += data.ActiveProject.Percentage;
                            flag = true;
                        }

                    }
                    if (percentage <= 100 && percentage > 0)
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
                        return Json(new
                        {
                            messaggio = $"The percentage is bigger than 100% or is less than 0%, insert another percentage!"
                            ,
                            flag = true,
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

        public ActionResult ModifyForm(int IDProject)
        {
            Project project = null;
            List<Client> clientlist = new List<Client>();
            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                project = db.Project.Where(l => l.IDProject == IDProject).FirstOrDefault();
                //ViewBag.RoleList = db.Client.Select(r => new SelectListItem() { Value = r.IDClient.ToString(), Text = r.Name }).ToList();



            }
            return PartialView(project);
        }
        
        public ActionResult DoModifyProject(int IDProject, string ProjectName, DateTime StartDate, DateTime EndDate)
        {
            Project project = null;
            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                project = db.Project.Where(l => l.IDProject == IDProject).FirstOrDefault();
                if(ProjectName != null && StartDate != null && EndDate != null && EndDate > DateTime.Now && EndDate > StartDate)
                { 
                    project.ProjectName = ProjectName;
                    project.StartDate = StartDate;
                    project.EndDate = EndDate;
                    db.SaveChanges();
                    return Json(new { flag = true, messaggio = "Project modified with success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { flag = false, messaggio = "The entered data are not correct" }, JsonRequestBehavior.AllowGet);
                }
            }

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

        public ActionResult CheckResourceForConclude(int IDProject)
        {
            ActiveProject activeProject = null;
            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                activeProject = db.ActiveProject.Where(l => l.IDProject == IDProject).FirstOrDefault();
            }
            if(activeProject != null)
            {
                return Json(new { flag = true, messaggio = "There are some active resource on this project!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { flag = false }, JsonRequestBehavior.AllowGet);
            }
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
                //if(activeProjects.Count != 0)
                //{
                    project.IsFinish = true;
                    project.EndDate = DateTime.Now;
                    foreach(ActiveProject a in activeProjects)
                    {
                        a.EndActiveDate = DateTime.Now;
                       
                    }
                    db.SaveChanges();
                    return Json(new { flag = true, messaggio = "Project concluded with success" }, JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                //    return Json(new { flag = false, messaggio = "You can't conclude this project because is not active" },JsonRequestBehavior.AllowGet);
                //}
            }


        }

        public ActionResult AddProject()
        {
            List<Client> clientlist = new List<Client>();
            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                ViewBag.RoleList = db.Client.Select(r => new SelectListItem() { Value = r.IDClient.ToString(), Text = r.Name }).ToList();


                
            }
            return PartialView();
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
            return Json(new { messaggio = $"Project{data.IDProject} add with success", flag = true }, JsonRequestBehavior.AllowGet);

        }else
            {
                return Json(new { messaggio = $"Dati mancanti o non validi", flag = false }, JsonRequestBehavior.AllowGet);
            }
        }
        
    }
}