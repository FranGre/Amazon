using Amazon.Contexto;
using Amazon.Modelos;
using Aspose.Cells;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Amazon.Utilidades
{
    internal class Migracion
    {
        private const int ID = 0;
        private const int NOMBRE = 1;
        private const int APELLIDO = 2;
        private const int EMAIL = 3;
        private const int FECHA_NACIMIENTO = 4;
        private const int TELEFONO = 5;
        private const string EMAIL_REGEX = @"^[\w\.-]+@[a-zA-Z\d\.-]+\.[a-zA-Z]{2,}$";
        private const string TELEFONO_REGEX = @"^\d+$";
        private const ConsoleColor COLOR_ERROR = ConsoleColor.Red;
        private const ConsoleColor COLOR_SUCCESS = ConsoleColor.Green;
        private AppDbContexto db;

        public Migracion()
        {
            db = new AppDbContexto();
        }

        public void MigrarDatos(string rutaCsv)
        {
            if (!File.Exists(rutaCsv))
            {
                Consola.EscribirLinea(new Mensaje($"No se ha encontrado el fichero {rutaCsv}", COLOR_ERROR));
                return;
            }

            // Cargamos el csv usando Aspose.Cells
            Workbook workbook = new Workbook(rutaCsv);

            // Establecemos nuestra hoja de trabajo para leer el csv uisado Aspose.Cells
            Worksheet worksheet = workbook.Worksheets.First();

            for (int i = 8; i <= worksheet.Cells.MaxDataRow; i++)
            {
                var id = worksheet.Cells[i, ID].StringValue;
                var nombre = worksheet.Cells[i, NOMBRE].StringValue;
                var apellidos = worksheet.Cells[i, APELLIDO].StringValue;
                var email = worksheet.Cells[i, EMAIL].StringValue;
                var fechaNacimientoString = worksheet.Cells[i, FECHA_NACIMIENTO].StringValue;
                var telefono = worksheet.Cells[i, TELEFONO].StringValue;


                // ID
                if (string.IsNullOrEmpty(id))
                {
                    Consola.EscribirLinea(new Mensaje($"CLIENTE {nombre} {apellidos} {email} NO GUARDADO, NO TIENE ID", COLOR_ERROR));
                    continue;
                }


                // NOMBRE
                if (string.IsNullOrEmpty(nombre))
                {
                    nombre = "-";
                }

                nombre = nombre.Trim();
                var nombresSplit = nombre.Split(' ');
                var nombres = new string[2];

                // probar quitando el if
                if (nombresSplit[0].Count() > 25)
                {
                    nombresSplit[0] = nombresSplit[0].Substring(0, 25);
                }

                if (nombresSplit.Count() == 1)
                {
                    nombres = new string[] { nombresSplit[0], null };
                }

                if (nombresSplit.Count() > 1)
                {
                    nombres = new string[] { nombresSplit[0], nombresSplit[1] };
                    if (nombresSplit[1].Count() > 25)
                    {
                        nombres[1] = nombres[1].Substring(0, 25);
                    }
                }


                // APELLIDOS
                if (string.IsNullOrEmpty(apellidos))
                {
                    apellidos = "-";
                }

                apellidos = apellidos.Trim();

                if (apellidos.Count() > 40)
                {
                    apellidos = apellidos.Substring(0, 40);
                }


                // EMAIL
                if (string.IsNullOrEmpty(email))
                {
                    Consola.EscribirLinea(new Mensaje($"CLIENTE {id} {nombres[0]} {email} NO GUARDADO, NO TIENE EMAIL", COLOR_ERROR));
                    continue;
                }

                email = email.Trim();

                if (!Regex.IsMatch(email, EMAIL_REGEX))
                {
                    Consola.EscribirLinea(new Mensaje($"CLIENTE {id} {nombres[0]} {email}  NO GUARDADO, FORMATO EMAIL INCORRECTO", COLOR_ERROR));
                    continue;
                }

                if (db.Clientes.Where(c => c.Email == email).FirstOrDefault() != null)
                {
                    Consola.EscribirLinea(new Mensaje($"CLIENTE {id} {nombres[0]} {email} NO GUARDADO, EMAIL EN USO", COLOR_ERROR));
                    continue;
                }


                // FECHA NACIMIENTO
                if (string.IsNullOrEmpty(fechaNacimientoString))
                {
                    fechaNacimientoString = DateTime.Now.ToString("yyyy-MM-dd");
                }

                fechaNacimientoString = fechaNacimientoString.Trim();
                var fechaNacimiento = DateTime.ParseExact(fechaNacimientoString, "yyyy-MM-dd", null);

                if (fechaNacimiento > DateTime.Now)
                {
                    fechaNacimiento = DateTime.ParseExact(DateTime.Now.ToString(), "yyyy-MM-dd", null);
                }

                if (fechaNacimiento < new DateTime(1920, 01, 01))
                {
                    fechaNacimiento = new DateTime(1920, 01, 01);
                }


                // TELEFONO
                if (string.IsNullOrEmpty(telefono))
                {
                    telefono = "000000000";
                }

                telefono = telefono.Trim();

                if (!Regex.IsMatch(telefono, TELEFONO_REGEX))
                {
                    Consola.EscribirLinea(new Mensaje($"CLIENTE {id} {nombres[0]} {email} NO GUARDADO, TELEFONO CONTIENE LETRAS", COLOR_ERROR));
                    continue;
                }

                if (telefono.Count() > 9)
                {
                    telefono = telefono.Substring(0, 9);
                }

                var cliente = new Cliente
                {
                    Id = Convert.ToInt32(id),
                    PrimerNombre = nombres[0],
                    SegundoNombre = nombres[1],
                    Apellidos = apellidos,
                    Email = email,
                    Telefono = telefono,
                    FechaNacimiento = fechaNacimiento,
                    FechaCreacion = DateTime.Now,
                };

                db.Clientes.Add(cliente);
                try
                {
                    db.SaveChanges();
                    Consola.EscribirLinea(new Mensaje($"CLIENTE {id} GUARDADO :)", COLOR_SUCCESS));
                }
                catch (Exception ex)
                {
                    Consola.EscribirLinea(new Mensaje($"{cliente.Id} {cliente.PrimerNombre} {cliente.Email} - {ex.ToString()}", COLOR_ERROR));
                }
            }
        }
    }
}
