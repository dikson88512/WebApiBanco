using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiBanco
{
    public class Resultado
    {
        public string mensaje_control { get; set; }
        public int error { get; set; }
        public string respuesta_1 { get; set; }
        public string respuesta_2 { get; set; }
    }
}
