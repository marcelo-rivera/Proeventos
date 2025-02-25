//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Testiculo.Domain.Identity;

namespace Testiculo.Domain
{
    //[Table("EventosDetalhes"] // para que a tabela no banco tenha nome diferente
    public class Evento
    {
        //[Key] // se vc quiser mudar o nome da chave de Id para outro qualquer (codigo por ex)
        public int Id { get; set; }
        public string? Local { get; set; }
        public DateTime? DataEvento { get; set; }

        //[NotMapped] // campo da classe que não será criado na tabela. 
        //public int ContagemDias {get; set;}

        //[Required] //para exigir que seja preenchido
        //[MaxLength(50)]
        public string? Tema { get; set; }
        public int QtdPessoas { get; set; }
        public string? ImagemURL { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public IEnumerable<Lote> Lotes { get; set; }
        public IEnumerable<RedeSocial>? RedesSociais { get; set; }
        public IEnumerable<PalestranteEvento>? PalestrantesEventos { get; set; }
        
    }
}