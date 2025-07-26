using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SaphiraTerror.Data;
using SaphiraTerror.Interfaces;
using SaphiraTerror.Models;

namespace SaphiraTerror.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        //campo de apoio
        private readonly SaphiraTerrorDbContext _context;
        //construtor com injeção de dependência
        public UsuarioRepository(SaphiraTerrorDbContext context)
        {
            _context = context;
        }

        //inserir novo usuario - create
        public async Task AddAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        //listar todos os usuarios - read
        public async Task<List<Usuario>> GetAllAsync()
        {
            return await _context.Usuarios.Include(u => u.TipoUsuario).ToListAsync();
        }

        //
        public async Task<List<Usuario>> GetAllAtivosAsync()
        {
            return await _context.Usuarios.Where(u => u.Ativo).Include(u => u.TipoUsuario).ToListAsync();
        }
        
        //buscar por id
        public async Task<Usuario> GetByIdAsync(int id)
        {
            return await _context.Usuarios.Include(u => u.TipoUsuario).FirstOrDefaultAsync(u => u.IdUsuario == id);

        }

        //inativar usuario
        public async Task InativarAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if  (usuario != null && usuario.Ativo)
            {
                usuario.Ativo = false;
                await _context.SaveChangesAsync();
            }

        }

        //reativar usuario
        public async Task ReativarAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null && !usuario.Ativo)
            {
                usuario.Ativo = true;
                await _context.SaveChangesAsync();
            }
        }

        //editar usuario
        public async Task UpdateAsync(Usuario usuario)
        {
             _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }


        public async Task<Usuario?> ValidarLoginAsync(string email, string senha)
        {
            return await _context.Usuarios.Include(u => u.TipoUsuario).FirstOrDefaultAsync(u => u.Email == email && u.Senha == senha);
        }
    }
}
