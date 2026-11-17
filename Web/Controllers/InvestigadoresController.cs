using Microsoft.AspNetCore.Mvc;
using Dominio;

namespace Web.Controllers
{
    public class InvestigadoresController : Controller
    {
        Sistema miSistema = Sistema.Instancia;


        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            try
            {
                if (HttpContext.Session.GetString("rol") != null) return View("NoAuth");
                if (string.IsNullOrEmpty(email)) throw new Exception("Debe ingresar email");
                if (string.IsNullOrEmpty(password)) throw new Exception("Debe ingresar una contraseña");
                Investigador unInvestigador = miSistema.Login(email, password);
                HttpContext.Session.SetString("email", email);
                HttpContext.Session.SetString("rol", unInvestigador.Rol.ToString());
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }

        }

        [HttpGet]
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("rol") == null) return View("NoAuth");
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Perfil()
        {
            if (HttpContext.Session.GetString("rol") == null) return View("NoAuth");
            ViewBag.Investigador = miSistema.BuscarInvestigador(HttpContext.Session.GetString("email"));
            return View();
        }


        [HttpGet]
        public IActionResult Registrarse()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Registrarse(string email, string password, string nombre)
        {
            try
            {
                if (string.IsNullOrEmpty(email)) throw new Exception("Debe ingresar email");
                if (string.IsNullOrEmpty(password)) throw new Exception("Debe ingresar una contraseña");
                if (string.IsNullOrEmpty(nombre)) throw new Exception("Debe ingresar un nombre");
                miSistema.AltaInvestigador(new Investigador(email, password, nombre, Rol.DETECTIVE));
                Investigador unInvestigador = miSistema.Login(email, password);
                HttpContext.Session.SetString("email", email);
                HttpContext.Session.SetString("rol", unInvestigador.Rol.ToString());
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
            return View();
        }

    }
}
