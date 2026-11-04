using System;
using System.Collections.Generic;
using System.Text;
using Dominio.Interfaces;

namespace Dominio
{
    public class Sospechoso : IValidable
    {
        #region Propiedades

        private string _nombre;
        private string _cedula;
        private DateTime _fechaNacimiento;
        private bool _tieneAntecedentes;

        public string Cedula { get { return _cedula; } }
        public string Nombre { get { return _nombre; } }
        public bool TieneAntecedentes { get { return _tieneAntecedentes; } }

        #endregion


        #region Constructor
        public Sospechoso(string nombre, string cedula, DateTime fechaNacimiento, bool tieneAntecedentes)
        {
            _nombre = nombre;
            _cedula = cedula;
            _fechaNacimiento = fechaNacimiento;
            _tieneAntecedentes = tieneAntecedentes;
        }
        #endregion


        #region Métodos

        public void Validar()
        {
            if (string.IsNullOrEmpty(_nombre) || ValidarDigitos(_nombre)) throw new Exception("El nombre no puede ser vacio y no debe conentener números");
            if (string.IsNullOrEmpty(_cedula) || !ValidarDigitos(_cedula) || _cedula.Length != 8) throw new Exception("La cédula no puede ser vacía, solo debe contener números y con un largo de 8 números");
        }

        public bool ValidarDigitos(string texto)
        {
            foreach (char caracter in texto)
            {
                if (!char.IsDigit(caracter)) return false;
            }
            return true;
        }

        public override string ToString()
        {
            return $"Nombre: {_nombre}; Cédula: {_cedula}; Fecha de nacimiento: {_fechaNacimiento.ToShortDateString()}; Antecedentes: {(_tieneAntecedentes ? "Si" : "No")}";
        }

        public override bool Equals(object? obj)
        {
            return obj is Sospechoso unSospechoso && _cedula == unSospechoso.Cedula;
        }

        #endregion

    }
}
