using System.ComponentModel.DataAnnotations;

namespace FIAP.Domain.Extensions.Objects;


/// <summary>
/// Represents a name with validation for length and character composition.
/// Ensures the name has a length between 1 and 100 characters and only contains
/// alphanumeric characters, including accented letters, with spaces allowed between words.
/// </summary>
/// <param name="value">The string value of the name.</param>
public class Name(string value)
{
    [StringLength(100, ErrorMessage = "Deve ter entre 1 à 100 caracteres", MinimumLength = 1)]
    [RegularExpression(@"^[A-Za-zÀ-ÖØ-öø-ÿ0-9]+(\s[A-Za-zÀ-ÖØ-öø-ÿ0-9\-]+)*$"
        , ErrorMessage = "São válidos números, letras maiúsculas e minúsculas (incluindo acentuadas) e espaço somente entre as palavras")]
    public string Value { get; } = value;

    public static implicit operator string(Name? name) 
        => name is null
            ? string.Empty
            : name.Value;

    public static implicit operator Name(string value) => new(value);

    public override string ToString() => Value;
}