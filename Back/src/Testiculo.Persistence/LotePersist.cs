using Microsoft.EntityFrameworkCore;
using Testiculo.Domain;
using Testiculo.Persistence.Contratos;
using Testiculo.Persistence.Contexto;

namespace Testiculo.Persistence
{
    public class LotePersist : ILotePersist
    {
        private readonly TesticuloContext _context;
        public LotePersist(TesticuloContext context)
        {
            _context = context;
            //_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            //colocado em todos os eventos Get para liberar o objeto para o update.
            //Ex.: query = query.AsNoTracking().OrderBy(e => e.Id);

        }
        public async Task<Lote[]> GetLotesbyEventoIdAsync(int eventoId)
        {
            IQueryable<Lote> query = _context.Lotes;

            query = query.AsNoTracking()
                        .Where(lote => lote.EventoId == eventoId);

            return await query.ToArrayAsync();

        }

        public async Task<Lote> GetLotebyIdsAsync(int eventoId, int id)
        {
            IQueryable<Lote> query = _context.Lotes;

            query = query.AsNoTracking()
                        .Where(lote => lote.EventoId == eventoId
                                && lote.Id == id);

            return await query.FirstOrDefaultAsync();
        }
    }
}