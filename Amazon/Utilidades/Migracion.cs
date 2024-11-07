using Amazon.Contexto;
using Amazon.Modelos;
using Aspose.Cells;
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
        private AppDbContexto db;

        public Migracion()
        {
            db = new AppDbContexto();
        }

        public void MigrarDatos(string rutaCsv)
        {
            if (!File.Exists(rutaCsv))
            {
                Consola.EscribirError($"no existe {rutaCsv}");
                return;
            }

            // Cargamos el csv usando Aspose.Cells
            Workbook workbook = new Workbook(rutaCsv);

            // Establecemos nuestra hoja de trabajo para leer el csv uisado Aspose.Cells
            Worksheet worksheet = workbook.Worksheets.First();

            for (int i = 1; i <= worksheet.Cells.MaxDataRow; i++)
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
                    Consola.EscribirError($"cliente {nombre} {apellidos} {email} NO TIENE ID");
                    return;
                }


                // NOMBRE
                if (string.IsNullOrEmpty(nombre))
                {
                    Consola.EscribirError($"cliente {id} NO TIENE NOMBRE");
                    return;
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
                if (!string.IsNullOrEmpty(apellidos))
                {
                    apellidos = apellidos.Trim();

                    if (apellidos.Count() > 40)
                    {
                        apellidos = apellidos.Substring(0, 40);
                    }
                }


                // EMAIL
                if (string.IsNullOrEmpty(email))
                {
                    Consola.EscribirError($"cliente {id} {nombre} NO TIENE EMAIL");
                    return;
                }

                email = email.Trim();

                if (!Regex.IsMatch(email, EMAIL_REGEX))
                {
                    Consola.EscribirError($"cliente {id} {nombre} FORMATO EMAIL INCORRECTO");
                    return;
                }
                // Comprobar si el email es unique


                // FECHA NACIMIENTO
                if (string.IsNullOrEmpty(fechaNacimientoString))
                {
                    Consola.EscribirError($"cliente {id} {nombre} NO TIENE FECHA DE NACIMIENTO");
                    return;
                }

                fechaNacimientoString = fechaNacimientoString.Trim();
                var fechaNacimiento = DateTime.ParseExact(fechaNacimientoString, "yyyy-MM-dd", null);

                if (fechaNacimiento > DateTime.Now)
                {
                    Consola.EscribirError($"cliente {id} {nombre} NO PUEDE TENER LA FECHA NACIMIENTO SUPERIOR AL DIA DE HOY");
                    return;
                }

                if (fechaNacimiento < new DateTime(1920, 01, 01))
                {
                    Consola.EscribirError($"cliente {id} {nombre} NO PUEDE TENER LA FECHA NACIMIENTO INFERIOR A 1920");
                    return;
                }


                // TELEFONO
                if (string.IsNullOrEmpty(telefono))
                {
                    Consola.EscribirError($"cliente {id} {nombre} NO TIENE TELEFONO");
                    return;
                }

                telefono = telefono.Trim();

                if (!Regex.IsMatch(telefono, TELEFONO_REGEX))
                {
                    Consola.EscribirError($"cliente {id} {nombre} EL TELEFONO CONTIENE LETRAS");
                    return;
                }

                if (telefono.Count() > 9)
                {
                    telefono = telefono.Substring(0, 9);
                }

                // comprobar tlf es unique

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
                }
                catch (Exception ex)
                {
                    Consola.EscribirError($"cliente {cliente.Id} {cliente.PrimerNombre} {cliente.Email} {ex.ToString()}");
                    Console.ReadKey();
                }

                Consola.EscribirExito("Clientes validados obtenidos :)");
            }
        }
    }
}
