using Microsoft.AspNetCore.Mvc;
using Testiculo.Application.Contratos;
using Testiculo.Application.Dtos;
using testiculo.Extensions;
using Microsoft.AspNetCore.Authorization;

using System.Security.Claims;
using Testiculo.Persistence.Models;
using Testiculo.Extensions;


namespace Testiculo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PalestranteController : ControllerBase
    {

        private readonly IPalestranteService _palestranteService;
        private readonly IWebHostEnvironment _hostEnvironment;

        private readonly IAccountService _accountService;

        public PalestranteController(IPalestranteService palestranteService, 
                                IWebHostEnvironment hostEnvironment,
                                IAccountService accountService)
        {
            _palestranteService = palestranteService;
            _hostEnvironment = hostEnvironment;
            _accountService = accountService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery]PageParams pageParams)
        {
            try
            {
                var palestrantes = await _palestranteService.GetAllPalestrantesAsync(pageParams, true);
                if (palestrantes == null) NoContent();

                Response.AddPagination(palestrantes.CurrentPage,
                                       palestrantes.PageSize,
                                       palestrantes.TotalCount,
                                       palestrantes.TotalPages);

                return Ok(palestrantes);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar palestrantes. Erro {ex.Message}");

            }
        }
        
        [HttpGet()]
        public async Task<IActionResult> GetPalestrantes()
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId(), true);
                if (palestrante == null) NoContent();

                return Ok(palestrante);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar palestrantes. Erro {ex.Message}");

            }            
        }

        [HttpPost]
        public async Task<IActionResult> Post(PalestranteAddDto model)
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId(), false);
                
                if (palestrante == null)
                    palestrante = await _palestranteService.AddPalestrantes(User.GetUserId(), model);

                return Ok(palestrante);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar adicionar palestrantes. Erro: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(PalestranteUpdateDto model)
        {
            try
            {
                var palestrante = await _palestranteService.UpdatePalestrante(User.GetUserId(), model);
                if (palestrante == null) return NoContent();

                return Ok(palestrante);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar atualizar palestrantes. Erro: {ex.Message}");
            }
        }

    }
}