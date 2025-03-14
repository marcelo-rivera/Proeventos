using System.Threading.Tasks;
using Testiculo.Domain;
using Testiculo.Persistence.Models;

namespace Testiculo.Persistence.Contratos
{
    public interface IEventoPersist
    {
        //Eventos
        //Task<PageList<Evento>> GetallEventosByTemaAsync(int userId, PageParams pageParams, string tema, bool includePalestrantes = false);
        Task<PageList<Evento>> GetallEventosAsync(int userId, PageParams pageParams, bool includePalestrantes = false);
        Task<Evento> GetEventosByIdAsync(int userId, int eventoId, bool includePalestrantes = false);
    }
}