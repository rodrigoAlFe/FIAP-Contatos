using System.ComponentModel.DataAnnotations;

namespace persistencia_api.Entities;

public class Contato
{
    public int Id { get; set; }

    [Required]
    public string? Nome { get; set; }

    [Required]
    public string? Telefone { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public int Ddd { get; set; }
}
