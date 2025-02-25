using Microsoft.AspNetCore.Mvc;
using Testiculo.Persistence;
using Testiculo.Domain;
using Testiculo.Persistence.Contexto;
using Testiculo.Application.Contratos;
using System.Diagnostics.Tracing;
using Testiculo.Application.Dtos;

namespace Testiculo.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LoteController : ControllerBase
    {
        // public IEnumerable<Evento> _evento = new Lote[] {
        //         new Lote(){
        //             LoteId = 1,
        //             Tema = "Angula 11 e .NET 5",
        //             Local = "Belo Horizonte",
        //             Lote = "1º lote",
        //             QtdPessoas = 250,
        //             DataEvento = DateTime.Now.AddDays(2),
        //             ImagemURL = "foto.png"
        //         },
        //         new Lote(){
        //             LoteId = 2,
        //             Tema = "Angular 11 e suas novidades",
        //             Local = "São Paulo",
        //             Lote = "2º lote",
        //             QtdPessoas = 350,
        //             DataEvento = DateTime.Now.AddDays(5),
        //             ImagemURL = "foto2.png"
        //         }
        // };
        
        //private readonly TesticuloContext _context;
        private readonly ILoteService _loteService;
        

        //public LoteController(TesticuloContext context)
        public LoteController(ILoteService loteService)
        {
        //    _context = context;
            _loteService = loteService;

            
        }

        [HttpGet("{eventoId}")]
        public async Task<IActionResult> Get(int eventoId)
        {
            //return _evento; 
            //return _context.Eventos;
            try
            {
                var lotes = await _loteService.GetLotesbyEventoIdAsync(eventoId);
                if (lotes == null) NoContent();

                // var eventosRetorno = new List<EventoDto>();

                // foreach (var evento in eventos)
                // {
                //     eventosRetorno.Add(new EventoDto() {
                //         Id = evento.Id,
                //         Local = evento.Local,
                //         DataEvento = evento.DataEvento.ToString(),
                //         Tema = evento.Tema,
                //         QtdPessoas = evento.QtdPessoas,
                //         ImagemURL = evento.ImagemURL,
                //         Telefone = evento.Telefone,
                //         Email = evento.Email
                //     });
                // }

                // return Ok(eventosRetorno); // 
                return Ok(lotes);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar lotes. Erro {ex.Message}");

            }
        }
        

        [HttpPut("{eventoId}")]
        public async Task<IActionResult> SaveLotes(int eventoId, LoteDto[] models)
        {
            try
            {
                var lotes = await _loteService.SaveLotes( eventoId, models);
                if (lotes == null) return NoContent();

                return Ok(lotes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar salvar lotes. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{eventoId}/{loteId}")]
        public async Task<IActionResult> Delete(int eventoId, int loteId)
        {
            try
            {
                var lote = await _loteService.GetLotebyIdsAsync(eventoId,loteId);
                if (lote == null) NoContent();

                return await _loteService.DeleteLote(lote.EventoId, lote.Id) ?
                    Ok(new { message = "Lote Excluído"}) :
                    throw new Exception("Ocorreu um problema não específico ao tentar excluir lote.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar lote. Erro: {ex.Message}");
            }
        }
    }
}