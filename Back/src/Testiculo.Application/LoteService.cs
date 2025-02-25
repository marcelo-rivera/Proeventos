using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Testiculo.Application.Contratos;
using Testiculo.Application.Dtos;
using Testiculo.Domain;
using Testiculo.Persistence.Contratos;

namespace Testiculo.Application
{
    public class LoteService : ILoteService
    {
        private readonly IGeralPersist _geralPersist;
        private readonly ILotePersist _lotePersist;
        private readonly IMapper _mapper;

        public LoteService(IGeralPersist geralPersist, 
                                ILotePersist lotePersist,
                                IMapper mapper)
        {
            _geralPersist = geralPersist;
            _lotePersist = lotePersist;
            _mapper = mapper;
        }
        public async Task AddLote(int eventoId, LoteDto model)
        {
            try
            {
                var lote = _mapper.Map<Lote>(model);
                lote.EventoId = eventoId;

                _geralPersist.Add<Lote>(lote);

                await _geralPersist.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto[]> SaveLotes(int eventoId, LoteDto[] models)
        {
            try
            {
 
                var lotes = await _lotePersist.GetLotesbyEventoIdAsync(eventoId);
                if(lotes == null) return null;

                foreach (var model in models)
                {
                    if(model.Id == 0)    
                    {
                        await AddLote(eventoId, model);
                    }
                    else
                    {
                        var lote = lotes.FirstOrDefault(lote => lote.Id == model.Id);
                        model.EventoId = eventoId;
                        _mapper.Map(model, lote);
                        _geralPersist.Update<Lote>(lote);
                        await _geralPersist.SaveChangesAsync();
                    }
                }

                var loteRetorno = await _lotePersist.GetLotesbyEventoIdAsync(eventoId);

                return _mapper.Map<LoteDto[]>(loteRetorno);               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteLote(int eventoId, int loteId)
        {
            try
            {
                var lote = await _lotePersist.GetLotebyIdsAsync(eventoId, loteId);
                if(lote == null) throw new Exception("Delete - Lote não encontrado");

                _geralPersist.Delete<Lote>(lote);
                return await _geralPersist.SaveChangesAsync();
                               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<LoteDto[]> GetLotesbyEventoIdAsync(int eventoId)
        {
            try
            {
                var lotes = await _lotePersist.GetLotesbyEventoIdAsync(eventoId);
                if (lotes == null) return null;

                var resultado = _mapper.Map<LoteDto[]>(lotes);

                return resultado;
            }                                    
            catch (Exception ex)                                                                                                                        
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto> GetLotebyIdsAsync(int eventoId, int loteId)
        {
            try
            {
                var lote = await _lotePersist.GetLotebyIdsAsync(eventoId,loteId);
                if (lote == null) return null;

                var resultado = _mapper.Map<LoteDto>(lote);

                return resultado;
            }                                    
            catch (Exception ex)                                                                                                                        
            {
                throw new Exception(ex.Message);
            }
        }

    }
}