using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using CrudAPI.Context;
using CrudAPI.DTOs;
using CrudAPI.Entities;

namespace CrudAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {

        private readonly AppDbContext _context;
        public EmpleadoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("lista")]
        public async Task<ActionResult<List<EmpleadoDTO>>> Get() {
        
            var listaDTO = new List<EmpleadoDTO>();
            var listaDB = await _context.Empleados.Include(p => p.PerfilReferencia).ToListAsync();

            foreach (var item in listaDB) {

                listaDTO.Add( new EmpleadoDTO
                {
                    IdEmpleado = item.IdEmpleado,
                    NombreCompleto = item.NombreCompleto,
                    Sueldo = item.Sueldo,
                    IdPerfil = item.IdPerfil,
                    NombrePerfil = item.PerfilReferencia.Nombre
                });
            }

            return Ok(listaDTO);
        }

        [HttpGet]
        [Route("buscar/{id}")]
        public async Task<ActionResult<EmpleadoDTO>> Get(int id)
        {
            var empleadoDTO = new EmpleadoDTO();
            var empleadoDB = await _context.Empleados.Include(p => p.PerfilReferencia)
                .Where(e => e.IdEmpleado == id).FirstAsync();

            empleadoDTO.IdEmpleado = id;
            empleadoDTO.NombreCompleto = empleadoDB.NombreCompleto;
            empleadoDTO.Sueldo = empleadoDB.Sueldo;
            empleadoDTO.IdPerfil = empleadoDB.IdPerfil;
            empleadoDTO.NombrePerfil = empleadoDB.PerfilReferencia.Nombre;
            return Ok(empleadoDTO);
        }

        [HttpPost]
        [Route("guardar")]
        public async Task<ActionResult<EmpleadoDTO>> Guardar(EmpleadoDTO empleadoDTO)
        {

            var empleadoDB = new Empleado
            {
                NombreCompleto = empleadoDTO.NombreCompleto,
                Sueldo = empleadoDTO.Sueldo,
                IdPerfil = empleadoDTO.IdPerfil,
            };

            await _context.Empleados.AddAsync(empleadoDB);
            await _context.SaveChangesAsync();
            return Ok("Empleado agregado");

        }

        [HttpPut]
        [Route("editar")]
        public async Task<ActionResult<EmpleadoDTO>> Editar(EmpleadoDTO empleadoDTO)
        {
            var empleadoDB = await _context.Empleados.Include(p => p.PerfilReferencia)
                .Where(e => e.IdEmpleado == empleadoDTO.IdEmpleado).FirstAsync();

            empleadoDB.NombreCompleto = empleadoDTO.NombreCompleto;
            empleadoDB.Sueldo = empleadoDTO.Sueldo;
            empleadoDB.IdPerfil = empleadoDTO.IdPerfil;

            _context.Empleados.Update(empleadoDB);
            await _context.SaveChangesAsync();
            return Ok("Empleado Modificado");

        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public async Task<ActionResult<EmpleadoDTO>> Eliminar(int id)
        {
            //var empleadoDB = await _context.Empleados.Where(e => e.IdEmpleado == id).FirstOrDefaultAsync();
            var empleadoDB = await _context.Empleados.FindAsync(id);
            
            if(empleadoDB is null) return NotFound("Empleado no encontrado");

            _context.Empleados.Remove(empleadoDB);
            await _context.SaveChangesAsync();

            return Ok("Empleado eliminado");
        }


    }
}
