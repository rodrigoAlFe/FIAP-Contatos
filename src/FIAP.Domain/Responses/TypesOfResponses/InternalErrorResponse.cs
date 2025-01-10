namespace FIAP.Domain.Responses.TypesOfResponses;

/// <summary>
/// Represents an internal error response.
/// </summary>
/// <typeparam name="TData">The type of the data provided within the response.</typeparam>
public class InternalErrorResponse<TData> : Response<TData>
{
    private readonly string[] _messages;

    /// <summary>
    /// Initializes a new instance of the <see cref="InternalErrorResponse{TData}"/> class.
    /// </summary>
    /// <param name="messages">The array of error messages.</param>
    public InternalErrorResponse(string[] messages)
        : base(null, StatusConfiguration.DefaultErrorCode)
    {
        _messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }

    /// <summary>
    /// Gets the error messages.
    /// </summary>
    public IReadOnlyList<string> Messages => _messages;
}