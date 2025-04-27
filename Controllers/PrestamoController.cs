using BiblioAPI.Models;
using BiblioAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace BiblioAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrestamoController : Controller
    {
        private readonly PrestamoService _prestamoService;

        public PrestamoController(PrestamoService prestamoService)
        {
            _prestamoService = prestamoService;
        }

        [HttpGet]
        public async Task<ActionResult<List<PrestamoModel>>> ObtenerPrestamos()
        {
            var prestamos = await _prestamoService.ObtenerPrestamosAsync();
            return Ok(prestamos);
        }

        [HttpPost]
        public async Task<ActionResult> RegistrarPrestamo([FromBody] PrestamoModel prestamo)
        {
            await _prestamoService.RegistrarPrestamoAsync(prestamo);
            return CreatedAtAction(nameof(ObtenerPrestamos), new { id = prestamo.Id }, prestamo);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> ActualizarPrestamo(int id, [FromBody] PrestamoModel prestamo)
        {
            var actualizado = await _prestamoService.ActualizarPrestamoAsync(id, prestamo);
            if (!actualizado)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> EliminarPrestamo(int id)
        {
            var eliminado = await _prestamoService.EliminarPrestamoAsync(id);
            if (!eliminado)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
