using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrufasAPI.Data;

namespace TrufasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InventarioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetInventario()
        {
            var inventario = await _context.Inventarios
                .Include(i => i.Producto)
                .ToListAsync();

            return Ok(inventario);
        }
    }
}
