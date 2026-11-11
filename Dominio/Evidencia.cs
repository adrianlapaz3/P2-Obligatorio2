using System;
using System.Collections.Generic;
using System.Text;
using Dominio.Interfaces;

namespace Dominio
{
    public abstract class Evidencia : IValidable
    {
        #region Propiedades

        private static int s_ultimoId = 1;
        protected int _id;
        protected DateTime? _fechaColecta;
        protected string _descripcion;

        public int Id{get { return _id; }}
        public string Descripcion { get { return _descripcion; } }
        public DateTime? FechaColecta { get { return _fechaColecta; } }

        #endregion


        #region Constructor
        protected Evidencia(DateTime fechaColecta, string descripcion)
        {
            _id = s_ultimoId++;
            _fechaColecta = fechaColecta;
            _descripcion = descripcion;
        }
        #endregion

        #region Métodos
        public virtual void Validar()
        {
            if (_fechaColecta == null || _fechaColecta > DateTime.Now) throw new Exception("Fecha vacia o fecha de colecta es mayor a la fecha de Registro");
            if (string.IsNullOrEmpty(_descripcion)) throw new Exception("La descripción no puede ser vacia");
        }

        public virtual int CalcularPuntaje()
        {
            int puntaje = 3;
            DateTime fechaActual = DateTime.Now;
            if (_fechaColecta > fechaActual.AddMonths(-6)) puntaje = 5;
            return puntaje;
        }

        public abstract string TipoEvidencia();

        public override string ToString()
        {
            return $"Id: {_id}; Descripción: {_descripcion}";
        }

        #endregion

    }
}
