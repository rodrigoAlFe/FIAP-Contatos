using System.ComponentModel.DataAnnotations;

namespace FIAP.Domain.Responses.TypesOfResponses;

/// <summary>
/// Represents a response indicating a bad request with specific messages or validation errors.
/// </summary>
/// <typeparam name="TData">The type of the data included in the response.</typeparam>
public class BadRequestResponse<TData> : Response<TData>
{

    /// <summary>
    /// Represents a response indicating a bad request with specific messages.
    /// </summary>
    /// <param name="messages">Array of messages that describe the bad request error.</param>
    public BadRequestResponse(string[]? messages)
        : base(messages as List<TData>, StatusConfiguration.BadRequestErrorCode)
    { }

    /// <summary>
    /// Represents a response indicating a bad request with specific messages or validation errors.
    /// </summary>
    /// <param name="validationResults">Array of messages that describe the bad request error.</param>
    public BadRequestResponse(List<ValidationResult> validationResults)
        : base(validationResults as List<TData>, StatusConfiguration.BadRequestErrorCode)
    { }
}