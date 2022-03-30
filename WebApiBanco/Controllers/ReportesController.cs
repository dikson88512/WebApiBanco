using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiBanco.Data.Repositorio;

namespace WebApiBanco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportesController : ControllerBase
    {

        private readonly ILogger<ReportesController> _logger;

        IReporteRepositorio _ReporteRepositorio;

        public ReportesController(ILogger<CuentasController> logger, IReporteRepositorio ReporteRepositorio)
        {
            _ReporteRepositorio = ReporteRepositorio;
        }

        [HttpGet("{id}")]  //https://localhost:44305/api/Cuentas/7
        public async Task<IActionResult> ListaEstadoCuenta(string id, string fechaDesde, string fechaHasta)
        {
            try
            {
                if (String.IsNullOrEmpty(fechaDesde) || String.IsNullOrEmpty(fechaHasta)){
                    return new JsonResult(new
                    {
                        error="Debe indicar las fechas"
                    });
                }
                
                var resultado = await _ReporteRepositorio.ListaEstadoCuenta(id, fechaDesde, fechaHasta);
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
    }
}
