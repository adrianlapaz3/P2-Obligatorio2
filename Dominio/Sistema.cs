using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public class Sistema
    {
        #region Singleton
        //SINGLETON 1: Hacer privado el constructor
        public Sistema()
        {
            PrecargaInvestigadores();
            PrecargaSospechosos();
            PrecargaCasos();
            PrecargaEvidencias();
        }

        //SINGLETON 2: Crear un atributo estatico de tipo Sistema donde guardo la referencia al unico objeto Sistema que voy a permitir en mi aplicación
        private static Sistema s_instancia;

        //SINGLETON 3: Crear una property para crear y obtener el unico objeto Sistema de mi aplicacion
        public static Sistema Instancia
        {
            get
            {
                if (s_instancia == null) s_instancia = new Sistema();
                return s_instancia;
            }
        }


        #endregion

        #region Propiedades
        private List<Sospechoso> _sospechosos = new List<Sospechoso>();
        private List<Investigador> _investigadores = new List<Investigador>();
        private List<Caso> _casos = new List<Caso>();

        public List<Sospechoso> Sospechosos { get { return _sospechosos; } }
        public List<Investigador> Investigadores { get { return _investigadores; } }
        public List<Caso> Casos { get { return _casos; } }

        #endregion

        #region Métodos
        public void AltaInvestigador(Investigador unInvestigador)
        {
            if (unInvestigador == null) throw new Exception("El investigador no puede ser vacio");
            unInvestigador.Validar();
            if (_investigadores.Contains(unInvestigador)) throw new Exception("El investiagador ya existe");
            _investigadores.Add(unInvestigador);
        }

        public void AltaSospechoso(Sospechoso unSospechoso)
        {
            if (unSospechoso == null) throw new Exception("El sospechoso no puede ser vacio");
            unSospechoso.Validar();
            if (_sospechosos.Contains(unSospechoso)) throw new Exception("El sospechosos ya existe");
            _sospechosos.Add(unSospechoso);
        }
        public void AltaCaso(Caso unCaso)
        {
            if (unCaso == null) throw new Exception("El caso no puede ser vacio");
            unCaso.Validar();
            _casos.Add(unCaso);
        }

        public void AgregarEvidenciaFisicaAUnCaso(int unCasoId, DateTime fechaColecta, string descripcion, bool tieneHuella)
        {
            Caso unCaso = BuscarCaso(unCasoId);
            if (unCaso == null) throw new Exception("No existe el caso");
            unCaso.AltaEvidenciaFisica(fechaColecta, descripcion, tieneHuella);
        }

        public void AgregarEvidenciaGrabacionAUnCaso(int unCasoId, DateTime fechaColecta, string descripcion, int calidad, bool capturaInfraganti)
        {
            Caso unCaso = BuscarCaso(unCasoId);
            if (unCaso == null) throw new Exception("No existe el caso");
            unCaso.AltaEvidenciaGrabacion(fechaColecta, descripcion, calidad, capturaInfraganti);
        }

        public void AgregarEvidenciaTestimonioAUnCaso(int unCasoId, DateTime fechaColecta, string descripcion, string nombre, Credibilidad credibilidad)
        {
            Caso unCaso = BuscarCaso(unCasoId);
            if (unCaso == null) throw new Exception("No existe el caso");
            unCaso.AltaEvidenciaTestimonio(fechaColecta, descripcion, nombre, credibilidad);
        }

        public Caso BuscarCaso(int? unCasoId)
        {
            if (unCasoId == null) throw new Exception("El id del caso no puede ser vacio");
            foreach (Caso unCaso in _casos)
            {
                if (unCaso.Id == unCasoId) return unCaso;
            }
            return null;
        }

        public string RealizarRecomendacion(int? unCasoId)
        {
            Caso unCaso = BuscarCaso(unCasoId);
            if (unCaso == null) throw new Exception("No existe el caso");
            string recomendacion;

            if (unCaso.CalcularPuntajeTotal() < 30)
            {
                recomendacion = $"El caso {unCaso.Nombre} carece de fundamentos. El peso de la prueba es {unCaso.CalcularPuntajeTotal()} y debería ser desestimado finalizando la investigación en contra de {unCaso.PrincipalSospechoso.Nombre}";
            }
            else if (unCaso.CalcularPuntajeTotal() >= 30 && unCaso.CalcularPuntajeTotal() <= 70)
            {
                recomendacion = $"CONTINUAR INVESTIGANDO: El caso “{unCaso.Nombre}” parece ir por el camino correcto, pero necesita más investigación. El peso de la prueba es {unCaso.CalcularPuntajeTotal()} pero aún no hay información suficiente para imputar a {unCaso.PrincipalSospechoso.Nombre}";
            }
            else if (unCaso.CalcularPuntajeTotal() > 30)
            {
                recomendacion = $"IMPUTACIÓN INMINENTE: El caso “{unCaso.Nombre}” está resuelto. El peso de la prueba es {unCaso.CalcularPuntajeTotal()} y debería imputarse a {unCaso.PrincipalSospechoso.Nombre} inmediatamente";
            }
            else
            {
                throw new Exception("El caso no tiene puntaje porque carece de evidencia");
            }
            return recomendacion;
        }

        public Sospechoso BuscarSospechosoPorCedula(string cedula)
        {
            if (string.IsNullOrEmpty(cedula)) throw new Exception("Cédula no puede ser vacia");
            else
            {
                foreach (Sospechoso unSospechoso in _sospechosos)
                {
                    if (unSospechoso.Cedula == cedula) return unSospechoso;
                }
            }
            return null;
        }

        public List<Sospechoso> ListarSospechososConAntecedentes()
        {
            List<Sospechoso> listaDeSospechosos = new List<Sospechoso>();
            foreach (Sospechoso unSospechoso in _sospechosos)
            {
                if (unSospechoso.TieneAntecedentes == true) listaDeSospechosos.Add(unSospechoso);
            }
            return listaDeSospechosos;
        }

        public Investigador BuscarInvestigador(string email)
        {
            if (string.IsNullOrEmpty(email)) throw new Exception("Email no puede ser vacio");
            else
            {
                foreach (Investigador unInvestigador in _investigadores)
                {
                    if (unInvestigador.Email == email) return unInvestigador;
                }
            }
            return null;
        }

        public bool ValidarSiExisteEmail(string email)
        {
            foreach (Investigador unInvestigador in _investigadores)
            {
                if (unInvestigador.Email == email) return true;
            }
            return false;
        }


        public List<Caso> BuscarCasosPorInvestigador(string email)
        {
            if (!ValidarSiExisteEmail(email)) throw new Exception("Ese email no se asocia a ningún investigador");

            Investigador unInvestigador = BuscarInvestigador(email);

            List<Caso> casosEncontrados = new List<Caso>();
            foreach (Caso unCaso in _casos)
            {
                if (unCaso.Investigador.Equals(unInvestigador))
                {
                    casosEncontrados.Add(unCaso);
                }
            }
            if (casosEncontrados.Count == 0) throw new Exception("El investigador carece de casos");
            return casosEncontrados;

        }

        public Investigador Login(string email, string password)
        {
            foreach (Investigador unInvestigador in _investigadores)
            {
                if (unInvestigador.Email == email && unInvestigador.Password == password) return unInvestigador;
            }

            throw new Exception("Email o contraseña incorrecta");
        }
        

        #endregion

        #region Precargas

        public void PrecargaInvestigadores()
        {
            // 5 investigadores con rol DETECTIVE
            AltaInvestigador(new Investigador("jose.perez@fiscalia.com.uy", "1234", "Jose Perez", Rol.DETECTIVE));
            AltaInvestigador(new Investigador("ana.rodriguez@mdn.gub.uy", "1234", "Ana Rodriguez", Rol.DETECTIVE));
            AltaInvestigador(new Investigador("martin.silva@fiscalia.com.uy", "1234", "Martin Silva", Rol.DETECTIVE));
            AltaInvestigador(new Investigador("lucia.fernandez@mdn.gub.uy", "1234", "Lucia Fernandez", Rol.DETECTIVE));
            AltaInvestigador(new Investigador("diego.pereira@fiscalia.com.uy", "1234", "Diego Pereira", Rol.DETECTIVE));

            // 5 investigadores con rol FISCAL
            AltaInvestigador(new Investigador("sofia.gonzalez@fiscalia.com.uy", "1234", "Sofia Gonzalez", Rol.FISCAL));
            AltaInvestigador(new Investigador("andres.martinez@fiscalia.com.uy", "1234", "Andres Martinez", Rol.FISCAL));
            AltaInvestigador(new Investigador("valentina.lopez@fiscalia.com.uy", "1234", "Valentina Lopez", Rol.FISCAL));
            AltaInvestigador(new Investigador("nicolas.castro@fiscalia.com.uy", "1234", "Nicolas Castro", Rol.FISCAL));
            AltaInvestigador(new Investigador("camila.suarez@fiscalia.com.uy", "1234", "Camila Suarez", Rol.FISCAL));
        }
        public void PrecargaSospechosos()
        {
            AltaSospechoso(new Sospechoso("Juan Carrasco", "23456783", new DateTime(2000, 5, 21), true));
            AltaSospechoso(new Sospechoso("Pedro Ramirez", "27654307", new DateTime(1995, 8, 12), false));
            AltaSospechoso(new Sospechoso("Carlos Morales", "31245681", new DateTime(1988, 3, 15), true));
            AltaSospechoso(new Sospechoso("Luis Fernandez", "34567872", new DateTime(1992, 11, 7), false));
            AltaSospechoso(new Sospechoso("Mario Gonzalez", "38765430", new DateTime(1985, 6, 20), true));
            AltaSospechoso(new Sospechoso("Roberto Silva", "41234529", new DateTime(1998, 9, 25), false));
            AltaSospechoso(new Sospechoso("Daniel Pereira", "44567886", new DateTime(1990, 2, 18), true));
            AltaSospechoso(new Sospechoso("Fernando Castro", "47891272", new DateTime(2001, 4, 10), false));
            AltaSospechoso(new Sospechoso("Miguel Lopez", "42345654", new DateTime(1987, 12, 3), true));
            AltaSospechoso(new Sospechoso("Ricardo Suarez", "45678911", new DateTime(1994, 7, 14), false));
            AltaSospechoso(new Sospechoso("Andres Medina", "25781241", new DateTime(1996, 1, 30), true));
            AltaSospechoso(new Sospechoso("Sebastian Torres", "32468151", new DateTime(1989, 10, 5), false));
            AltaSospechoso(new Sospechoso("Javier Rodriguez", "39876559", new DateTime(1999, 3, 22), true));
            AltaSospechoso(new Sospechoso("Hector Martinez", "46543206", new DateTime(1991, 6, 8), false));
            AltaSospechoso(new Sospechoso("Oscar Alvarez", "48765450", new DateTime(1986, 11, 19), true));
        }

        public void PrecargaCasos()
        {
            // Solo se asignan investigadores con rol DETECTIVE.
            // Los detectives ocupan los indices 0 al 4.
            AltaCaso(new Caso("Caso 1", "Asalto en agencia de pagos", false,
                _sospechosos[0], _investigadores[0]));

            AltaCaso(new Caso("Caso 2", "Robo de vehiculo en Montevideo", false,
                _sospechosos[1], _investigadores[1]));

            AltaCaso(new Caso("Caso 3", "Fraude bancario mediante transferencias", false,
                _sospechosos[3], _investigadores[2]));

            AltaCaso(new Caso("Caso 4", "Hurto en comercio del centro", false,
                _sospechosos[4], _investigadores[3]));

            AltaCaso(new Caso("Caso 5", "Estafa mediante venta de productos", false,
                _sospechosos[6], _investigadores[4]));

            AltaCaso(new Caso("Caso 6", "Robo en vivienda particular", false,
                _sospechosos[8], _investigadores[0]));

            AltaCaso(new Caso("Caso 7", "Falsificacion de documentos", false,
                _sospechosos[9], _investigadores[1]));

            AltaCaso(new Caso("Caso 8", "Asalto a estacion de servicio", false,
                _sospechosos[11], _investigadores[2]));

            AltaCaso(new Caso("Caso 9", "Hurto de equipos informaticos", false,
                _sospechosos[13], _investigadores[3]));

            AltaCaso(new Caso("Caso 10", "Fraude mediante identidad digital", false,
                _sospechosos[14], _investigadores[4]));
        }

        public void PrecargaEvidencias()
        {
            // CASO 1 - Febrero 2026
            AgregarEvidenciaFisicaAUnCaso(1, new DateTime(2026, 2, 21), "Huellas en vidrio del local", true);
            AgregarEvidenciaFisicaAUnCaso(1, new DateTime(2026, 2, 22), "Herramienta encontrada en el local", false);
            AgregarEvidenciaGrabacionAUnCaso(1, new DateTime(2026, 2, 23), "Video de camara de seguridad", 4, true);
            AgregarEvidenciaTestimonioAUnCaso(1, new DateTime(2026, 2, 24), "Declaracion del cajero", "Pablo Gomez", Credibilidad.ALTO);

            // CASO 2 - Marzo 2026
            AgregarEvidenciaFisicaAUnCaso(2, new DateTime(2026, 3, 3), "Huellas en puerta del vehiculo", true);
            AgregarEvidenciaFisicaAUnCaso(2, new DateTime(2026, 3, 4), "Fragmento de vidrio encontrado", false);
            AgregarEvidenciaGrabacionAUnCaso(2, new DateTime(2026, 3, 5), "Camara de estacionamiento", 3, false);
            AgregarEvidenciaTestimonioAUnCaso(2, new DateTime(2026, 3, 6), "Declaracion de vecino", "Laura Perez", Credibilidad.MEDIO);

            // CASO 3 - Marzo 2026
            AgregarEvidenciaFisicaAUnCaso(3, new DateTime(2026, 3, 12), "Documentos bancarios impresos", false);
            AgregarEvidenciaFisicaAUnCaso(3, new DateTime(2026, 3, 13), "Computadora incautada", true);
            AgregarEvidenciaGrabacionAUnCaso(3, new DateTime(2026, 3, 14), "Grabacion de cajero automatico", 5, true);
            AgregarEvidenciaTestimonioAUnCaso(3, new DateTime(2026, 3, 15), "Declaracion de empleado bancario", "Santiago Diaz", Credibilidad.ALTO);

            // CASO 4 - Abril 2026
            AgregarEvidenciaFisicaAUnCaso(4, new DateTime(2026, 4, 15), "Huellas en mostrador", true);
            AgregarEvidenciaFisicaAUnCaso(4, new DateTime(2026, 4, 16), "Bolso abandonado", false);
            AgregarEvidenciaGrabacionAUnCaso(4, new DateTime(2026, 4, 17), "Video del comercio", 2, true);
            AgregarEvidenciaTestimonioAUnCaso(4, new DateTime(2026, 4, 18), "Declaracion de cliente", "Mariana Costa", Credibilidad.BAJO);

            // CASO 5 - Mayo 2026
            AgregarEvidenciaFisicaAUnCaso(5, new DateTime(2026, 5, 10), "Contrato de compraventa", false);
            AgregarEvidenciaFisicaAUnCaso(5, new DateTime(2026, 5, 11), "Telefono celular incautado", true);
            AgregarEvidenciaGrabacionAUnCaso(5, new DateTime(2026, 5, 12), "Grabacion de reunion comercial", 4, false);
            AgregarEvidenciaTestimonioAUnCaso(5, new DateTime(2026, 5, 13), "Declaracion de comprador", "Federico Ruiz", Credibilidad.ALTO);

            // CASO 6 - Junio 2026
            AgregarEvidenciaFisicaAUnCaso(6, new DateTime(2026, 6, 8), "Huellas en ventana", true);
            AgregarEvidenciaFisicaAUnCaso(6, new DateTime(2026, 6, 9), "Guante encontrado en vivienda", false);
            AgregarEvidenciaGrabacionAUnCaso(6, new DateTime(2026, 6, 10), "Video de camara vecinal", 3, true);
            AgregarEvidenciaTestimonioAUnCaso(6, new DateTime(2026, 6, 11), "Declaracion del propietario", "Gabriela Ramos", Credibilidad.MEDIO);

            // CASO 7 - Julio 2026
            AgregarEvidenciaFisicaAUnCaso(7, new DateTime(2026, 7, 5), "Documento de identidad falsificado", false);
            AgregarEvidenciaFisicaAUnCaso(7, new DateTime(2026, 7, 6), "Impresora utilizada para falsificaciones", true);
            AgregarEvidenciaGrabacionAUnCaso(7, new DateTime(2026, 7, 7), "Video de ingreso a oficina", 5, false);
            AgregarEvidenciaTestimonioAUnCaso(7, new DateTime(2026, 7, 8), "Declaracion de funcionario", "Rodrigo Molina", Credibilidad.ALTO);

            // CASO 8 - Agosto 2026
            AgregarEvidenciaFisicaAUnCaso(8, new DateTime(2026, 8, 12), "Huellas en caja registradora", true);
            AgregarEvidenciaFisicaAUnCaso(8, new DateTime(2026, 8, 13), "Prenda encontrada en el lugar", false);
            AgregarEvidenciaGrabacionAUnCaso(8, new DateTime(2026, 8, 14), "Camara de estacion de servicio", 4, true);
            AgregarEvidenciaTestimonioAUnCaso(8, new DateTime(2026, 8, 15), "Declaracion del encargado", "Natalia Romero", Credibilidad.MEDIO);

            // CASO 9 - Septiembre 2026
            AgregarEvidenciaFisicaAUnCaso(9, new DateTime(2026, 9, 9), "Huellas en escritorio", true);
            AgregarEvidenciaFisicaAUnCaso(9, new DateTime(2026, 9, 10), "Cable de seguridad cortado", false);
            AgregarEvidenciaGrabacionAUnCaso(9, new DateTime(2026, 9, 11), "Video de ingreso al edificio", 2, false);
            AgregarEvidenciaTestimonioAUnCaso(9, new DateTime(2026, 9, 12), "Declaracion de guardia", "Esteban Vidal", Credibilidad.BAJO);

            // CASO 10 - Octubre 2026
            AgregarEvidenciaFisicaAUnCaso(10, new DateTime(2026, 10, 7), "Dispositivo de almacenamiento incautado", false);
            AgregarEvidenciaFisicaAUnCaso(10, new DateTime(2026, 10, 8), "Telefono con huellas digitales", true);
            AgregarEvidenciaGrabacionAUnCaso(10, new DateTime(2026, 10, 9), "Grabacion de acceso a terminal", 5, true);
            AgregarEvidenciaTestimonioAUnCaso(10, new DateTime(2026, 10, 10), "Declaracion de especialista informatico", "Alejandro Soto", Credibilidad.ALTO);
        }

        #endregion
    }
}
