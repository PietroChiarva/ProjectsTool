using ProjectsTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectsTool.Controllers
{
    public class ClientSectionController : Controller
    {
        // GET: ClientSection
        public ActionResult ClientSection()
        {
            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                
                List<Client> c = new List<Client>();
                ProjectModel project = new ProjectModel();
                c = db.Client.ToList();

                project.Clients = c;

                return View(project);
            }
        }

        public ActionResult AddNewClient()
        {
            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {

            }
                return PartialView();
        }

        public ActionResult _JSONAddNewClient(Client data)
        {
          ;
            
           
            if (!string.IsNullOrEmpty(data.Name) && !string.IsNullOrEmpty(data.Society) && !string.IsNullOrEmpty(data.NumberPhone) && !string.IsNullOrEmpty(data.Email) && !string.IsNullOrEmpty(data.PartitaIva))
            {
                using (ProjectToolsEntities db = new ProjectToolsEntities())
                {
                  
                    db.Client.Add(data);

                    db.SaveChanges();


                }
                return Json(new { messaggio = $"Client {data.IDClient} add with success", flag = true });

            }
            else
            {
                return Json(new { messaggio = $"Dati mancanti o non validi", flag = false });
            }
        }

        public ActionResult ClientProject(int IDClient)
        {
           
            List<Project> projectclient = null;
           

            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                projectclient = db.Project.Where(l => l.IDClient == IDClient).ToList();
               

                  
                }
          
            return PartialView(projectclient);
        }

        public ActionResult EditClient(int IDClient)
        {
            Client client = null;
            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                client = db.Client.Where(l => l.IDClient == IDClient).FirstOrDefault();

            }
            return PartialView(client);
            
            
        }

        public ActionResult DoEditClient(Client data)
        {
            Client client = null;
            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                client = db.Client.Where(l => l.IDClient == data.IDClient).FirstOrDefault();
                client.Name = data.Name;
                client.Society = data.Society;
                client.NumberPhone = data.NumberPhone;
                client.Email = data.Email;
                client.PartitaIva = data.PartitaIva;
                db.SaveChanges();

            }
            return RedirectToAction("ClientSection");

        }



        public ActionResult DeleteClient(int IDClient)
        {
            Client client = null;
            List<ActiveProject> activeProject = new List<ActiveProject>();
            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                client = db.Client.Where(l => l.IDClient == IDClient).FirstOrDefault();
                activeProject = db.ActiveProject.Where(l => l.IDProject == client.IDClient).ToList();
                if (activeProject.Count == 0)
                {
                    TempData["msg"] = "<script>alert('This client have got some project active!');</script>";
                }
            }
            return PartialView(client);
        }

        public ActionResult DoDeleteClient(int IDClient)
        {
            Client client = null;

            using (ProjectToolsEntities db = new ProjectToolsEntities())
            {
                client = db.Client.Where(l => l.IDClient == IDClient).FirstOrDefault();

                db.Client.Remove(client);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}