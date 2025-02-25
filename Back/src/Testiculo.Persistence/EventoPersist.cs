using Microsoft.EntityFrameworkCore;
using Testiculo.Domain;
using Testiculo.Persistence.Contratos;
using Testiculo.Persistence.Contexto;

namespace Testiculo.Persistence
{
    public class EventoPersist : IEventoPersist
    {
        private readonly TesticuloContext _context;
        public EventoPersist(TesticuloContext context)
        {
            _context = context;
            //_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            //colocado em todos os eventos Get para liberar o objeto para o update.
            //Ex.: query = query.AsNoTracking().OrderBy(e => e.Id);

        }
        public async Task<Evento[]> GetallEventosAsync(int userId, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(e => e.Lotes)
                .Include(e => e.RedesSociais);

            if(includePalestrantes)
            {
                query = query
                    .Include(e => e.PalestrantesEventos)
                    .ThenInclude(pe => pe.Palestrante);
            }

            query = query.AsNoTracking()
                        .Where(e => e.UserId == userId)
                        .OrderBy(e => e.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Evento[]> GetallEventosByTemaAsync(int userId, string tema, bool includePalestrantes=false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(e => e.Lotes)
                .Include(e => e.RedesSociais);

            if(includePalestrantes)
            {
                query = query
                    .Include(e => e.PalestrantesEventos)
                    .ThenInclude(pe => pe.Palestrante);
                    
            }

            query = query.AsNoTracking().OrderBy(e => e.Id)
                .Where(e => e.Tema.ToLower().Contains(tema.ToLower())
                        && e.UserId == userId);

            return await query.ToArrayAsync();
        }

        public async Task<Evento> GetEventosByIdAsync(int userId, int EventoId, bool includePalestrantes=false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(e => e.Lotes)
                .Include(e => e.RedesSociais);

            if(includePalestrantes)
            {
                query = query
                    .Include(e => e.PalestrantesEventos)
                    .ThenInclude(pe => pe.Palestrante);
            }

            query = query.AsNoTracking().OrderBy(e => e.Id)
                .Where(e => e.Id == EventoId 
                        && e.UserId == userId);

            return await query.FirstOrDefaultAsync();
        }

   }
}