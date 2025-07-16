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
            var trabajadores = await _context.ObtenerTrabajadoresFiltradosAsync(sexo);
            ViewBag.Departamentos = await _context.ObtenerDepartamentosAsync();
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

            if (!ModelState.IsValid)
            {
              
                TempData["Error"] = "Error de datos";
                return RedirectToAction(nameof(Index));
            }


            await _context.CrearTrabajadorAsync(trabajador);
            TempData["Success"] = "Trabajador registrado correctamente";
            return RedirectToAction(nameof(Index));
        }




        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await _context.EliminarTrabajadorAsync(id);
                TempData["Success"] = "Trabajador eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex)
            {
                TempData["Error"] = "Error al eliminar al trabajador";
               
                return RedirectToAction(nameof(Index)); 
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
