namespace cadastro_api.DTOs;

public class ContatoFilaDTO
{
    public string Acao { get; set; } = ""; // "create", "update" ou "delete"
    public ContatoDTO? Contato { get; set; } // DTO que passará o contato para dar create ou update
    public int? Id { get; set; } // O id caso for DELETE, para identificar qual contato deletar
}