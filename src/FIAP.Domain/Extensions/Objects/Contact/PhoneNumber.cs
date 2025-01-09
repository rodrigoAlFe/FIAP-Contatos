using System.ComponentModel.DataAnnotations;

namespace FIAP.Domain.Extensions.Objects.Contact;

/// <summary>
/// Represents a phone number with specific formatting and validation requirements.
/// </summary>
/// <param name="value">The string of the phone number</param>
public class PhoneNumber(string value)
{
    [Required(ErrorMessage = "O número de telefone é obrigatório.")]
    [Display(Name = "Número de Telefone", Description = "Número de telefone.")]
    [RegularExpression(@"^([1-9][0-9]{3}-[0-9]{4}|9[0-9]{4}-[0-9]{4})$", ErrorMessage = "O número de telefone é inválido.")]
    public string Value { get; } = value;

    public static implicit operator string(PhoneNumber? phoneNumber) 
        => phoneNumber is null
            ? string.Empty
            : phoneNumber.Value;

    public static implicit operator PhoneNumber(string phoneNumber) => new(phoneNumber);

    public override string ToString() => Value;
}