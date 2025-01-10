using System.Text.Json.Serialization;
using FIAP.Domain.Extensions.Objects;
using FIAP.Domain.Extensions.Objects.Contact;

namespace FIAP.Domain.Requests.ModelRequests;

/// <summary>
/// Represents a request model for Contact information.
/// This request includes details such as the ID, name, email, phone, and area code.
/// </summary>
/// <param name="id">The unique identifier for the contact.</param>
/// <param name="name">The name of the contact.</param>
/// <param name="email">The email address of the contact.</param>
/// <param name="phone">The phone number of the contact.</param>
/// <param name="ddd">The area code of the contact's phone number.</param>
[method: JsonConstructor] public class ContactRequest
( uint id, Name name, EmailAddress email, PhoneNumber phone, AreaCodeNumber ddd)
: Request(id)
{
    public Name Name { get; set; } = name;
    public EmailAddress Email { get; set; } = email;
    public PhoneNumber Phone { get; set; } = phone;
    public AreaCodeNumber Ddd { get; set; } = ddd;
    
    public ContactRequest(Models.Contact contact) :
        this ( contact.Id 
              , contact.Name
              , contact.Email
              , contact.Phone
              , contact.Ddd)
    {}
    
    public static implicit operator ContactRequest(Models.Contact request)
        => new(request);

}