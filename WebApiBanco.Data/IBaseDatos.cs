using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiBanco.Data
{
    public interface IBaseDatos
    {
        Task<Resultado> ActualizarData(string SQLQuery);
        Task<Resultado> ActualizarData(string SQLQuery, List<SqlParameter> parameters);
        Task<Resultado> ActualizarData(List<SqlCommand> commands);
        Task<T> ConsultarEntidad<T>(string SQLQuery);
        Task<List<T>> ListarData<T>(string SQLQuery);
        Task<string> RespuestaActualizacion(string SQLQuery);
        Task<Boolean> EJecutaTarea(string SQLQuery);

        Task<ResultadoOutput> EjecutarSpConVariablesOutput(string SQLQuery, List<SqlParameter> parameters);
    }
}
