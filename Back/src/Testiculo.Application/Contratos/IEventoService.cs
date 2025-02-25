using Testiculo.Application.Dtos;

namespace Testiculo.Application.Contratos
{
    public interface IEventoService
    {
        Task<EventoDto> AddEventos(int userId, EventoDto model);    
        Task<EventoDto> UpdateEvento(int userId, int eventoId,EventoDto model);    
        Task<bool> DeleteEvento(int userId, int eventoId);   

        Task<EventoDto[]> GetallEventosAsync(int userId, bool includePalestrantes = false);
        Task<EventoDto[]> GetallEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false);
        Task<EventoDto> GetEventosByIdAsync(int userId, int eventoId, bool includePalestrantes = false);         
    }
}