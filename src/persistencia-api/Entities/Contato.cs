using System.ComponentModel.DataAnnotations;

namespace persistencia_api.Entities;

public class Contato
{
    public int Id { get; set; }

    [Required]
    public string? Nome { get; set; }

    [Required]
    [RegularExpression(@"\d{4,5}-?\d{4}$", ErrorMessage = "Telefone no formato inválido.")]
    public string? Telefone { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "Endereço de e-mail inválido.")]
    public string? Email { get; set; }

    [Required]
    [RegularExpression(@"^\d{2}$", ErrorMessage = "DDD no formato inválido.")]
    public int Ddd { get; set; }
}
