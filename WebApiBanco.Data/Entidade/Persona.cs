using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiBanco.Data.Entidade
{
    public class Persona
    {
        public string documentoIdentificacion { get; set; }

        public string nombreCompleto { get; set; }

        public string genero { get; set; }

        public int edad { get; set; }

        public string direccion { get; set; }
        public string telefono { get; set; }
    }
}
