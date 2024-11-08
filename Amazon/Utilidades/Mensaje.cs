using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Utilidades
{
    internal class Mensaje
    {
        public string texto { get; set; }
        public ConsoleColor color { get; set; }

        public Mensaje(string texto, ConsoleColor color)
        {
            this.texto = texto;
            this.color = color;
        }
    }
}
