﻿using Microsoft.EntityFrameworkCore;
using SaphiraTerror.Data;
using SaphiraTerror.Interfaces;
using SaphiraTerror.Models;

namespace SaphiraTerror.Repositories
{
    public class GeneroRepository : IGeneroRepository
    {
        //campo de apoio
        private readonly SaphiraTerrorDbContext _context;
        //injeção de dependência no construtor
        public GeneroRepository(SaphiraTerrorDbContext context)
        {
            _context = context;
        }


        public async Task AddAsync(Genero genero)
        {
            await _context.Generos.AddAsync(genero);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var genero = await _context.Generos.FirstOrDefaultAsync(g => g.IdGenero == id);
            if (genero != null)
            {
                _context.Generos.Remove(genero);
                await _context.SaveChangesAsync();
            }
        }

        //todos os generos
        public async Task<List<Genero>> GetAllAsync()
        {
            return await _context.Generos.Include(g => g.Filmes).ToListAsync();
        }

        
        public async Task<Genero> GetByIdAsync(int id)
        {
            return await _context.Generos.Include(g => g.Filmes).FirstOrDefaultAsync(g => g.IdGenero == id);
        }

        public async Task UpdateAsync(Genero genero)
        {
            _context.Generos.Update(genero);
            await _context.SaveChangesAsync();
        }
    }
}
