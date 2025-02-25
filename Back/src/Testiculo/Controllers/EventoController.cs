using Microsoft.AspNetCore.Mvc;
using Testiculo.Application.Contratos;
using Testiculo.Application.Dtos;
using testiculo.Extensions;
using Microsoft.AspNetCore.Authorization;

using System.Security.Claims;


namespace Testiculo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        // public IEnumerable<Evento> _evento = new Evento[] {
        //         new Evento(){
        //             EventoId = 1,
        //             Tema = "Angula 11 e .NET 5",
        //             Local = "Belo Horizonte",
        //             Lote = "1º lote",
        //             QtdPessoas = 250,
        //             DataEvento = DateTime.Now.AddDays(2),
        //             ImagemURL = "foto.png"
        //         },
        //         new Evento(){
        //             EventoId = 2,
        //             Tema = "Angular 11 e suas novidades",
        //             Local = "São Paulo",
        //             Lote = "2º lote",
        //             QtdPessoas = 350,
        //             DataEvento = DateTime.Now.AddDays(5),
        //             ImagemURL = "foto2.png"
        //         }
        // };
        
        //private readonly TesticuloContext _context;
        private readonly IEventoService _eventoService;
        private readonly IWebHostEnvironment _hostEnvironment;

        private readonly IAccountService _accountService;

        //public EventoController(TesticuloContext context)
        public EventoController(IEventoService eventoService, 
                                IWebHostEnvironment hostEnvironment,
                                IAccountService accountService)
        {
        //    _context = context;
            _eventoService = eventoService;
            _hostEnvironment = hostEnvironment;
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //return _evento; 
            //return _context.Eventos;
            try
            {
                var eventos = await _eventoService.GetallEventosAsync(User.GetUserId(), true);
                if (eventos == null) NoContent();

                return Ok(eventos);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar eventos. Erro {ex.Message}");

            }
        }
        
        [HttpGet("{id}")]
        //public async Evento GetById(int id)
        public async Task<IActionResult> GetById(int id)
        {
            //return _evento.Where(evento => evento.EventoId == id);
            //return _context.Eventos.Where(evento => evento.Id == id);
            //return _context.Eventos.FirstOrDefault(evento => evento.Id == id);
            try
            {
                var evento = await _eventoService.GetEventosByIdAsync(User.GetUserId(), id, true);
                if (evento == null) NoContent();

                return Ok(evento);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar eventos. Erro {ex.Message}");

            }            
        }
        [HttpGet("{tema}/tema")]

        //public async Evento GetByTema(int id)
        public async Task<IActionResult> GetByTema(string tema)
        {
            //return _evento.Where(evento => evento.EventoId == id);
            //return _context.Eventos.Where(evento => evento.Id == id);
            //return _context.Eventos.FirstOrDefault(evento => evento.Id == id);
            try
            {
                var evento = await _eventoService.GetallEventosByTemaAsync(User.GetUserId(), tema, true);
                if (evento == null) NoContent();

                return Ok(evento);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar eventos. Erro {ex.Message}");

            }            
        }

        [HttpPost("upload-image/{eventoId}")]
        public async Task<IActionResult> UploadIimage(int eventoId)
        {
            try
            {
                var evento = await _eventoService.GetEventosByIdAsync(User.GetUserId(), eventoId,false);
                if (evento == null) return NoContent();

                var file = Request.Form.Files[0];
                if(file.Length > 0)
                {
                    DeleteImage(evento.ImagemURL);
                    evento.ImagemURL = await SaveImage(file);
                }
                var EventoRetorno = await _eventoService.UpdateEvento(User.GetUserId(), eventoId, evento);

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar adicionar eventos. Erro: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                var evento = await _eventoService.AddEventos(User.GetUserId(), model);
                if (evento == null) return NoContent();

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar adicionar eventos. Erro: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EventoDto model)
        {
            try
            {
                var evento = await _eventoService.UpdateEvento(User.GetUserId(), id, model);
                if (evento == null) return NoContent();

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar atualizar eventos. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var evento = await _eventoService.GetEventosByIdAsync(User.GetUserId(), id, true);
                if (evento == null) NoContent();

                if (await _eventoService.DeleteEvento(User.GetUserId(), id))
                {
                    DeleteImage(evento.ImagemURL);
                    return Ok(new { message = "Excluído"});
                }
                else
                {
                    throw new Exception("Ocorreu um problema não específico ao tentar excluir Evento.");
                }
                
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar eventos. Erro: {ex.Message}");
            }
        }

        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName)
                                            .Take(10)
                                            .ToArray()
                                            ).Replace(' ','-');

            imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imageFile.FileName)}";

            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/images", imageName);

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            
            return imageName;
        }

        [NonAction]
        public void DeleteImage(string imageName)
        {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/images", imageName); 
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
        }
    }
}