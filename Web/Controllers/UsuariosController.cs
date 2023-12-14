using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web.Models;
using Web.Repos;
using OfficeOpenXml;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using Microsoft.AspNetCore.Hosting;


namespace Web.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly BibliotecaUTNContext _context;
        private readonly IWebHostEnvironment _env;

        public UsuariosController(BibliotecaUTNContext context, IWebHostEnvironment env) 
        {
            _context = context;
            _env = env;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,CorreoElectronico,Telefono")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,CorreoElectronico,Telefono")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
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
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        
        [HttpPost, ActionName("MostrarDatos")]
        public IActionResult MostrarDatos([FromForm] IFormFile ArchivoExcel)
        {
            if (ArchivoExcel != null)
            {
                Stream stream = ArchivoExcel.OpenReadStream();

                IWorkbook MiExcel = null;

                if (Path.GetExtension(ArchivoExcel.FileName) == ".xlsx")
                {
                    MiExcel = new XSSFWorkbook(stream);
                }
                else
                {
                    MiExcel = new HSSFWorkbook(stream);
                }

                ISheet HojaExcel = MiExcel.GetSheetAt(0);

                int cantidadFilas = HojaExcel.LastRowNum;

                List<Usuario> lista = new List<Usuario>();

                for (int i = 1; i <= cantidadFilas; i++)
                {

                    IRow fila = HojaExcel.GetRow(i);


                    lista.Add(new Usuario
                    {
                        Nombre = fila.GetCell(0).ToString(),
                        CorreoElectronico = fila.GetCell(1).ToString(),
                        Telefono = fila.GetCell(2).ToString()

                    });
                }

                return StatusCode(StatusCodes.Status200OK, lista);
            }
            else
            {

                return View();
            }

        }


        [HttpPost, ActionName("EnviarDatos")]
        public IActionResult EnviarDatos([FromForm] IFormFile ArchivoExcel)
        {
            if (ArchivoExcel != null)
            {
                Stream stream = ArchivoExcel.OpenReadStream();

                IWorkbook MiExcel = null;

                if (Path.GetExtension(ArchivoExcel.FileName) == ".xlsx")
                {
                    MiExcel = new XSSFWorkbook(stream);
                }
                else
                {
                    MiExcel = new HSSFWorkbook(stream);
                }

                ISheet HojaExcel = MiExcel.GetSheetAt(0);

                int cantidadFilas = HojaExcel.LastRowNum;
                List<Usuario> lista = new List<Usuario>();

                for (int i = 1; i <= cantidadFilas; i++)
                {

                    IRow fila = HojaExcel.GetRow(i);

                    lista.Add(new Usuario
                    {
                        Nombre = fila.GetCell(0).ToString(),
                        CorreoElectronico =fila.GetCell(1).ToString(),
                        Telefono = fila.GetCell(2).ToString()

                    });
                }

                _context.Usuarios.AddRange(lista);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            else
            {
                return View();
            }

        }

        public IActionResult DownloadFile()
        {
            var filepath = Path.Combine(_env.WebRootPath, "archivos", "Usuarios.xlsx");
            return File(System.IO.File.ReadAllBytes(filepath), "application/vnd.ms-excel", System.IO.Path.GetFileName(filepath));
        }

        public IActionResult DownloadUsersAsExcel()
        {
            var users = _context.Usuarios.ToList(); 

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Usuarios");

                
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Nombre de Usuario";
                worksheet.Cells[1, 3].Value = "Correo Electrónico";
                worksheet.Cells[1, 4].Value = "Teléfono";

                
                int row = 2;
                foreach (var user in users)
                {
                    worksheet.Cells[row, 1].Value = user.Id;
                    worksheet.Cells[row, 2].Value = user.Nombre;
                    worksheet.Cells[row, 3].Value = user.CorreoElectronico;
                    worksheet.Cells[row, 4].Value = user.Telefono;
                    row++;
                }


                var fileBytes = package.GetAsByteArray();
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var fileName = "Usuarios.xlsx";

                return File(fileBytes, contentType, fileName);
            }
        }

        public IActionResult ImportarUsuarios()
        {
            return View();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
