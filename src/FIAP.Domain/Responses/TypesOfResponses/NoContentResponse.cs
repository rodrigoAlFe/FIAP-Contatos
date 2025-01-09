namespace FIAP.Domain.Responses.TypesOfResponses;

/// <summary> Represents a successful no-content response.</summary>
/// <typeparam name="TData">The type of the response data.</typeparam>
/// <param name="data">The data that was deleted.</param>
/// <param name="messages">The response messages.</param>
public class NoContentResponse<TData>
( TData data )
    : Response<TData>
        ([data], StatusConfiguration.NoContentStatusCode)
{
}