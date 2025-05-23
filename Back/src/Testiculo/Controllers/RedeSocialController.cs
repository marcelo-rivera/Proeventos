using Microsoft.AspNetCore.Mvc;
using Testiculo.Persistence;
using Testiculo.Domain;
using Testiculo.Persistence.Contexto;
using Testiculo.Application.Contratos;
using System.Diagnostics.Tracing;
using Testiculo.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using testiculo.Extensions;

namespace Testiculo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RedeSocialController : ControllerBase
    {
        private readonly IRedeSocialService _redeSocialService;
        private readonly IEventoService _eventoService;
        private readonly IPalestranteService _palestranteService;

        public RedeSocialController(IRedeSocialService redeSocialService,
                                    IEventoService eventoService,
                                    IPalestranteService palestranteService)
        {
            _redeSocialService = redeSocialService;
            _eventoService = eventoService;
            _palestranteService = palestranteService;
        }

        [HttpGet("evento/{eventoId}")]
        public async Task<IActionResult> GetByEvento(int eventoId)
        {
            try
            {
                if (!(await AutorEvento(eventoId))) 
                    return Unauthorized("Acesso não autorizado.");

                var redeSocials = await _redeSocialService.GetAllByEventoIdAsync(eventoId);
                if (redeSocials == null) NoContent();

                return Ok(redeSocials);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar Redes Sociais. Erro {ex.Message}");

            }
        }

        [HttpGet("palestrante")]
        public async Task<IActionResult> GetByPalestrante()
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
                if (palestrante == null)
                    return Unauthorized("Acesso não autorizado.");

                var redeSocials = await _redeSocialService.GetAllByPalestranteIdAsync(palestrante.Id);
                if (redeSocials == null) NoContent();

                return Ok(redeSocials);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar Redes Sociais. Erro {ex.Message}");

            }
        }
        

        [HttpPut("evento/{eventoId}")]
        public async Task<IActionResult> SaveByEvento(int eventoId, RedeSocialDto[] models)
        {
            try
            {
                if (!(await AutorEvento(eventoId))) 
                    return Unauthorized("Acesso não autorizado.");

                var redeSocial = await _redeSocialService.SaveByEvento( eventoId, models);
                if (redeSocial == null) return NoContent();

                return Ok(redeSocial);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar salvar Redes Sociais. Erro: {ex.Message}");
            }
        }

        [HttpPut("palestrante")]
        public async Task<IActionResult> SaveByPalestrante(RedeSocialDto[] models)
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
                if (palestrante == null)
                    return Unauthorized("Acesso não autorizado.");
                    
                var redeSocial = await _redeSocialService.SaveByPalestrante( palestrante.Id, models);
                if (redeSocial == null) return NoContent();

                return Ok(redeSocial);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar salvar Redes Sociais. Erro: {ex.Message}");
            }
        }

        [HttpDelete("evento/{eventoId}/{redeSocialId}")]
        public async Task<IActionResult> DeleteByEvento(int eventoId, int redeSocialId)
        {
            try
            {
                if (!(await AutorEvento(eventoId))) 
                    return Unauthorized("Acesso não autorizado.");

                var redeSocial = await _redeSocialService.GetRedeSocialEventoByIdsAsync(eventoId,redeSocialId);
                if (redeSocial == null) NoContent();

                return await _redeSocialService.DeleteByEvento(eventoId, redeSocialId) ?
                    Ok(new { message = "Rede Social Excluída"}) :
                    throw new Exception("Ocorreu um problema não específico ao tentar excluir rede social.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar Rede Social por Evento. Erro: {ex.Message}");
            }
        }

        [HttpDelete("palestrante/{redeSocialId}")]
        public async Task<IActionResult> DeleteByPalestrante(int redeSocialId)
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
                if (palestrante == null)
                    return Unauthorized("Acesso não autorizado.");
                    
                var redeSocial = await _redeSocialService.GetRedeSocialPalestranteByIdsAsync(palestrante.Id,redeSocialId);
                if (redeSocial == null) NoContent();

                return await _redeSocialService.DeleteByPalestrante((int)redeSocial.PalestranteId, redeSocial.Id) ?
                    Ok(new { message = "Rede Social Excluída"}) :
                    throw new Exception("Ocorreu um problema não específico ao tentar excluir rede social.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar Rede Social por Palestrante. Erro: {ex.Message}");
            }
        }

        [NonAction]
        private async Task<bool> AutorEvento(int eventoId)
        {
            var evento = await _eventoService.GetEventosByIdAsync(User.GetUserId(), eventoId, false);
            if (evento == null) return false;

            return evento.UserId == User.GetUserId();
        }
    }
}