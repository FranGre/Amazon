using Amazon.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var db = new AppDbContexto();
            db.Clientes.ToList();
        }
    }
}
