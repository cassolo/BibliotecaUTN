using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }


        [Display(Name = "Nombre")]
        [Required(ErrorMessage ="Ingresar nombre")]
        public string? Nombre { get; set; }

        [Display(Name = "Correo Electrónico")]
        [Required(ErrorMessage ="Ingresar correo")]
        public string? CorreoElectronico { get; set; }

        [Display(Name = "Telefono de contacto")]
        [Required(ErrorMessage ="Ingresar telefono")]
        public string? Telefono { get; set; }

    }
}

