using System;
using System.Collections.Generic;
using System.Text;
using Dominio.Interfaces;

namespace Dominio
{
    public class Investigador : IValidable
    {
        #region Propiedades
        private string _email;
        private string _password;
        private string _nombre;
        private Rol? _rol;

        public string Email { get { return _email; } }
        public string Password { get { return _password; } }
        public string Nombre { get { return _nombre; } }
        public Rol? Rol { get { return _rol; } }

        #endregion

        #region Constructor
        public Investigador(string email, string password, string nombre, Rol rol)
        {
            _email = email;
            _password = password;
            _nombre = nombre;
            _rol = rol;
        }
        #endregion

        #region Métodos
        public void Validar()
        {
            if (string.IsNullOrEmpty(_nombre) || ValidarDigitos(_nombre)) throw new Exception("El nombre no puede ser vacio y no debe conentener números");
            if (string.IsNullOrEmpty(_email) || !ValidarArroba(_email)) throw new Exception("El email no puede ser vacio");
            if (string.IsNullOrEmpty(_password)) throw new Exception("La constraseña no puede ser vacia");
            if (_rol == null) throw new Exception("El rol no puede ser vacio");
        }

        public bool ValidarDigitos(string texto)
        {
            foreach (char caracter in texto)
            {
                if (!char.IsDigit(caracter)) return false;
            }
            return true;
        }

        public bool ValidarArroba(string texto)
        {
            foreach (char caracter in texto)
            {
                if (caracter == '@') return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"Nombre: {_email}; Nombre: {_nombre}; [Rol: {_rol.ToString()}]";
        }

        public override bool Equals(object? obj)
        {
            return obj is Investigador unInvestigador && _email == unInvestigador.Email;
        }

        #endregion

    }
}
