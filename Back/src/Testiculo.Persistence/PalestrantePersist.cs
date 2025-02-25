
using Microsoft.EntityFrameworkCore;
using Testiculo.Domain;
using Testiculo.Persistence.Contratos;
using Testiculo.Persistence.Contexto;

namespace Testiculo.Persistence
{
    public class PalestrantePersist : IPalestrantePersist
    {
        private readonly TesticuloContext _context;
        public PalestrantePersist(TesticuloContext context)
        {
            _context = context;
            
        }
        public async Task<Palestrante[]> GetallPalestrantesAsync(bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(p => p.RedesSociais);

            if(includeEventos)
            {
                query = query
                    .Include(p => p.PalestrantesEventos)
                    .ThenInclude(pe => pe.Evento);
            }

            query = query.AsNoTracking().OrderBy(p => p.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante[]> GetallPalestrantesByNomeAsync(string nome, bool includeEventos=false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(p => p.RedesSociais);

            if(includeEventos)
            {
                query = query
                    .Include(p => p.PalestrantesEventos)
                    .ThenInclude(pe => pe.Evento);
            }

            query = query.AsNoTracking().OrderBy(p => p.Id)
                .Where(p => p.User.PrimeiroNome.ToLower().Contains(nome.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante> GetPalestrantesByIdAsync(int PalestranteId, bool includeEventos=false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(p => p.RedesSociais);

            if(includeEventos)
            {
                query = query
                    .Include(p => p.PalestrantesEventos)
                    .ThenInclude(pe => pe.Evento);
            }

            query = query.AsNoTracking().OrderBy(p => p.Id)
                        .Where(p => p.Id == PalestranteId);

            return await query.FirstOrDefaultAsync();        }
    }
}