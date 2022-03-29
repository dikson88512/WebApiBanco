using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApiBanco.Data.Entidade;

namespace WebApiBanco.Data.Repositorio
{
    public interface IClienteRepositorio
    {
        Task<List<Cliente>> ListarClientes();
        Task<ResultadoOutput> CrearCliente(Cliente cliente);
        Task<List<Cliente>> ConsultarCliente(string id);
        Task<ResultadoOutput> ActualizarCliente(Cliente cliente);
        Task<ResultadoOutput> EliminarCliente(string id);

    }
}
