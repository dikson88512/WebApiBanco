using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WebApiBanco.Data.Entidade;

namespace WebApiBanco.Data.Repositorio
{
    public class ClienteRepositorio: RepositorioGenerico, IClienteRepositorio
    {
        public IBaseDatos _bd;

        public ClienteRepositorio(IBaseDatos bd)
        {
            _bd = bd;
        }
        public async Task<List<Cliente>> ListarClientes()
        {
            string sqlQuery = $@"sp_ListaClientes";
            return await _bd.ListarData<Cliente>(sqlQuery);

        }

        public async Task<ResultadoOutput> CrearCliente(Cliente cliente)
        {
            string sp = "sp_CrearCliente";


            List<SqlParameter> parametros = new List<SqlParameter>();

            // Parámetros INPUT  
            parametros.Add(CrearParamString("@documentoIdentificacion", cliente.documentoIdentificacion));
            parametros.Add(CrearParamString("@nombreCompleto", cliente.nombreCompleto));
            parametros.Add(CrearParamString("@genero", cliente.genero));
            parametros.Add(CrearParamInt("@edad", cliente.edad));
            parametros.Add(CrearParamString("@direccion", cliente.direccion));
            parametros.Add(CrearParamString("@telefono", cliente.telefono));


            parametros.Add(CrearParamDec("@clienteId", cliente.clienteId));
            parametros.Add(CrearParamString("@contrasena", cliente.contrasena));
            parametros.Add(CrearParamInt("@estado", cliente.estado));

            return await _bd.EjecutarSpConVariablesOutput(sp, parametros);

        }

        public async Task<List<Cliente>> ConsultarCliente(string id)
        {
            string sqlQuery = $@"sp_ConsultarCliente '{id}'";
            return await _bd.ListarData<Cliente>(sqlQuery);

        }

        public async Task<ResultadoOutput> ActualizarCliente(Cliente cliente)
        {
            string sp = "sp_ActualizarCliente";


            List<SqlParameter> parametros = new List<SqlParameter>();

            // Parámetros INPUT  
            parametros.Add(CrearParamString("@documentoIdentificacion", cliente.documentoIdentificacion));
            parametros.Add(CrearParamString("@nombreCompleto", cliente.nombreCompleto));
            parametros.Add(CrearParamString("@genero", cliente.genero));
            parametros.Add(CrearParamInt("@edad", cliente.edad));
            parametros.Add(CrearParamString("@direccion", cliente.direccion));
            parametros.Add(CrearParamString("@telefono", cliente.telefono));


           
            parametros.Add(CrearParamString("@contrasena", cliente.contrasena));
            parametros.Add(CrearParamInt("@estado", cliente.estado));

            return await _bd.EjecutarSpConVariablesOutput(sp, parametros);

        }

        public async Task<ResultadoOutput> EliminarCliente(string id)
        {
            string sp = "sp_EliminarCliente";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(CrearParamString("@documentoIdentificacion", id));
            return await _bd.EjecutarSpConVariablesOutput(sp, parametros);

        }
    }
}
