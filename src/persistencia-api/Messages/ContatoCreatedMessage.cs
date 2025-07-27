using System.ComponentModel.DataAnnotations;

namespace persistencia_api.Messages;

public class ContatoCreatedMessage
{
    [Required]
    public string? Nome { get; set; }

    [Required]
    [RegularExpression(@"\d{4,5}-?\d{4}$", ErrorMessage = "Telefone no formato inválido.")]
    public string? Telefone { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [RegularExpression(@"^\d{2}$", ErrorMessage = "DDD no formato inválido.")]
    public int Ddd { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}