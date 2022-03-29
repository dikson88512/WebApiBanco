using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApiBanco.Data.Entidade;

namespace WebApiBanco.Data.Repositorio
{
    public interface ICuentaRepositorio
    {
        Task<List<Cuenta>> ListarCuentas();
        Task<ResultadoOutput> CrearCuenta(Cuenta cliente);
        Task<List<ListadoCuentas>> ConsultarCuenta(string id);
        Task<ResultadoOutput> ActualizarCuenta(Cuenta cliente);
        Task<ResultadoOutput> EliminarCuenta(string id);
    }
}
