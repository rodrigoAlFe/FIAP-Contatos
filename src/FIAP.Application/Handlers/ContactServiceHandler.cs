using FIAP.Domain.Handlers;
using FIAP.Domain.Requests;
using FIAP.Domain.Requests.ModelRequests;
using FIAP.Domain.Responses;
using FIAP.Domain.Responses.ModelsResponses;
using FIAP.Domain.Responses.TypesOfResponses;

namespace FIAP.Application.Handlers;

public class ContactServiceHandler(IContactHandler contactDataHandler) : IContactHandler
{
    private readonly IContactHandler _contactDataHandler = 
        contactDataHandler ?? throw new ArgumentNullException(nameof(contactDataHandler));
    public async Task<Response<ContactResponse>> CreateAsync(ContactRequest request)
    {
        // Validate request 
        var (isValid, errors) = request.Validate();
        if (!isValid) return new BadRequestResponse<ContactResponse>(errors);

        try
        {
            return await _contactDataHandler.CreateAsync(request);
        }
        catch (Exception e)
        {
            return new InternalErrorResponse<ContactResponse>
                (["Erro ao criar o contato",  e.Message]);
        }
    }

    public async Task<Response<ContactResponse>> UpdateAsync(ContactRequest request)
    {
        // Validate request 
        var (isValid, errors) = request.Validate();
        if (!isValid) return new BadRequestResponse<ContactResponse>(errors);

        try
        {
            return await _contactDataHandler.UpdateAsync(request);
        }
        catch (Exception e)
        {
            return new InternalErrorResponse<ContactResponse>
                (["Erro ao criar o contato",  e.Message]);
        }
    }

    public async Task<Response<ContactResponse>> DeleteAsync(Request request)
    {
        // Validate request 
        var (isValid, errors) = request.Validate();
        if (!isValid) return new BadRequestResponse<ContactResponse>(errors);

        try
        {
            return await _contactDataHandler.DeleteAsync(request);
        }
        catch (Exception e)
        {
            return new InternalErrorResponse<ContactResponse>
                (["Erro ao criar o contato",  e.Message]);
        }
    }

    public async Task<Response<ContactResponse>> GetByIdAsync(Request request)
    {
        // Validate request 
        var (isValid, errors) = request.Validate();
        if (!isValid) return new BadRequestResponse<ContactResponse>(errors);

        try
        {
            return await _contactDataHandler.GetByIdAsync(request);
        }
        catch (Exception e)
        {
            return new InternalErrorResponse<ContactResponse>
                (["Erro ao criar o contato",  e.Message]);
        }
    }

    public async Task<PagedResponse<ContactResponse>> GetAllAsync(PagedRequest request)
    {
        // Validate request 
        var (isValid, errors) = request.Validate();
        if (!isValid) return new BadRequestPagedResponse<ContactResponse>(errors);

        try
        {
            return await _contactDataHandler.GetAllAsync(request);
        }
        catch (Exception e)
        {
            return new InternalPagedErrorResponse<ContactResponse>
                (["Erro ao criar o contato",  e.Message]);
        }
    }
}