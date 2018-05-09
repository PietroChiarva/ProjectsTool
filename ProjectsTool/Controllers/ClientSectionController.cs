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
    }
}