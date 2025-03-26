
using Microsoft.EntityFrameworkCore;
using Testiculo.Domain;
using Testiculo.Persistence.Contratos;
using Testiculo.Persistence.Contexto;
using Testiculo.Persistence.Models;

namespace Testiculo.Persistence
{
    public class PalestrantePersist : GeralPersist, IPalestrantePersist
    {
        private readonly TesticuloContext _context;
        public PalestrantePersist(TesticuloContext context) : base(context)
        {
            _context = context;
            
        }
        public async Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(p => p.User)
                .Include(p => p.RedesSociais);

            if(includeEventos)
            {
                query = query
                    .Include(p => p.PalestrantesEventos)
                    .ThenInclude(pe => pe.Evento);
            }

            query = query.AsNoTracking()
                        .Where(p => (p.MiniCurriculo.ToLower().Contains(pageParams.Term.ToLower()) ||
                                    p.User.PrimeiroNome.ToLower().Contains(pageParams.Term.ToLower()) ||
                                    p.User.UltimoNome.ToLower().Contains(pageParams.Term.ToLower())) &&
                                    p.User.Funcao == Domain.Enum.Funcao.Palestrante)
                        .OrderBy(p => p.Id);

            return await PageList<Palestrante>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        // public async Task<Palestrante[]> GetallPalestrantesByNomeAsync(string nome, bool includeEventos=false)
        // {
        //     IQueryable<Palestrante> query = _context.Palestrantes
        //         .Include(p => p.RedesSociais);

        //     if(includeEventos)
        //     {
        //         query = query
        //             .Include(p => p.PalestrantesEventos)
        //             .ThenInclude(pe => pe.Evento);
        //     }

        //     query = query.AsNoTracking().OrderBy(p => p.Id)
        //         .Where(p => p.User.PrimeiroNome.ToLower().Contains(nome.ToLower()));

        //     return await query.ToArrayAsync();
        // }

        public async Task<Palestrante> GetPalestrantesByUserIdAsync(int userId, bool includeEventos=false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(p => p.User)
                .Include(p => p.RedesSociais);

            if(includeEventos)
            {
                query = query
                    .Include(p => p.PalestrantesEventos)
                    .ThenInclude(pe => pe.Evento);
            }

            query = query.AsNoTracking().OrderBy(p => p.Id)
                        .Where(p => p.UserId == userId);

            return await query.FirstOrDefaultAsync();        }
    }
}