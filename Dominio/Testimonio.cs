using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public class Testimonio : Evidencia
    {
        #region Propiedades
        private string _nombre;
        private Credibilidad? _credibilidad;
        #endregion

        #region Constructor
        public Testimonio(DateTime fechaColecta, string descripcion, string nombre, Credibilidad credibilidad) : base(fechaColecta, descripcion)
        {
            _nombre = nombre;
            _credibilidad = credibilidad;
        }
        #endregion

        #region Métodos
        public override void Validar()
        {
            base.Validar();
            if (string.IsNullOrEmpty(_nombre) || ValidarDigitos(_nombre)) throw new Exception("El nombre no puede ser vacio y no debe conentener números");
            if (_credibilidad == null) throw new Exception("La credibilidad no debe ser nula");
        }
        public bool ValidarDigitos(string texto)
        {
            foreach (char caracter in texto)
            {
                if (!char.IsDigit(caracter)) return false;
            }
            return true;
        }

        public override int CalcularPuntaje()
        {
            int puntaje = base.CalcularPuntaje();
            if (_credibilidad == Credibilidad.MEDIO || _credibilidad == Credibilidad.ALTO) puntaje += 10;
            return puntaje;
        }

        public override string ToString()
        {
            /*return $"{base.ToString()}; Nombre: {_nombre}; [Credibilidad: {_credibilidad.ToString()}]";*/
            return $"{base.ToString()} - Testimonio";
        }
        #endregion


    }
}
