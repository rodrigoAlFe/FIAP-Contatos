using System.ComponentModel.DataAnnotations;

namespace FIAP.Domain.Extensions.Objects.Contact;

/// <summary>
/// Represents an email address with validation attributes for required, display
/// name and description, and a regular expression to ensure a valid email format.
/// </summary>
/// <param name="value">The string of the email</param>
public class EmailAddress(string value)
{
    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [Display(Name = "E-mail", Description = "Endereço de e-mail.")]
    [RegularExpression(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$", ErrorMessage = "O endereço de e-mail é inválido.")]
    public string Value { get; } = value;

    public static implicit operator string(EmailAddress? email) 
        => email is null
            ? string.Empty
            : email.Value;
    public static implicit operator EmailAddress(string email) => new(email);
    public override string ToString() => Value;
}