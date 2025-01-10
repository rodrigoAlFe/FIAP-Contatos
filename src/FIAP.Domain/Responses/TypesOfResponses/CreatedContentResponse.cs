namespace FIAP.Domain.Responses.TypesOfResponses;

/// <summary>
/// Represents a response for content that has been successfully created.
/// </summary>
/// <typeparam name="TData">The type of the content data.</typeparam>
/// <param name="data">The created content data.</param>
public class CreatedContentResponse<TData>
    ( TData data )
    : Response<TData>
        ([data], StatusConfiguration.CreatedStatusCode)
{ }