using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Web.Models;

namespace Web.ViewModels
{
    public class LibroViewModel
    {

        public int Id { get; set; }

        [Required(ErrorMessage ="Ingresar Titulo")]
        [Display(Name = "Título")]

        public string? Titulo { get; set; }


        [Display(Name ="Imagen portada")]
        public IFormFile Imagen {  get; set; }

        [Display(Name = "Imagen")]
        public string? ImagenPortada { get; set; }


        [Display(Name = "Género")]
        public string? Genero { get; set; }

        [Display(Name = "Autor")]
        [Required(ErrorMessage ="Ingresar autor")]
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
