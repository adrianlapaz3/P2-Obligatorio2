using Dominio;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class SospechososController : Controller
    {
        Sistema miSistema = Sistema.Instancia;

        public IActionResult Index(int? edad)
        {
            if (HttpContext.Session.GetString("rol") == null || HttpContext.Session.GetString("rol") != "DETECTIVE") return View("NoAuth");
            ViewBag.Listado = miSistema.ListarSopechososPorEdad(edad);
            return View();
        }
    }
}
