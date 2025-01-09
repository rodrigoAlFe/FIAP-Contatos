using System.Text.Json.Serialization;
using FIAP.Domain.Requests;

namespace FIAP.Domain.Responses;

/// <summary> Represents a paginated response. </summary>
/// <typeparam name="TData">The types of the entities</typeparam>
/// <param name="data">Response data</param>
/// <param name="totalCount">Total number of records</param>
/// <param name="messages">List of messages (optional)</param>
/// <param name="request">Uses <see cref="PagedRequest" /> to get (optional)</param>
/// <param name="code">Response status code. (optional)</param>
/// <remarks>Creates an instance of a paginated response</remarks>
[method: JsonConstructor] public class PagedResponse<TData>
( List<TData>? data
    , int totalCount
    , PagedRequest? request = null
    , string[]? messages = null
    , int code = StatusConfiguration.DefaultStatusCode )
    : Response<TData>(data, code)
{
    public int CurrentPage
        => request?.PageNumber ?? StatusConfiguration.DefaultPageNumber;

    public int PageSize
        => request?.PageSize ?? StatusConfiguration.DefaultPageSize;

    public int TotalPages
        => (int)Math.Ceiling(TotalCount / (double)PageSize);
    
    public string[] Messages { get; } = messages ?? [];

    [JsonIgnore] public int TotalCount { get; } = totalCount;
}