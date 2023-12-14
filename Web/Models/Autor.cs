using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models
{
    [Table("Autor")]
    public class Autor
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [StringLength(50)]
        [Required(ErrorMessage ="Ingresar nombre")]
        public string? Nombre { get; set; }

        [Display(Name = "Nacionalidad")]
        [StringLength(50)]
        public string? Nacionalidad { get; set; }


    }
}
