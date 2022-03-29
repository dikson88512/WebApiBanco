using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApiBanco.Data.Entidade;

namespace WebApiBanco.Data.Repositorio
{
    public interface IMovimientoRepositorio
    {
        Task<List<Movimiento>> ListarMovimientos();
        Task<ResultadoOutput> CrearMovimiento(Movimiento movimiento);
        Task<List<ListadoMovimientos>> ConsultarMovimiento(string id);
        Task<ResultadoOutput> ActualizarMovimiento(int id, Movimiento movimiento);
        Task<ResultadoOutput> EliminarMovimiento(string id);
    }
}
