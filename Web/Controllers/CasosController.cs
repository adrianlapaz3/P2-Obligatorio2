using Dominio;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class CasosController : Controller
    {
        Sistema miSistema = Sistema.Instancia;

        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("rol") == null) return View("NoAuth");
            if (TempData["Exito"] != null) ViewBag.Exito = TempData["Exito"];
            if (HttpContext.Session.GetString("rol") == "DETECTIVE") 
            {
                ViewBag.Listado = miSistema.BuscarCasosPorInvestigador(HttpContext.Session.GetString("email"));
                ViewBag.MiRol = HttpContext.Session.GetString("rol");
                /*BuscarCasosPorInvestigador(email)*/
            }
            else if (HttpContext.Session.GetString("rol") == "FISCAL")
            {
                ViewBag.Listado = miSistema.Casos;
                ViewBag.MiRol = HttpContext.Session.GetString("rol");
            }

            return View();
        }

        public IActionResult Detalle(int? id)
        {
            if (HttpContext.Session.GetString("rol") == null) return View("NoAuth");

            if (id != null)
            {
                ViewBag.Caso = miSistema.BuscarCaso(id);
                ViewBag.Recomendacion = miSistema.RealizarRecomendacion(id);
            }
            return View();
        }

    }

}
