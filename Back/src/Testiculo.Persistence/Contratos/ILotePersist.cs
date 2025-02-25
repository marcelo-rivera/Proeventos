using System.Threading.Tasks;
using Testiculo.Domain;

namespace Testiculo.Persistence.Contratos
{
    public interface ILotePersist
    {
        //Eventos
        /// <summary>
        /// metodo get que retornará todos os lotes do evento
        /// </summary>
        /// <param name="eventoId">codigo chave da tabela evento</param>
        /// <returns>array de lotes</returns>
        Task<Lote[]> GetLotesbyEventoIdAsync(int eventoId);
    
    
        /// <summary>
        /// Método get que retornará apenas um lote
        /// </summary>
        /// <param name="eventoId">cíoodigo chave da tabela evento</param>
        /// <returns></returns> <summary>
        /// 
        /// </summary>
        /// <param name="id">Código chave da tabela lote</param>
        /// <returns>apenas um lote</returns>
        Task<Lote> GetLotebyIdsAsync(int eventoId, int id);
    }
}