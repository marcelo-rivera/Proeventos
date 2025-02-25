using System.ComponentModel.DataAnnotations;

namespace Testiculo.Application.Dtos
{
    public class EventoDto
    {
        public int Id { get; set; }
        public string? Local { get; set; }
        public string? DataEvento { get; set; }

        [Required (ErrorMessage = "O campo {0} é obrigatório."),
        //MinLength(3, ErrorMessage = "{0} deve ter no mínimo 3 caracteres"),
        //MaxLength(30, ErrorMessage = "{0} deve ter no máximo 30 caracteres")
         StringLength (30,MinimumLength =3,
            ErrorMessage = "O Tamanho do campo deve estar entre 3 e 30 caracteres")]
        public string? Tema { get; set; }

        [Range(1,120000,ErrorMessage ="{0} deve estar no intervalo entre 1 e 120000"),
        Display(Name = "Qtde Pessoas")]
        public int QtdPessoas { get; set; }
        
        [RegularExpression(@".*\.(gif|jpe?g|bmp|png)$",
                           ErrorMessage = "Não é uma imagem válida. (gif,jpg,jpeg,bmp ou png)")]
        public string? ImagemURL { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório"),
        Phone(ErrorMessage = "O campo {0} está com número inválido")]
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório."),
        Display(Name = "e-mail"),
        EmailAddress (ErrorMessage = "O campo {0} deve ser um e-mail válido.")]
        public string? Email { get; set; }
        public int UserId { get; set; }
        public UserDto? UserDto { get; set; }
        public IEnumerable<LoteDto> Lotes { get; set; }
        public IEnumerable<RedeSocialDto>? RedesSociais { get; set; }
        public IEnumerable<PalestranteDto>? Palestrantes { get; set; }        
    }
}