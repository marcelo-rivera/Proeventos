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
using Testiculo.Persistence.Models;

namespace Testiculo.Application
{
    public class PalestranteService : IPalestranteService
    {
        private readonly IPalestrantePersist _palestrantePersist;
        private readonly IMapper _mapper;

        public PalestranteService(IPalestrantePersist palestrantePersist,
                                IMapper mapper)
        {
            _palestrantePersist = palestrantePersist;
            _mapper = mapper;
        }
        public async Task<PalestranteDto> AddPalestrantes(int userId, PalestranteAddDto model)
        {
            try
            {
                var palestrante = _mapper.Map<Palestrante>(model);
                palestrante.UserId = userId;
                
                if (await _palestrantePersist.SaveChangesAsync())
                {
                    var palestranteRetorno = await _palestrantePersist.GetPalestrantesByUserIdAsync(userId, false);

                    return _mapper.Map<PalestranteDto>(palestranteRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PalestranteDto> UpdatePalestrante(int userId, PalestranteUpdateDto model)
        {
            try
            {
 
                var palestrante = await _palestrantePersist.GetPalestrantesByUserIdAsync(userId, false);
                if(palestrante == null) return null;

                model.Id = palestrante.Id;
                model.UserId = userId;

                _mapper.Map(model, palestrante);

                _palestrantePersist.Update<Palestrante>(palestrante);

                if (await _palestrantePersist.SaveChangesAsync())
                {
                    var palestranteRetorno = await _palestrantePersist.GetPalestrantesByUserIdAsync(userId, false);

                    return _mapper.Map<PalestranteDto>(palestranteRetorno);
                }
                  return null;                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // public async Task<bool> DeletePalestrante(int userId, int eventoId)
        // {
        //     try
        //     {
        //         var evento = await _eventoPersist.GetEventosByIdAsync(userId, eventoId, false);
        //         if(evento == null) throw new Exception("Delete - Evento não encontrado");

        //         _geralPersist.Delete<Evento>(evento);
        //         return await _geralPersist.SaveChangesAsync();
                               
        //     }
        //     catch (Exception ex)
        //     {
        //         throw new Exception(ex.Message);
        //     }
        // }

        public async Task<PageList<PalestranteDto>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false)
        {
            try
            {
                var palestrantes = await _palestrantePersist.GetAllPalestrantesAsync(pageParams, includeEventos);
                if (palestrantes == null) return null;

                var resultado = _mapper.Map<PageList<PalestranteDto>>(palestrantes);

                resultado.TotalCount = palestrantes.TotalCount;
                resultado.PageSize = palestrantes.PageSize;
                resultado.CurrentPage = palestrantes.CurrentPage;
                resultado.TotalPages = palestrantes.TotalPages;

                return resultado;

                //return Palestrantes;
            }                                    
            catch (Exception ex)                                                                                                                        
            {
                throw new Exception(ex.Message);
            }
        }

        // public async Task<EventoDto[]> GetallEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false)
        // {
        //     try
        //     {
        //         var eventos = await _eventoPersist.GetallEventosByTemaAsync(userId, tema,includePalestrantes);
        //         if (eventos == null) return null;

        //         var resultado = _mapper.Map<EventoDto[]>(eventos);

        //         return resultado;
        //     }                                    
        //     catch (Exception ex)                                                                                                                        
        //     {
        //         throw new Exception(ex.Message);
        //     }
        // }

        public async Task<PalestranteDto> GetPalestrantesByUserIdAsync(int userId, bool includeEventos = false)
        {
            try
            {
                var palestrante = await _palestrantePersist.GetPalestrantesByUserIdAsync(userId, includeEventos);
                if (palestrante == null) return null;

                var resultado = _mapper.Map<PalestranteDto>(palestrante);

                return resultado;
            }                                    
            catch (Exception ex)                                                                                                                        
            {
                throw new Exception(ex.Message);
            }
        }
    }

}