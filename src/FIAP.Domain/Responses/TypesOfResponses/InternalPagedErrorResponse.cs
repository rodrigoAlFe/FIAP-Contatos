using FIAP.Domain.Requests;

namespace FIAP.Domain.Responses.TypesOfResponses;

/// <summary>
/// Represents a response that contains error details along with paging information. 
/// This is used to encapsulate errors that occurred while processing a paged request.
/// </summary>
/// <param name="messages">An array of error messages describing what went wrong.</param>
/// <param name="request">The original paged request associated with the error. Defaults to null.</param>
/// <typeparam name="TData">The type of the data expected in the normal response.</typeparam>
public class InternalPagedErrorResponse<TData>(string[] messages, PagedRequest? request = null)
    : PagedResponse<TData>(null, 0, request, messages, StatusConfiguration.DefaultErrorCode);