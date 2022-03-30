using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApiBanco.Data.Entidade;

namespace WebApiBanco.Data.Repositorio
{
    public interface IReporteRepositorio
    {
        Task<List<ReporteEstadoCuenta>> ListaEstadoCuenta(string documentoIdentificacion, string FechaDesde, string FechaHasta); 
    }
}
