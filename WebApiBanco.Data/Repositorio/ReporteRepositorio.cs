using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApiBanco.Data.Entidade;

namespace WebApiBanco.Data.Repositorio
{
    public class ReporteRepositorio : RepositorioGenerico, IReporteRepositorio
    {
        public IBaseDatos _bd;

        public ReporteRepositorio(IBaseDatos bd)
        {
            _bd = bd;
        }
        public async Task<List<ReporteEstadoCuenta>> ListaEstadoCuenta(string documentoIdentificacion, string FechaDesde, string FechaHasta)
        {
            string fechaIni = Convert.ToDateTime(FechaDesde).ToString("yyyyMMdd");
            string fechaFin = Convert.ToDateTime(FechaHasta).ToString("yyyyMMdd");

            string sqlQuery = $@"sp_ListaEstadoCuenta '{documentoIdentificacion}', '{fechaIni}', '{fechaFin}'";
            return await _bd.ListarData<ReporteEstadoCuenta>(sqlQuery);
        }
    }
   
}
