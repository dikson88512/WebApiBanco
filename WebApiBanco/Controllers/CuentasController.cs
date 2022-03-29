using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WebApiBanco.Data.Entidade;
using WebApiBanco.Data.Repositorio;

namespace WebApiBanco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentasController : ControllerBase
    {

        private readonly ILogger<CuentasController> _logger;

        ICuentaRepositorio _CuentaRepositorio;

        public CuentasController(ILogger<CuentasController> logger, ICuentaRepositorio CuentaRepositorio)
        {

            _CuentaRepositorio = CuentaRepositorio;
        }

        #region CRUD

        [HttpPost]
        public async Task<ResultadoOutput> CrearCuenta(Cuenta cuenta)
        {
            try
            {
                var resultado = await _CuentaRepositorio.CrearCuenta(cuenta);
                return resultado;
            }
            catch (Exception ex)
            {

                var error = new ResultadoOutput();
                error.Mensaje_control = ex.Message;
                return error;
            }


        }
        [HttpGet] //https://localhost:44305/api/Cuenta
        public async Task<IActionResult> ListarCuentas()
        {
            try
            {
                var resultado = await _CuentaRepositorio.ListarCuentas();
                return new JsonResult(resultado);

            }
            catch (Exception ex)
            {

                return new JsonResult(new
                {
                    error = ex.Message
                });
            }

        }

        [HttpGet("{id}")]  //https://localhost:44305/api/Cuentas/7
        public async Task<IActionResult> consultarCuentas(string id)
        {
            try
            {
                var resultado = await _CuentaRepositorio.ConsultarCuenta(id);
                return new JsonResult(resultado);

            }
            catch (Exception ex)
            {

                return new JsonResult(new
                {
                    error = ex.Message
                });
            }

        }

        [HttpPut()]  //https://localhost:44305/api/Cuentas
        public async Task<ResultadoOutput> actualizarCuenta(Cuenta cuenta)
        {

            try
            {
                var resultado = await _CuentaRepositorio.ActualizarCuenta(cuenta);
                return resultado;
            }
            catch (Exception ex)
            {
                var error = new ResultadoOutput();
                error.Mensaje_control = ex.Message;
                return error;

            }

        }

        [HttpDelete("{id}")]  //https://localhost:44305/api/Cuenta/1
        public async Task<ResultadoOutput> eliminarCuenta(string id)
        {
            try
            {
                var resultado = await _CuentaRepositorio.EliminarCuenta(id);
                return resultado;


            }
            catch (Exception ex)
            {

                var error = new ResultadoOutput();
                error.Mensaje_control = ex.Message;
                return error;
            }

        }
        #endregion

    }
}
