using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIUsuario.Models;

namespace WebAPIUsuario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly DbUsuarioContext _context;

        public UsuarioController(DbUsuarioContext context)
        {
            _context = context;
        }

        [HttpGet("listar")]
        public async Task<ActionResult<IEnumerable<Usuario>>> ListarUsuario()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return Ok(usuarios); //200
        }

        [HttpPost("guardar")]
        public async Task<ActionResult<Usuario>> GuardarUsuario(Usuario usuario)
        {
            usuario.FechaCreacion = DateTime.Now;
            _context.Usuarios.Add(usuario); 
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created, usuario);
        }
        
        [HttpPut("actualizar/{id}")]
        public async Task<ActionResult> ActualizarUsuario(int id, Usuario usuario)
        {
            var usuarioActualizado = await _context.Usuarios.FindAsync(id);

            if (usuarioActualizado == null)
            {
                return NotFound(); //404
            }

            usuarioActualizado.Nombres= usuario.Nombres;
            usuarioActualizado.Apellidos = usuario.Apellidos;
            usuarioActualizado.Correo = usuario.Correo;
            usuarioActualizado.Username = usuario.Username;

            await _context.SaveChangesAsync();
            return Ok(usuarioActualizado);
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<ActionResult> EliminarUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound(); //404
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("buscar/{id}")]
        public async Task<ActionResult<Usuario>> BuscarPorId(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario); //200

        }
    }
}
