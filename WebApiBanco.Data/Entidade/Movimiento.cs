using System;

namespace WebApiBanco.Data.Entidade
{
    public class Movimiento
    {
        public int idMovimiento { get; set; }
        public int idCuenta { get; set; }
        public DateTime fechaMovimiento { get; set; }
        public string numeroCuenta { get; set; }
		public string tipoMovimiento { get; set; }
		public decimal saldoInicial { get; set; }
        public decimal valorMovimiento{ get; set; }
        public decimal saldoMovimiento { get; set; }
        public string descripcionMovimiento { get; set; }
    }
    public class ListadoMovimientos
    {
        public int idMovimiento { get; set; }
        public int idCuenta { get; set; }
        public DateTime fechaMovimiento { get; set; }
        public string numeroCuenta { get; set; }
        public string tipoMovimiento { get; set; }
        public decimal saldoInicial { get; set; }
        public decimal valorMovimiento { get; set; }
        public decimal saldoMovimiento { get; set; }
        public string descripcionMovimiento { get; set; }



    }
}
