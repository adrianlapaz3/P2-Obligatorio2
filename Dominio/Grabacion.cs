using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public class Grabacion : Evidencia
    {
        #region Propiedades
        private int? _calidad;
        private bool? _capturaInfraganti;
        #endregion

        #region Constructor
        public Grabacion(DateTime fechaColecta, string descripcion, int calidad, bool capturaInfraganti) : base(fechaColecta, descripcion)
        {
            _calidad = calidad;
            _capturaInfraganti = capturaInfraganti;
        }
        #endregion

        #region Métodos
        public override void Validar()
        {
            base.Validar();
            if (_calidad == null || _calidad < 1 || _calidad > 5) throw new Exception("La calidad debe ser un número entero del 1 al 5");
            if (_capturaInfraganti == null) throw new Exception("La captura infraganti no puede estar vacia");
        }

        public override int CalcularPuntaje()
        {
            int puntaje = base.CalcularPuntaje();
            if (_capturaInfraganti == true) puntaje += 60;
            if (_calidad >= 3) puntaje += 10;
            return puntaje;
        }

        public override string ToString()
        {
            /*return $"{base.ToString()}; Calidad: {_calidad}; Captura infraganti: {(_capturaInfraganti == true ? "Si" : "No")}";*/
            return $"{base.ToString()} - Grabación";
        }
        #endregion
    }
}
