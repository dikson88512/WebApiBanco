using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApiBanco.Data.Entidade;

namespace WebApiBanco.Data.Repositorio
{
    public interface IPersonaRepositorio
    {
        Task<ResultadoOutput> CrearPersona(Persona persona);
        Task<List<Persona>> ListarPersonas();
        Task<List<Persona>> ConsultarPersona(string id);
        Task<ResultadoOutput> ActualizarPersona(Persona persona);
        Task<ResultadoOutput> EliminarPersona(string id);



    }
}
