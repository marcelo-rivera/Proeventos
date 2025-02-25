using System.Threading.Tasks;
using Testiculo.Domain;

namespace Testiculo.Persistence.Contratos
{
    public interface IEventoPersist
    {
        //Eventos
        Task<Evento[]> GetallEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false);
        Task<Evento[]> GetallEventosAsync(int userId, bool includePalestrantes = false);
        Task<Evento> GetEventosByIdAsync(int userId, int eventoId, bool includePalestrantes = false);
    }
}