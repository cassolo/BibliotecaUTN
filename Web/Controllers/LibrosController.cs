using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web.Models;
using Web.Repos;
using Web.ViewModels;

namespace Web.Controllers
{
    public class LibrosController : Controller
    {
        private readonly BibliotecaUTNContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LibrosController(BibliotecaUTNContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Libros
        public async Task<IActionResult> Index()
        {
            var bibliotecaUTNContext = _context.Libros.Include(l => l.Autor);
            return View(await bibliotecaUTNContext.ToListAsync());
        }

        // GET: Libros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .Include(l => l.Autor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // GET: Libros/Create
        public IActionResult Create()
        {
            ViewData["AutorRefId"] = new SelectList(_context.Autors, "Id", "Nombre");
            return View();
        }

        // POST: Libros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LibroViewModel model)
        {
            string uniqueFileName = UploadedFile(model);
            if (ModelState.IsValid)
            {
                Libro libro = new Libro()
                {
                    ImagenPortada = uniqueFileName,
                    Titulo = model.Titulo,
                    Genero = model.Genero,
                    AutorRefId = model.AutorRefId,
                    FechaPublicacion = model.FechaPublicacion
                };

                _context.Add(libro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AutorRefId"] = new SelectList(_context.Autors, "Id", "Nombre", model.AutorRefId);
            return View(model);
        }

        // GET: Libros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros.FindAsync(id);
            LibroViewModel libroViewModel = new LibroViewModel()
            {
                Titulo = libro.Titulo,
                Genero = libro.Genero,
                Autor = libro.Autor,
                FechaPublicacion =libro.FechaPublicacion
            };

            if (libro == null)
            {
                return NotFound();
            }
            ViewData["AutorRefId"] = new SelectList(_context.Autors, "Id", "Nombre", libro.AutorRefId);
            return View(libroViewModel);
        }

        // POST: Libros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LibroViewModel model)
        {
            string uniqueFileName = UploadedFile(model);
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var libro = await _context.Libros.FindAsync(id);

                    libro.ImagenPortada = uniqueFileName;
                    libro.Autor = model.Autor;
                    libro.Titulo = model.Titulo;
                    libro.Genero = model.Genero;
                    libro.FechaPublicacion = model.FechaPublicacion;

                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibroExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AutorRefId"] = new SelectList(_context.Autors, "Id", "Nombre", model.AutorRefId);
            return View(model);
        }

        // GET: Libros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .Include(l => l.Autor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // POST: Libros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro != null)
            {
                _context.Libros.Remove(libro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private string UploadedFile(LibroViewModel model)
        {
            string uniqueFileName = null;

            if (model.Imagen != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Imagen.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Imagen.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        private bool LibroExists(int id)
        {
            return _context.Libros.Any(e => e.Id == id);
        }
    }
}
