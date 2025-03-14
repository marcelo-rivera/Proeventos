using Testiculo.Application.Dtos;
using Testiculo.Persistence.Models;

namespace Testiculo.Application.Contratos
{
    public interface IEventoService
    {
        Task<EventoDto> AddEventos(int userId, EventoDto model);    
        Task<EventoDto> UpdateEvento(int userId, int eventoId,EventoDto model);    
        Task<bool> DeleteEvento(int userId, int eventoId);   

        Task<PageList<EventoDto>> GetallEventosAsync(int userId, PageParams pageParams, bool includePalestrantes = false);
        
        //Task<EventoDto[]> GetallEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false);
        Task<EventoDto> GetEventosByIdAsync(int userId, int eventoId, bool includePalestrantes = false);         
    }
}