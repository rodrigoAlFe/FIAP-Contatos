using persistencia_api.Entities;

namespace persistencia_api.DTOs;

public class ContatoFilaDTO
{
    public string Acao { get; set; } = "";
    public Contato? Contato { get; set; }
    public int? Id { get; set; }
}