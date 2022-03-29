using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WebApiBanco.Data.Entidade;

namespace WebApiBanco.Data.Repositorio
{
    public class MovimientoRepositorio : RepositorioGenerico, IMovimientoRepositorio
    {

        public IBaseDatos _bd;

        public MovimientoRepositorio(IBaseDatos bd)
        {
            _bd = bd;
        }
        public async Task<ResultadoOutput> ActualizarMovimiento(int id, Movimiento movimiento)
        {
            string sp = "sp_ActualizarMovimiento";


            List<SqlParameter> parametros = new List<SqlParameter>();

            // Parámetros INPUT  
            parametros.Add(CrearParamInt("@idMovimiento", id));
            parametros.Add(CrearParamDatetime("@fechaMovimiento", movimiento.fechaMovimiento));
            parametros.Add(CrearParamString("@numeroCuenta", movimiento.numeroCuenta));
            parametros.Add(CrearParamString("@tipoMovimiento", movimiento.tipoMovimiento));
            parametros.Add(CrearParamDec("@saldoInicial", movimiento.saldoInicial));
            parametros.Add(CrearParamDec("@valorMovimiento", movimiento.valorMovimiento));
            parametros.Add(CrearParamString("@descripcionMovimiento", movimiento.descripcionMovimiento));


            return await _bd.EjecutarSpConVariablesOutput(sp, parametros);

        }


        public async Task<List<ListadoMovimientos>> ConsultarMovimiento(string id)
        {
            string sqlQuery = $@"sp_ConsultarMovimiento '{id}'";
            return await _bd.ListarData<ListadoMovimientos>(sqlQuery);
        }

        public async Task<ResultadoOutput> CrearMovimiento(Movimiento movimiento)
        {
            string sp = "sp_CrearMovimiento";


            List<SqlParameter> parametros = new List<SqlParameter>();

            // Parámetros INPUT  
            
            parametros.Add(CrearParamDatetime("@fechaMovimiento", movimiento.fechaMovimiento));
            parametros.Add(CrearParamString("@numeroCuenta", movimiento.numeroCuenta));
            parametros.Add(CrearParamString("@tipoMovimiento", movimiento.tipoMovimiento));
            parametros.Add(CrearParamDec("@saldoInicial", movimiento.saldoInicial));
            parametros.Add(CrearParamDec("@valorMovimiento", movimiento.valorMovimiento));
            parametros.Add(CrearParamString("@descripcionMovimiento", movimiento.descripcionMovimiento));

            return await _bd.EjecutarSpConVariablesOutput(sp, parametros);

        }

        public async Task<ResultadoOutput> EliminarMovimiento(string id)
        {
            string sp = "sp_EliminarMovimiento";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(CrearParamString("@idMovimiento", id));
            return await _bd.EjecutarSpConVariablesOutput(sp, parametros);
        }

        public async Task<List<Movimiento>> ListarMovimientos()
        {
            string sqlQuery = $@"sp_ListaMovimientos";
            return await _bd.ListarData<Movimiento>(sqlQuery);
        }
    }
}
