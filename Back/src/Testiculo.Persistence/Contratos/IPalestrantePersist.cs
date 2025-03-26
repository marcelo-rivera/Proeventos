using System.Threading.Tasks;
using Testiculo.Domain;
using Testiculo.Persistence.Models;

namespace Testiculo.Persistence.Contratos
{
    public interface IPalestrantePersist : IGeralPersist
    {
        //Palestrantes
        Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos=false);
        Task<Palestrante> GetPalestrantesByUserIdAsync(int userId, bool includeEventos=false);      
          
    }
}