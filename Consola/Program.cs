using Dominio;
using System.Drawing;
namespace Consola

{
    internal class Program
    {
        static Sistema miSistema;

        static void Main(string[] args)
        {
            miSistema = new Sistema();
            string opcion = "";
            while (opcion != "0")

            #region Menú 
            {
                MostrarMenu();
                opcion = PedirTexto("Ingrese una opcion > ");

                switch (opcion)
                {
                    case "1":
                        ListarCasos();
                        PressToContinue();
                        break;
                    case "2":
                        ListarCasosPorInvestigador();
                        PressToContinue();
                        break;
                    case "3":
                        AltaDeUnSospechoso();
                        PressToContinue();
                        break;
                    case "4":
                        MostrarSospechososConAntecedentes();
                        PressToContinue();
                        break;
                    case "0":
                        Console.WriteLine("Saliendo...");
                        break;
                    default:
                        MostrarError("ERROR: Opción inválida");
                        PressToContinue();
                        break;
                }
            }
            #endregion

            #region Funciones Auxiliares
            static void MostrarMenu()
            {
                Console.Clear();
                MostrarMensajeColor(ConsoleColor.Cyan, "****************");
                MostrarMensajeColor(ConsoleColor.Cyan, "      MENU      ");
                MostrarMensajeColor(ConsoleColor.Cyan, "****************");
                Console.WriteLine();
                Console.WriteLine("1 - Listar casos");
                Console.WriteLine("2 - Mostrar casos de un investigador");
                Console.WriteLine("3 - Dar alta a un sospechoso");
                Console.WriteLine("4 - Listar sospechosos con antecedentes");
                Console.WriteLine("0 - Salir");
            }

            static string PedirTexto(string mensaje)
            {
                Console.Write(mensaje);
                string texto = Console.ReadLine();
                return texto;
            }

            static int PedirEntero(string mensaje)
            {
                Console.Write(mensaje);
                string enteroString = Console.ReadLine();
                int enteroConvertido = 0;

                while (!int.TryParse(enteroString, out enteroConvertido))
                {
                    MostrarError("ERROR DE INGRESO");
                    Console.Write(mensaje);
                    enteroString = Console.ReadLine();
                }

                return enteroConvertido;
            }

            static DateTime LeerFecha(string mensaje)
            {
                bool exito = false;
                DateTime fecha = new DateTime();
                while (!exito)
                {
                    Console.Write(mensaje + " [DD/MM/YYYY]:");
                    exito = DateTime.TryParse(Console.ReadLine(), out fecha);

                    if (!exito) MostrarError("ERROR: Debe ingresar una fecha en formato DD/MM/YYYY");
                }
                return fecha;
            }

            static bool LeerBooleano(string mensaje)
            {
                bool exito = false;
                bool resultado = false;
                while (!exito)
                {
                    Console.Write(mensaje + " [S/N]:");
                    string booleanoString = Console.ReadLine();
                    if (booleanoString.ToUpper() == "S")
                    {
                        resultado = true;
                        exito = true;
                    }
                    else if (booleanoString.ToUpper() == "N")
                    {
                        resultado = false;
                        exito = true;
                    }

                    if (!exito) MostrarError("ERROR: Debe ingresar solo S o N");
                }

                return resultado;
            }

            static void MostrarMensajeColor(ConsoleColor color1, string mensaje)
            {
                Console.ForegroundColor = color1;
                Console.WriteLine(mensaje);
                Console.ResetColor();
            }

            static void MostrarError(string mensaje)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(mensaje);
                Console.ResetColor();
            }

            static void PressToContinue()
            {
                Console.WriteLine();
                Console.WriteLine("Presione una tecla para volver al menu");
                Console.ReadKey();
            }
            #endregion

            #region Objetivo I 
            static void ListarCasos()
            {
                if (miSistema.Casos.Count == 0)
                {
                    Console.WriteLine("El investigador carece de casos");
                }
                else
                {
                    foreach (Caso unCaso in miSistema.Casos)
                    {
                        Console.WriteLine();
                        MostrarMensajeColor(ConsoleColor.Cyan,"----------------------------------------------------------------------------");
                        Console.WriteLine(unCaso);
                        MostrarMensajeColor(ConsoleColor.Cyan, "----------------------------------------------------------------------------");
                    }
                }
            }
            #endregion

            #region Objetivo II
            static void ListarCasosPorInvestigador()
            {
                string email = PedirTexto("Ingresar el email del investigador: ");
                if (!miSistema.ValidarSiExisteEmail(email))
                {
                    MostrarError("Ese email no se asocia a ningún investigador");
                    
                }
                else
                { 
                    List<Caso> casos = miSistema.BuscarCasosPorInvestigador(email);

                    if (casos.Count == 0)
                    {
                        MostrarMensajeColor(ConsoleColor.Yellow,"El investigador carece de casos"); 
                    }
                    else
                    {
                        foreach(Caso unCaso in casos) 
                        {
                            MostrarMensajeColor(ConsoleColor.Green, $"Caso Id: {unCaso.Id}; Nombre: {unCaso.Nombre}; Investigador: {unCaso.Investigador.Nombre}");
                        }
                    }
                }
            }
            #endregion

            #region Objetivo III 
            static void AltaDeUnSospechoso()
            {
                string nombre = PedirTexto("Ingrese nombre ");
                string cedula = PedirTexto("Ingrese cédula sin puntos ni guiones y con el digito verificador ");
                DateTime fechaNacimiento = LeerFecha("Ingrese fecha de nacimiento en formato DD/MM/YYYY");
                bool tieneAntecedentes = LeerBooleano("¿Tiene antecedentes? Debe ingresar solo S o N");

                try
                {
                    if (string.IsNullOrEmpty(nombre)) throw new Exception("El nombre no puede ser vacio");
                    if (string.IsNullOrEmpty(cedula)) throw new Exception("La cédula no puede estar vacio");
                    miSistema.AltaSospechoso(new Sospechoso(nombre, cedula, fechaNacimiento, tieneAntecedentes));
                    MostrarMensajeColor(ConsoleColor.Green, $"Sospechoso ingreso\n{miSistema.BuscarSospechosoPorCedula(cedula)}");
                }
                catch (Exception ex)
                {
                    MostrarError(ex.Message);
                }
                PressToContinue();
            }
            #endregion

            #region Objetivo IV 
            static void MostrarSospechososConAntecedentes()
            {
                List<Sospechoso> sospechososConAntecedentes = miSistema.ListarSospechososConAntecedentes();
                if(sospechososConAntecedentes.Count == 0 )
                {
                    MostrarMensajeColor(ConsoleColor.Yellow, "No hay sospechosos con antecedentes");
                }
                else
                {
                    foreach (Sospechoso unSospechoso in sospechososConAntecedentes)
                    {
                        Console.WriteLine(unSospechoso);
                    }
                }
            }
            #endregion

        }
    }
}
