using System.Threading.Tasks;
using Testiculo.Domain;

namespace Testiculo.Persistence.Contratos
{
    public interface IPalestrantePersist
    {
        //Palestrantes

        Task<Palestrante[]> GetallPalestrantesByNomeAsync(string nome, bool includeEventos);
        Task<Palestrante[]> GetallPalestrantesAsync(bool includeEventos);
        Task<Palestrante> GetPalestrantesByIdAsync(int palestranteId, bool includeEventos);        
    }
}