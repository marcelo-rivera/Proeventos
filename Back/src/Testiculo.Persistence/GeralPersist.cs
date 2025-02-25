
using Testiculo.Persistence.Contratos;
using Testiculo.Persistence.Contexto;

namespace Testiculo.Persistence
{
    public class GeralPersist : IGeralPersist
    {
        private readonly TesticuloContext _context;
        public GeralPersist(TesticuloContext context)
        {
            _context = context;
            
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public void DeleteRange<T>(T[] entityArray) where T : class
        {
            _context.RemoveRange(entityArray);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

    }
}