using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models
{
    [Table("Libro")]
    public class Libro
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Display(Name = "Título")]
        [StringLength(50)]
        public string? Titulo { get; set; }


        [Display(Name = "Imagen")]
        public string? ImagenPortada { get; set; }


        [Display(Name = "Género")]
        [StringLength(50)]
        public string? Genero { get; set; }

        [Display(Name = "Autor")]
        [Required(ErrorMessage ="Ingresar Autor")]
        public int? AutorRefId { get; set; }
        [ForeignKey("AutorRefId")]
        public virtual Autor? Autor { get; set; }



        [Display(Name = "Fecha Publicación")]
        [Column(TypeName = "smalldatetime")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? FechaPublicacion { get; set; }


    }
}
