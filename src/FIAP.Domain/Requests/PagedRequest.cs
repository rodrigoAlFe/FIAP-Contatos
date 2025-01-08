using System.ComponentModel.DataAnnotations;

namespace FIAP.Domain.Requests;

/// <summary>
/// Represents a request for data that is paged. It includes information
/// about the page number and page size for the request.
/// </summary>
/// <param name="pageNumber">The number of the page being requested. Defaults to 0.</param>
/// <param name="pageSize">The size of the page being requested. Defaults to 10.</param>
public class PagedRequest
( int pageNumber = StatusConfiguration.DefaultPageNumber
    , int pageSize = StatusConfiguration.DefaultPageSize ) : Request
{
    [RegularExpression(@"^[0-9]\d*$", ErrorMessage = "São validos número inteiro positivo.")]
    public int PageNumber { get; set; } = pageNumber;

    [RegularExpression(@"^[1-9]\d*0$", ErrorMessage = "Apenas múltiplos de 10 são permitidos.")]
    public int PageSize { get; set; } = pageSize;
}