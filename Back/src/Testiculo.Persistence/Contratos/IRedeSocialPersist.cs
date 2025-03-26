using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Testiculo.Domain;

namespace Testiculo.Persistence.Contratos
{
    public interface IRedeSocialPersist : IGeralPersist
    {
        Task<RedeSocial> GetRedeSocialEventoByIdsAsync(int eventoId, int id);    
        Task<RedeSocial> GetRedeSocialPalestranteByIdsAsync(int palestranteId, int id);    
        Task<RedeSocial[]> GetAllByEventoIdAsync(int eventoId);    
        Task<RedeSocial[]> GetAllByPalestranteIdAsync(int palestranteId);    
    }
}