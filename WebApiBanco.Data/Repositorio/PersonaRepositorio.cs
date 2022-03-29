using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using WebApiBanco.Data.Entidade;

namespace WebApiBanco.Data.Repositorio
{
    public class PersonaRepositorio: RepositorioGenerico, IPersonaRepositorio
    {
        public IBaseDatos _bd;

        public PersonaRepositorio(IBaseDatos bd)
        {
            _bd = bd;
        }

        public async Task<List<Persona>> ListarPersonas()
        {
            string sqlQuery = $@"sp_ListaPersonas";
            return await _bd.ListarData<Persona>(sqlQuery);

        }
        public async Task<List<Persona>> ConsultarPersona(string id)
        {
            string sqlQuery = $@"sp_ConsultarPersona '{id}'";
            return await _bd.ListarData<Persona>(sqlQuery);

        }

        public async Task<ResultadoOutput> EliminarPersona(string id)
        {
            string sp = "sp_EliminarPersona";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(CrearParamString("@documentoIdentificacion", id));
            return await _bd.EjecutarSpConVariablesOutput(sp, parametros);

        }

        public async Task<ResultadoOutput> ActualizarPersona(Persona persona)
        {
            string sp = "sp_ActualizarPersona";


            List<SqlParameter> parametros = new List<SqlParameter>();

            // Parámetros INPUT  
            parametros.Add(CrearParamString("@documentoIdentificacion", persona.documentoIdentificacion));
            parametros.Add(CrearParamString("@nombreCompleto", persona.nombreCompleto));
            parametros.Add(CrearParamString("@genero", persona.genero));
            parametros.Add(new SqlParameter("@edad", persona.edad));
            parametros.Add(CrearParamString("@direccion", persona.direccion));
            parametros.Add(CrearParamString("@telefono", persona.telefono));
                      

            return await _bd.EjecutarSpConVariablesOutput(sp, parametros);

        }


        public async Task<ResultadoOutput> CrearPersona(Persona persona)
        {
            string sp = "sp_CrearPersona";


            List<SqlParameter> parametros = new List<SqlParameter>();

            // Parámetros INPUT  
            parametros.Add(CrearParamString("@documentoIdentificacion", persona.documentoIdentificacion));
            parametros.Add(CrearParamString("@nombreCompleto", persona.nombreCompleto));
            parametros.Add(CrearParamString("@genero", persona.genero));
            parametros.Add(new SqlParameter("@edad", persona.edad));
            parametros.Add(CrearParamString("@direccion", persona.direccion));
            parametros.Add(CrearParamString("@telefono", persona.telefono));
            
            return await _bd.EjecutarSpConVariablesOutput(sp, parametros);

        }

    }
}
