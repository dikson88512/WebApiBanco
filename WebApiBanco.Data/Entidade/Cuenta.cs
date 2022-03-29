using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiBanco.Data.Entidade
{
    public class Cuenta
    {

        public string documentoIdentificacion { get; set; }
        public string nombreCompleto { get; set; }
        public int idCuenta { get; set; }
        public int clienteId { get; set; }
        public string numeroCuenta { get; set; }
        public string tipoCuenta { get; set; }
        public decimal saldoInicial { get; set; }
        public int estadoCuenta { get; set; }
        
    }


    public class ListadoCuentas
    {

        public string documentoIdentificacion { get; set; }
        public string nombreCompleto { get; set; }
        public string numeroCuenta { get; set; }
        public string tipoCuenta { get; set; }
        public decimal saldoInicial { get; set; }
        public int estadoCuenta { get; set; }

    }
}
