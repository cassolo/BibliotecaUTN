using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Web.Models;


namespace Web.Repos;

public partial class BibliotecaUTNContext : DbContext
{
    public BibliotecaUTNContext()
    {
    }

    public BibliotecaUTNContext(DbContextOptions<BibliotecaUTNContext> options)
        : base(options)
    {
    }


    public virtual DbSet<Libro> Libros { get; set; }
    public virtual DbSet<Autor> Autors { get; set; }
    public virtual DbSet<Usuario> Usuarios { get; set; }
    public virtual DbSet<Prestamo> Prestamo { get; set; }


    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      //  => optionsBuilder.UseSqlServer("name=conexion");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
