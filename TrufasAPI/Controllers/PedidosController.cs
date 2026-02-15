using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrufasAPI.Data;
using TrufasAPI.Models;

namespace TrufasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class PedidosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PedidosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("confirmar/{usuarioId}")]
        public async Task<IActionResult> ConfirmarPedido(int usuarioId)
        {
            var carrito = await _context.Carritos
                .Include(c => c.Detalles)
                .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId && c.Activo);

            if (carrito == null || !carrito.Detalles.Any())
                return BadRequest("Carrito vacío");

            decimal total = 0;

            var pedido = new Pedido
            {
                UsuarioId = usuarioId,
                Fecha = DateTime.Now,
                Total = 0
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            foreach (var item in carrito.Detalles)
            {
                total += item.Producto.Precio * item.Cantidad;

                var detallePedido = new DetallePedido
                {
                    PedidoId = pedido.Id,
                    ProductoId = item.ProductoId,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = item.Producto.Precio
                };

                _context.DetallePedidos.Add(detallePedido);

                // Descontar inventario
                var inventario = await _context.Inventarios
                    .FirstOrDefaultAsync(i => i.ProductoId == item.ProductoId);

                if (inventario != null)
                    inventario.Stock -= item.Cantidad;
            }

            pedido.Total = total;

            carrito.Activo = false;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Mensaje = "Compra realizada con éxito",
                Total = total,
                PedidoId = pedido.Id
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetPedidos()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.Detalles)
                .ThenInclude(d => d.Producto)
                .ToListAsync();

            return Ok(pedidos);
        }

        [HttpGet("ticket/{pedidoId}")]
        public async Task<IActionResult> ObtenerTicket(int pedidoId)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.Detalles)
                .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(p => p.Id == pedidoId);

            if (pedido == null)
                return NotFound();

            return Ok(pedido);
        }

        [HttpGet("ventas/total")]
        public async Task<IActionResult> VentasTotales()
        {
            var total = await _context.Pedidos.SumAsync(p => p.Total);

            return Ok(new { TotalVentas = total });
        }
    }
}