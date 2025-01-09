using FIAP.Domain.Handlers;
using FIAP.Domain.Models;
using FIAP.Domain.Requests;
using FIAP.Domain.Requests.ModelRequests;
using FIAP.Domain.Responses;
using FIAP.Domain.Responses.ModelsResponses;
using FIAP.Domain.Responses.TypesOfResponses;
using FIAP.Infrastructure.Context;
using FIAP.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace FIAP.Infrastructure.Handlers;

public class ContactDataHandler(MainDbContext context) : IContactHandler
{
    private readonly MainDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    public async Task<Response<ContactResponse>> CreateAsync(ContactRequest request)
    {
        // create entity
        var contact = Contact.Create(request);
        
        // save entity
        var savedContact = await _context.Contacts.AddAsync(contact);
        await _context.SaveChangesAsync();
        
        // return response
        return new Response<ContactResponse>([savedContact.Entity]);
    }

    public async Task<Response<ContactResponse>> UpdateAsync(ContactRequest request)
    {
        // validation of entities
        var contact = await _context.Contacts.FindEntityAsync(request.Id);
        
        // update entity
        contact.Update(request);
        
        // save entity
        await _context.SaveChangesAsync();
        
        // return response
        return new Response<ContactResponse>([contact]);
    }

    public async Task<Response<ContactResponse>> DeleteAsync(Request request)
    {
        // validation of entities
        var contact = await _context.Contacts.FindEntityAsync(request.Id);
        
        // delete entity
        _context.Contacts.Remove(contact);
        
        // save entity
        await _context.SaveChangesAsync();
        
        // return response
        return new NoContentResponse<ContactResponse>(contact);
    }

    public async Task<Response<ContactResponse>> GetByIdAsync(Request request)
    {
        // validation of entities
        var contact = await _context.Contacts.FindEntityAsync(request.Id);
        
        // return response
        return new Response<ContactResponse>([contact]);
    }

    public async Task<PagedResponse<ContactResponse>> GetAllAsync(PagedRequest request)
    {
        // get of entities
        var totalRecords = await _context.Contacts.CountAsync();
        var contacts = await _context.Contacts
            .AsNoTracking()
            .Skip(request.PageNumber * request.PageSize)
            .Take(request.PageSize)
            .Select(c => new ContactResponse(c))
            .ToListAsync();
        
        // return response
        return new PagedResponse<ContactResponse>(contacts, totalRecords, request);
    }
}