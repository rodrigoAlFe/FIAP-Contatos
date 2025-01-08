using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FIAP.Contatos.Domain.Entities
{
    public class Contato
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O Campo Nome é obrigatório.")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "O Campo Telefone é obrigatório.")]
        [RegularExpression(@"\d{4,5}-?\d{4}$", ErrorMessage = "Telefone no formato inválido.")]
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "O Campo E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Endereço de e-mail inválido.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "O Campo DDD é obrigatório.")]
        [RegularExpression(@"^\d{2}$", ErrorMessage = "DDD no formato inválido.")]
        public int Ddd { get; set; }

        
        
    }
}
