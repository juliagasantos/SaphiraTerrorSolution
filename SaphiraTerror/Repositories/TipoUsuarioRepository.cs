using Microsoft.EntityFrameworkCore;
using SaphiraTerror.Data;
using SaphiraTerror.Interfaces;
using SaphiraTerror.Models;

namespace SaphiraTerror.Repositories
{
    public class TipoUsuarioRepository : ITipoUsuarioRepository
    {
        private readonly SaphiraTerrorDbContext _context;
        public TipoUsuarioRepository(SaphiraTerrorDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TipoUsuario tipoUsuario)
        {
            await _context.TipoUsuarios.AddAsync(tipoUsuario);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tipoUsuario = await _context.TipoUsuarios.FirstOrDefaultAsync(g => g.IdTipoUsuario == id);
            if (tipoUsuario != null)
            {
                _context.TipoUsuarios.Remove(tipoUsuario);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<TipoUsuario>> GetAllAsync()
        {
            return await _context.TipoUsuarios.ToListAsync();
        }

        public async Task<TipoUsuario> GetByIdAsync(int id)
        {
            return await _context.TipoUsuarios.FirstOrDefaultAsync(t => t.IdTipoUsuario == id);
        }

        public async Task UpdateAsync(TipoUsuario tipoUsuario)
        {
            _context.TipoUsuarios.Update(tipoUsuario);
            await _context.SaveChangesAsync();
        }
    }
}
