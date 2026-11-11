using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using Dominio.Interfaces;

namespace Dominio
{
    public class Caso : IValidable
    {
        #region Propiedades
        private static int s_ultimoId = 1;
        private int _id;
        private string _nombre;
        private string _descripcion;
        private bool? _estaCerrado;
        private Sospechoso _principalSospechoso;
        private Investigador _investigador;
        private List<Evidencia> _evidencias = new List<Evidencia>();

        public int Id { get { return _id; } }
        public List<Evidencia> Evidencias { get { return _evidencias; } }
        public Investigador Investigador { get {  return _investigador; }  }
        #endregion

        #region Constructor
        public Caso(string nombre, string descripcion, bool? estaCerrado, Sospechoso principalSospechoso, Investigador investigador)
        {
            _id = s_ultimoId++;
            _nombre = nombre;
            _descripcion = descripcion;
            _estaCerrado = estaCerrado;
            _principalSospechoso = principalSospechoso;
            _investigador = investigador;
        }

        public string Descripcion { get { return _descripcion; } }
        public bool? EstaCerrado { get { return _estaCerrado; } }
        public string Nombre { get { return _nombre; } }
        public Sospechoso PrincipalSospechoso { get { return _principalSospechoso; } }
        #endregion

        #region Métodos
        public void Validar()
        {
            if (string.IsNullOrEmpty(_nombre) || ValidarDigitos(_nombre)) throw new Exception("El nombre no puede ser vacio y no debe conentener números");
            if (string.IsNullOrEmpty(_descripcion)) throw new Exception("La descripción no puede ser vacia");
            if (_estaCerrado == null) throw new Exception("Esta cerrado no puede ser vacio");
            if (_investigador.Rol == Rol.FISCAL) throw new Exception("El investigador tiene no puede ser FISCAL, tiene que ser DETECTIVE");
        }

        public bool ValidarDigitos(string texto)
        {
            foreach (char caracter in texto)
            {
                if (!char.IsDigit(caracter)) return false;
            }
            return true;
        }

        public void AltaEvidenciaFisica(DateTime fechaColecta, string descripcion, bool tieneHuella)
        {
            Fisica unaEvidenciaFisica = new Fisica(fechaColecta, descripcion, tieneHuella);
            unaEvidenciaFisica.Validar();
            _evidencias.Add(unaEvidenciaFisica);
        }

        public void AltaEvidenciaGrabacion(DateTime fechaColecta, string descripcion, int calidad, bool capturaInfraganti)
        {
            Grabacion unaEvidenciaGrabacion = new Grabacion(fechaColecta, descripcion, calidad, capturaInfraganti);
            unaEvidenciaGrabacion.Validar();
            _evidencias.Add(unaEvidenciaGrabacion);
        }

        public void AltaEvidenciaTestimonio(DateTime fechaColecta, string descripcion, string nombre, Credibilidad credibilidad)
        {
            Testimonio unaEvidenciaTestimonio = new Testimonio(fechaColecta, descripcion, nombre, credibilidad);
            unaEvidenciaTestimonio.Validar();
            _evidencias.Add(unaEvidenciaTestimonio);
        }

        public int CalcularPuntajeTotal()
        {
            if (_evidencias == null) throw new Exception("El caso no tiene evidencias");
            int puntaje = 0;

            foreach (Evidencia unaEvidencia in _evidencias)
            {
                puntaje += unaEvidencia.CalcularPuntaje();
            }

            if (_principalSospechoso.TieneAntecedentes) puntaje += 10;

            if (puntaje > 100) puntaje = 100;

            return puntaje;
        }

        public override string ToString()
        {
            string retorno = $"Caso Id: {_id}; Nombre: {_nombre}; ";

            if (_evidencias.Count > 0) 
            {
                foreach(Evidencia unaEvidencia in _evidencias) 
                {
                    retorno += $"\n -> {unaEvidencia.ToString()}";
                }
            }
            else
            {
                retorno += "\n -> No se han ingresado evidencias";
            }
            return retorno;
        }
        #endregion
    }
}
