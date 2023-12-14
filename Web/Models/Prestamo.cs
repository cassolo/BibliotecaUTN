using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Web.Models
{
    [Table("Prestamo")]

    public class Prestamo
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Display(Name = "Fecha Prestamo")]
        [Column(TypeName = "smalldatetime")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage ="Ingresar fecha de prestamo")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? FechaPrestamo { get; set; }


        [Display(Name = "Fecha de devolucion esperada")]
        [Column(TypeName = "smalldatetime")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Ingresar fecha de devolucion")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? FechaDevolucionEsperada { get; set; }

        [Display(Name ="Libro")]
        [Required(ErrorMessage ="Seleccionar libro")]
        public int? LibroRefId { get; set; }
        [ForeignKey("LibroRefId")]
        public virtual Libro? Libro {  get; set; }

        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "Seleccionar usuario")]
        public int? UsuarioRefId { get; set; }
        [ForeignKey("UsuarioRefId")]
        public virtual Usuario? Usuario { get; set; }


    }
}
