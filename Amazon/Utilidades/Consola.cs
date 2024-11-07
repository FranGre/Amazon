using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Utilidades
{
    internal class Consola
    {
        public static void Escribir(string mensaje, ConsoleColor colorTexto)
        {
            Console.ForegroundColor = colorTexto;
            Console.WriteLine(mensaje);
            Console.ResetColor();
        }

        public static void EscribirError(string mensaje)
        {
            Escribir(mensaje, ConsoleColor.Red);
            Console.ReadKey();
        }

        public static void EscribirExito(string mensaje)
        {
            Escribir(mensaje, ConsoleColor.Green);
            Console.ReadKey();
        }
    }
}
