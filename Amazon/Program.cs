using Amazon.Contexto;
using Amazon.Utilidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string raizProyecto = Directory.GetParent(Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName).FullName).FullName;

            var migracion = new Migracion();
            migracion.MigrarDatos($"{raizProyecto}/clientes.csv");
            Console.ReadKey();
        }
    }
}
