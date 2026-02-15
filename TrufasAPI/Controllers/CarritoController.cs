using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrufasAPI.Data;
using TrufasAPI.Models;

namespace TrufasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarritoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CarritoController(AppDbContext context)
        {
            _context = context;
        }

        // Agregar producto al carrito
        [HttpPost("agregar")]
        public async Task<IActionResult> AgregarProducto(int usuarioId, int productoId, int cantidad)
        {
            var carrito = await _context.Carritos
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId && c.Activo);

            if (carrito == null)
            {
                carrito = new Carrito
                {
                    UsuarioId = usuarioId,
                    FechaCreacion = DateTime.Now,
                    Activo = true
                };

                _context.Carritos.Add(carrito);
                await _context.SaveChangesAsync();
            }

            var detalle = new DetalleCarrito
            {
                CarritoId = carrito.Id,
                ProductoId = productoId,
                Cantidad = cantidad
            };

            _context.DetalleCarritos.Add(detalle);
            await _context.SaveChangesAsync();

            return Ok("Producto agregado al carrito");
        }

        // Ver carrito
        [HttpGet("{usuarioId}")]
        public async Task<IActionResult> VerCarrito(int usuarioId)
        {
            var carrito = await _context.Carritos
                .Include(c => c.Detalles)
                .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId && c.Activo);

            return Ok(carrito);
        }
    }
}