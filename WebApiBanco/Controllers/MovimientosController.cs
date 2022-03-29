using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WebApiBanco.Data.Entidade;
using WebApiBanco.Data.Repositorio;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiBanco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientosController : ControllerBase
    {
        private readonly ILogger<CuentasController> _logger;

        IMovimientoRepositorio _MovimientoRepositorio;

        public MovimientosController(ILogger<CuentasController> logger, IMovimientoRepositorio MovimientoRepositorio)
        {
            _MovimientoRepositorio = MovimientoRepositorio;
        }

        #region CRUD
        // GET: api/<MovimientosController>
        [HttpGet] //https://localhost:44305/api/Movimientos
        public async Task<IActionResult> ListarMovimientos()
        {
            try
            {
                var resultado = await _MovimientoRepositorio.ListarMovimientos();
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
        public async Task<IActionResult> consultarMovimientos(string id)
        {
            try
            {
                var resultado = await _MovimientoRepositorio.ConsultarMovimiento(id);
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

        // POST api/<MovimientosController>
        [HttpPost]
        public async Task<ResultadoOutput> CrearMovimiento(Movimiento movimiento)
        {
            try
            {
                var resultado = await _MovimientoRepositorio.CrearMovimiento(movimiento);
                return resultado;
            }
            catch (Exception ex)
            {

                var error = new ResultadoOutput();
                error.Mensaje_control = ex.Message;
                return error;
            }


        }

        // PUT api/<MovimientosController>/5
        [HttpPut("{id}")]  //https://localhost:44305/api/Movimientos
        public async Task<ResultadoOutput> actualizarMovimiento(int id, [FromBody] Movimiento cuenta)
        {

            try
            {
                var resultado = await _MovimientoRepositorio.ActualizarMovimiento(id,cuenta);
                return resultado;
            }
            catch (Exception ex)
            {
                var error = new ResultadoOutput();
                error.Mensaje_control = ex.Message;
                return error;

            }

        }

        // DELETE api/<MovimientosController>/5
        [HttpDelete("{id}")]  //https://localhost:44305/api/Movimientos/1
        public async Task<ResultadoOutput> eliminarCuenta(string id)
        {
            try
            {
                var resultado = await _MovimientoRepositorio.EliminarMovimiento(id);
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
