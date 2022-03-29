using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiBanco.Data.Entidade
{
    public class Cliente:Persona
    {

        public int clienteId { get; set; }
        public string contrasena { get; set; }
        public int estado { get; set; }
     
    
    }
}
