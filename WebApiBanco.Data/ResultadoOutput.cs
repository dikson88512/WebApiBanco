using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiBanco
{
    public class ResultadoOutput
    {
        public string Mensaje_control { get; set; }
        /// <summary>
        /// Indica si existió un error en la ejecución en la base de datos. Puede ser 1 (Proceso OK) o 2 (Error)
        /// </summary>
        public int Error { get; set; }
        
    }
}
