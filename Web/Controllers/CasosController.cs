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
            try
            {
                if (HttpContext.Session.GetString("rol") == "DETECTIVE")
                {
                    ViewBag.Listado = miSistema.BuscarCasosPorInvestigador(HttpContext.Session.GetString("email"));
                    ViewBag.MiRol = HttpContext.Session.GetString("rol");
                }
                else if (HttpContext.Session.GetString("rol") == "FISCAL")
                {
                    ViewBag.Listado = miSistema.Casos;
                    ViewBag.MiRol = HttpContext.Session.GetString("rol");
                }

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
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

        [HttpGet]
        public IActionResult AltaCaso()
        {
            if (HttpContext.Session.GetString("rol") != null || HttpContext.Session.GetString("rol") != "FISCAL") return View("NoAuth");
            ViewBag.ListadoSospechosos = miSistema.Sospechosos;
            ViewBag.ListadoInvestigadoresDetectives = miSistema.ListarInvestigadoresDetective();
            return View();
        }

        [HttpPost]
        public IActionResult AltaCaso(string nombre, string descripcion, bool estaCerrado, string cedula, string mail)
        {
            if (HttpContext.Session.GetString("rol") == null) return View("NoAuth");
            try
            {
                if (string.IsNullOrEmpty(nombre)) throw new Exception("El nombre no puede estar vacio");
                if (string.IsNullOrEmpty(descripcion)) throw new Exception("La descripcion no puede estar vacia");
                if (string.IsNullOrEmpty(cedula)) throw new Exception("Debe seleccionar un sospechoso");
                if (string.IsNullOrEmpty(mail)) throw new Exception("Debe seleccionar un investigador");
                Sospechoso unSospechoso = miSistema.BuscarSospechosoPorCedula(cedula);
                Investigador unInvestigador = miSistema.BuscarInvestigador(mail);
                Caso unCaso = new Caso(nombre, descripcion, estaCerrado, unSospechoso, unInvestigador);
                miSistema.AltaCaso(unCaso);
                ViewBag.Exito = $"El caso fue creado\nNombre: {nombre}\nDescripón: {descripcion}\nSospechoso: {unSospechoso.Nombre}(C.I.: {unSospechoso.Cedula}\nInvestigador: {unInvestigador.Nombre} (Email: {unInvestigador.Email}, Rol:{unInvestigador.Rol})";
                ViewBag.ListadoSospechosos = miSistema.Sospechosos;
                ViewBag.ListadoInvestigadoresDetectives = miSistema.ListarInvestigadoresDetective();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Nombre = nombre;
                ViewBag.Descripcion = descripcion;
                ViewBag.Cedula = cedula;
                ViewBag.Mail = mail;
                ViewBag.ListadoSospechosos = miSistema.Sospechosos;
                ViewBag.ListadoInvestigadoresDetectives = miSistema.ListarInvestigadoresDetective();
            }
            return View();
        }
    }

}
