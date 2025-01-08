using System.Text.Json.Serialization;

namespace FIAP.Domain.Responses;

/// <summary>Represents a standard response</summary>
/// <typeparam name="TData">The type of the entities</typeparam>
/// <param name="data">Response data</param>
/// <param name="code">Response status code</param>
[method: JsonConstructor]  public class Response<TData>
    ( List<TData>? data, int code = StatusConfiguration.DefaultStatusCode )
{
    [JsonIgnore] private readonly int _statusCode = code;

    public List<TData>? Data { get; } = data;

    public bool IsSuccess
        => _statusCode is >= 200 and < 300;
}