using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiBanco.Data.Entidade;
using WebApiBanco.Data.Repositorio;

namespace WebApiBanco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {

        private readonly ILogger<ClientesController> _logger;

        IClienteRepositorio _ClienteRepositorio;

        public ClientesController(ILogger<ClientesController> logger, IClienteRepositorio ClienteRepositorio)
        {

            _ClienteRepositorio = ClienteRepositorio;
        }

        #region CRUD

        [HttpPost]
        public async Task<ResultadoOutput> CrearCliente(Cliente persona)
        {
            try
            {
                var resultado = await _ClienteRepositorio.CrearCliente(persona);
                return resultado;
            }
            catch (Exception ex)
            {

                var error = new ResultadoOutput();
                error.Mensaje_control = ex.Message;
                return error;
            }


        }

        [HttpGet] //https://localhost:44305/api/Clientes
        public async Task<IActionResult> ListarClientes()
        {
            try
            {
                var resultado = await _ClienteRepositorio.ListarClientes();
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
                

        [HttpGet("{id}")]  //https://localhost:44305/api/Clientes/7
        public async Task<IActionResult> consultarCliente(string id)
        {
            try
            {
                var resultado = await _ClienteRepositorio.ConsultarCliente(id);
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


        [HttpPut()]  //https://localhost:44305/api/Clientes
        public async Task<ResultadoOutput> actualizarCliente(Cliente persona)
        {

            try
            {
                var resultado = await _ClienteRepositorio.ActualizarCliente(persona);
                return resultado;
            }
            catch (Exception ex)
            {
                var error = new ResultadoOutput();
                error.Mensaje_control = ex.Message;
                return error;

            }

        }

        [HttpDelete("{id}")]  //https://localhost:44305/api/Cliente/1757415649
        public async Task<ResultadoOutput> eliminarCliente(string id)
        {
            try
            {
                var resultado = await _ClienteRepositorio.EliminarCliente(id);
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
