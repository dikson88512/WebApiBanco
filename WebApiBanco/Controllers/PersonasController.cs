using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiBanco.Data.Entidade;
//using System.Web.Http;
using WebApiBanco.Data.Repositorio;

namespace WebApiBanco.Controllers
{
    [Route("api/Personas")]
    [ApiController]
    public class PersonasController : ControllerBase
    {
        private readonly ILogger<PersonasController> _logger;

        IPersonaRepositorio _PersonasRepositorio;

        public PersonasController(ILogger<PersonasController> logger, IPersonaRepositorio PersonasRepositorio)
        {
            _PersonasRepositorio = PersonasRepositorio;
        }

        #region CRUD

        [HttpPost]
        public async Task<ResultadoOutput> CrearPersona(Persona persona)
        {
            try
            {
                var resultado = await _PersonasRepositorio.CrearPersona(persona);
                return resultado;
            }
            catch (Exception ex)
            {

                var error = new ResultadoOutput();
                error.Mensaje_control = ex.Message;
                return error;
            }


        }

        [HttpGet] //https://localhost:44305/api/Personas
        
        public async Task<IActionResult> ListarPersonas()
        {
            try
            {
                var resultado = await _PersonasRepositorio.ListarPersonas();
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
               

        [HttpGet("{id}")]  //https://localhost:44305/api/Personas/7
        public async Task<IActionResult> consultarPersona(string id)
        {
            try
            {
                var resultado = await _PersonasRepositorio.ConsultarPersona(id);
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


        [HttpPut()]  //https://localhost:44305/api/Personas/1757415649
       
        public async Task<ResultadoOutput> actualizarPersona(Persona persona)
        {
                           
            try
            {
                var resultado =  await _PersonasRepositorio.ActualizarPersona(persona);
                return resultado;
            }
            catch (Exception ex)
            {
                var error = new ResultadoOutput();
                error.Mensaje_control = ex.Message;
                return error;
                
            }

        }

        [HttpDelete("{id}")]  //https://localhost:44305/api/Personas/1757415649
        public async Task<ResultadoOutput> eliminarPersona(string id)
        {
            try
            {
                var resultado = await _PersonasRepositorio.EliminarPersona(id);
                return resultado;
                

            }
            catch (Exception ex)
            {

                var error = new ResultadoOutput();
                error.Mensaje_control = ex.Message;
                return error;
            }

        }
        #endregion CRUD
    }
}
