using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiBanco.Data.Entidade
{
    public class ReporteEstadoCuenta
    {

        public DateTime Fecha { get; set; }
        public string Cliente { get; set; }
        public string numeroCuenta { get; set; }
        public  string Tipo { get; set; }
        public decimal SaldoInicial { get; set; }
		public int Estado { get; set; }
		public string Movimiento{ get; set; }
		public decimal SaldoDisponible { get; set; }



    }
}
