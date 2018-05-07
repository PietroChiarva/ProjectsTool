using ProjectsTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using System.Data.Services.Client;
using System.IO;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using ProjectsTool.Controllers;
using Microsoft.Ajax.Utilities;
using System.Web.Routing;

namespace ProjectsTool.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Resources()
        {
            using (ProjectToolsEntities db = new ProjectToolsEntities()) {
                List<Person> resources = new List<Person>();
                resources = db.Person.Where(x => x.IDRole == 0).ToList();

                return View(resources);
            }
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
            if (!string.IsNullOrEmpty(data.ProjectName) &&  data.StartDate.HasValue && data.EndDate.HasValue /*&& data.Client.IDClient != 0*/)
            {
                using (ProjectToolsEntities db = new ProjectToolsEntities())
                {
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