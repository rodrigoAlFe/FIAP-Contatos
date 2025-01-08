using System.ComponentModel.DataAnnotations;
using FIAP.Domain.Requests;

namespace FIAP.Domain.Responses.TypesOfResponses;

/// <summary>
/// Represents a response for a paged request that resulted in a bad request (HTTP 400).
/// </summary>
/// <typeparam name="TData">The type of the data being returned in the response.</typeparam>
public class BadRequestPagedResponse<TData>
    : PagedResponse<TData>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestPagedResponse{TData}"/> class
    /// with error messages and an optional paged request.
    /// </summary>
    /// <param name="messages">The error messages associated with the bad request.</param>
    /// <param name="request">The paged request that resulted in the bad request (optional).</param>
    public BadRequestPagedResponse
    ( string[] messages
        , PagedRequest? request = null)
        : base
        (null
            , 0
            , request
            , messages
            , StatusConfiguration.BadRequestErrorCode)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestPagedResponse{TData}"/> class
    /// with error messages and an optional paged request.
    /// </summary>
    /// <param name="validationResults">The error messages associated with the bad request.</param>
    /// <param name="request">The paged request that resulted in the bad request (optional).</param>
    public BadRequestPagedResponse
    ( List<ValidationResult> validationResults
        , PagedRequest? request = null) 
        : base
        (null
            , 0
            , request
            , validationResults.Select(x => x.ErrorMessage!).ToArray()
            , StatusConfiguration.BadRequestErrorCode ) 
    { }
}