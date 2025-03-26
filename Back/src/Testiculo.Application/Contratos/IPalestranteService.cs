using Testiculo.Application.Dtos;
using Testiculo.Persistence.Models;

namespace Testiculo.Application.Contratos
{
    public interface IPalestranteService
    {
        Task<PalestranteDto> AddPalestrantes(int userId, PalestranteAddDto model);    
        Task<PalestranteDto> UpdatePalestrante(int userId, PalestranteUpdateDto model);    
        Task<PageList<PalestranteDto>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false);
        //Task<PalestranteDto[]> GetallPalestrantesByTemaAsync(int userId, string tema, bool includePalestrantes = false);
        Task<PalestranteDto> GetPalestrantesByUserIdAsync(int userId, bool includeEventos = false);         
    }
}