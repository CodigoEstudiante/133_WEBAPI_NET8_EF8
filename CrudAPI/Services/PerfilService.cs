using Microsoft.EntityFrameworkCore;
using CrudAPI.Context;
using CrudAPI.DTOs;

namespace CrudAPI.Services
{
    public class PerfilService
    {
        private readonly AppDbContext _context;
        public PerfilService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PerfilDTO>> lista()
        {
            var listaDTO = new List<PerfilDTO>();

            foreach (var item in await _context.Perfiles.ToListAsync())
            {
                listaDTO.Add(new PerfilDTO
                {
                    IdPerfil = item.IdPerfil,
                    Nombre = item.Nombre,
                });
            }

            return listaDTO;

        }

    }
}
