using FIAP.Domain.Requests;
using FIAP.Domain.Requests.ModelRequests;
using FIAP.Domain.Responses;
using FIAP.Domain.Responses.ModelsResponses;

namespace FIAP.Domain.Handlers;

public interface IContactHandler
{
    public Task<Response<ContactResponse>> CreateAsync(ContactRequest request);
    public Task<Response<ContactResponse>> UpdateAsync(ContactRequest request);
    public Task<Response<ContactResponse>> DeleteAsync(Request request);
    public Task<Response<ContactResponse>> GetByIdAsync(Request request);
    public Task<PagedResponse<ContactResponse>> GetAllAsync(PagedRequest request);
}