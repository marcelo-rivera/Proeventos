using Testiculo.Application.Dtos;

namespace Testiculo.Application.Contratos
{
    public interface ILoteService
    {
        Task<LoteDto[]> SaveLotes(int eventoId,LoteDto[] model);    
        Task<bool> DeleteLote(int eventoId, int loteId);   

        Task<LoteDto[]> GetLotesbyEventoIdAsync(int eventoId);
        Task<LoteDto> GetLotebyIdsAsync(int eventoId, int loteId);         
    }
}