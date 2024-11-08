using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Utilidades
{
    internal class Consola
    {
        public static void EscribirLinea(Mensaje mensaje)
        {
            Console.ForegroundColor = mensaje.color;
            Console.WriteLine(mensaje.texto);
            Console.ResetColor();
        }

        public static void EscribirError(string mensaje)
        {
            EscribirLinea(new Mensaje(mensaje, ConsoleColor.Red));
        }

        public static void EscribirExito(string mensaje)
        {
            EscribirLinea(new Mensaje(mensaje, ConsoleColor.Green));
        }
    }
}
