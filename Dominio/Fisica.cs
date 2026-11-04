using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public class Fisica : Evidencia
    {
        #region Propiedades
        private bool? _tieneHuella;
        #endregion

        #region Constructor
        public Fisica(DateTime fechaColecta, string descripcion, bool tieneHuella) : base(fechaColecta, descripcion)
        {
            _tieneHuella = tieneHuella;
        }
        #endregion

        #region Métodos
        public override void Validar()
        {
            base.Validar();
            if (_tieneHuella == null) throw new Exception("La huella no puede estar vacia");
        }

        public override int CalcularPuntaje()
        {
            int puntaje = base.CalcularPuntaje();
            if (_tieneHuella == true) puntaje += 40;
            return puntaje;
        }

        public override string ToString()
        {
            /*return $"{base.ToString()}; Tiene huella: {(_tieneHuella == true ? "Si" : "No")}";*/
            return $"{base.ToString()} - Física";
        }

        #endregion
    }
}
