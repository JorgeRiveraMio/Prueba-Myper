using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Prueba_Myper.Data;
using Prueba_Myper.Models;

namespace Prueba_Myper.Controllers
{
    public class TrabajadoresController : Controller
    {
        private readonly AppDbContext _context;
        public TrabajadoresController(AppDbContext context)
        {
            _context = context;
        }

      
        public async Task<IActionResult> Index(string sexo)
        {
            var query = "EXEC sp_ListarTrabajadores";

            // Filtrar por sexo 
            if (!string.IsNullOrEmpty(sexo))
            {
                
                sexo = sexo.ToUpper();
                if (sexo == "M" || sexo == "F")
                {
                    query += $" @sexo = '{sexo}'"; 
                }
            }

            var trabajadores = await _context.TrabajadorVista
                .FromSqlRaw(query)
                .ToListAsync();

            var departamentos = await _context.ObtenerDepartamentosAsync();
            ViewBag.Departamentos = departamentos;

            return View(trabajadores);
        }


        [HttpGet]
        public async Task<IActionResult> ObtenerProvincias(int idDepartamento)
        {
            var provincias = await _context.ObtenerProvinciasAsync(idDepartamento);
            return Json(provincias);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerDistritos(int idProvincia)
        {
            var distritos = await _context.ObtenerDistritoAsync(idProvincia);
            return Json(distritos);
        }


        [HttpPost]
        public async Task<IActionResult> Crear(Trabajador trabajador)
        {
            Console.WriteLine("ENTRÓ AL MÉTODO CREAR");
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"Campo: {error.Key}");
                    foreach (var err in error.Value.Errors)
                    {
                        Console.WriteLine($" - Error: {err.ErrorMessage}");
                    }
                }

                return RedirectToAction(nameof(Index));
            }


            await _context.CrearTrabajadorAsync(trabajador);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await _context.EliminarTrabajadorAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex)
            {
              
                    ModelState.AddModelError("", $"Error al eliminar: {ex.Message}");
                return RedirectToAction(nameof(Index)); // o mostrar una vista de error
            }
        }

        [HttpPost]
       
        public async Task<IActionResult> Actualizar(Trabajador trabajador)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Datos inválidos. Revisa el formulario.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _context.ActualizarTrabajadorAsync(trabajador);
                TempData["Success"] = "Trabajador actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex)
            {
                TempData["Error"] = $"Error al actualizar: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }





    }
}
