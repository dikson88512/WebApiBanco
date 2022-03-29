using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WebApiBanco.Data.Entidade;

namespace WebApiBanco.Data.Repositorio
{
    public class CuentaRepositorio: RepositorioGenerico, ICuentaRepositorio
    {
        public IBaseDatos _bd;

        public CuentaRepositorio(IBaseDatos bd)
        {
            _bd = bd;
        }
        public async Task<List<Cuenta>> ListarCuentas()
        {
            string sqlQuery = $@"sp_ListaCuentas";
            return await _bd.ListarData<Cuenta>(sqlQuery);

        }

        public async Task<ResultadoOutput> CrearCuenta(Cuenta cuenta)
        {
            string sp = "sp_CrearCuenta";


            List<SqlParameter> parametros = new List<SqlParameter>();

            // Parámetros INPUT  
           
            parametros.Add(CrearParamInt("@clienteId", cuenta.clienteId));
            parametros.Add(CrearParamString("@numeroCuenta", cuenta.numeroCuenta));
            parametros.Add(CrearParamString("@tipoCuenta", cuenta.tipoCuenta));
            parametros.Add(CrearParamDec("@saldoInicial", cuenta.saldoInicial));
            parametros.Add(CrearParamInt("@estadoCuenta", cuenta.estadoCuenta));

            return await _bd.EjecutarSpConVariablesOutput(sp, parametros);

        }

        public async Task<List<ListadoCuentas>> ConsultarCuenta(string id)
        {
            string sqlQuery = $@"sp_ConsultarCuenta '{id}'";
            return await _bd.ListarData<ListadoCuentas>(sqlQuery);

        }

        public async Task<ResultadoOutput> ActualizarCuenta(Cuenta cuenta)
        {
            string sp = "sp_ActualizarCuenta";


            List<SqlParameter> parametros = new List<SqlParameter>();

            // Parámetros INPUT  
            parametros.Add(CrearParamString("@numeroCuenta", cuenta.numeroCuenta));
            parametros.Add(CrearParamString("@tipoCuenta", cuenta.tipoCuenta));
            parametros.Add(CrearParamDec("@saldoInicial", cuenta.saldoInicial));
            parametros.Add(CrearParamInt("@estadoCuenta", cuenta.estadoCuenta));
            

            return await _bd.EjecutarSpConVariablesOutput(sp, parametros);

        }

        public async Task<ResultadoOutput> EliminarCuenta(string id)
        {
            string sp = "sp_EliminarCuenta";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(CrearParamString("@numeroCuenta", id));
            return await _bd.EjecutarSpConVariablesOutput(sp, parametros);

        }


    }
}
