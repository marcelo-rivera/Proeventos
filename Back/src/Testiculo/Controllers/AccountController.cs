using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using testiculo.Extensions;
using Testiculo.Application.Contratos;
using Testiculo.Application.Dtos;
using Testiculo.Helpers;

namespace testiculo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;
        private readonly IUtil _util;
        private readonly string _destino = "Perfil";

        public AccountController(IAccountService accountService,
                                ITokenService tokenService,
                                IUtil util)
        {
            _util = util;
            _accountService = accountService;
            _tokenService = tokenService;
        }

        [HttpGet("GetUser")]
        
        public async Task<IActionResult> GetUser()
        {

            try
            {
                var userName = User.GetUserName();

                var user = await _accountService.GetUserByUserNameAsync(userName);
                return Ok(user);

            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar Usuário. Erro: {ex.Message}");
            }
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            try
            {
                if(await _accountService.UserExists(userDto.UserName))
                    return BadRequest("Usuário já existe");

                var user = await _accountService.CreateAccountAsync(userDto);
                if (user != null)
                    return Ok(new
                {
                    userName = user.UserName,
                    primeiroNome = user.PrimeiroNome,
                    token = _tokenService.CreateToken(user).Result
                });
                
                return BadRequest("Usuário não criado. Tente novamente mais tarde!");
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar Registrar Usuário. Erro: {ex.Message}");
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsync(userLogin.UserName);
                if (user == null) return Unauthorized("Nome de Usuario ou Senha Inválida");
                
                var result = await _accountService.CheckUserPasswordAsync(user, userLogin.Password);
                if(!result.Succeeded) return Unauthorized();

                return Ok(new
                {
                    userName = user.UserName,
                    primeiroNome = user.PrimeiroNome,
                    token = _tokenService.CreateToken(user).Result
                });
                
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar realizar Login. Erro: {ex.Message}");
            }
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> Updateuser(UserUpdateDto userUpdateDto)
        {
            try
            {
                if (userUpdateDto.UserName != User.GetUserName()) 
                    return Unauthorized("Não é possível atualizar outro usuário");
                    
                var user = await _accountService.GetUserByUserNameAsync(User.GetUserName());
                if (user == null) return Unauthorized("Usuário Inválido");

                var userReturn = await _accountService.UpdateAccount(userUpdateDto);
                if (userReturn == null) return NoContent();
                
                return Ok(new
                {
                    userName = userReturn.UserName,
                    primeiroNome = userReturn.PrimeiroNome,
                    token = _tokenService.CreateToken(userReturn).Result
                });
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar atualizar Usuário. Erro: {ex.Message}");
            }
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadIimage()
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsync(User.GetUserName());
                if (user == null) return NoContent();

                var file = Request.Form.Files[0];
                if(file.Length > 0)
                {
                    _util.DeleteImage(user.ImagemURL, _destino);
                    user.ImagemURL = await _util.SaveImage(file, _destino);
                }
                var userRetorno = await _accountService.UpdateAccount(user);

                return Ok(userRetorno);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar realizar upload imagem do Usuário. Erro: {ex.Message}");
            }
        }

        
    }
}