using System.ComponentModel.DataAnnotations;

namespace FIAP.Domain.Extensions.Objects.Contact;

/// <summary>
/// Represents an object for handling area code (DDD) validation and manipulation.
/// </summary>
/// <param name="areaCode">The string of the area code (DDD)</param>
public class AreaCodeNumber(string areaCode)
{
    [Required(ErrorMessage = "O código de área é obrigatório")]
    [Display(Name = "Código de Área", Description = "DDD do telefone")]
    [RegularExpression(@"^\([1-9][0-9]\)$"
        , ErrorMessage = "O código de área (DDD) é inválido.")]
    public string Value { get; } = areaCode;
    
    public static implicit operator string(AreaCodeNumber? areaCodeNumber)
        => areaCodeNumber is null
            ? string.Empty
            : areaCodeNumber.Value;
    
    public static implicit operator AreaCodeNumber(string areaCodeNumber) 
        => new(areaCodeNumber);
    public override string ToString() => Value;
}