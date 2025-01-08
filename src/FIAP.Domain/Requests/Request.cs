using System.ComponentModel.DataAnnotations;
using FIAP.Domain.Extensions.Validations;

namespace FIAP.Domain.Requests;

public class Request (uint id = 0)
{
    [RegularExpression(@"^[0-9]\d*$", ErrorMessage = "São validos número inteiro positivo e o zero.")]
    public uint Id { get; set; } = id;
    
    public (bool isValid, List<ValidationResult> errors) Validate() => ValidationHelper.ValidateObject(this);
}