using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Modelos
{
    internal class Cliente
    {
        public int Id { get; set; }

        [Required, MaxLength(25)]
        public string PrimerNombre { get; set; }

        [MaxLength(25)]
        public string SegundoNombre { get; set; }

        [Required, MaxLength(40)]
        public string Apellidos { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, Phone]
        public string Telefono { get; set; }

        [Required]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        public DateTime? FechaModificacion { get; set; }
    }
}
