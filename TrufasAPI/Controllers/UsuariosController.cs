using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrufasAPI.Data;
using TrufasAPI.Models;

namespace TrufasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        // Registrar usuario
        [HttpPost("registro")]
        public async Task<ActionResult<Usuario>> Registrar(Usuario usuario)
        {
            usuario.FechaRegistro = DateTime.Now;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(usuario);
        }

        // Obtener todos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }
    }
}