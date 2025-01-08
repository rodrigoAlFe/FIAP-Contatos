using System.Text.Json.Serialization;
using FIAP.Domain.Requests.ModelRequests;

namespace FIAP.Domain.Models;


/// <summary>
///  Represents a contact with basic contact details.
/// </summary>
/// <param name="id">The identifier of the contact</param>
/// <param name="name">The name of the contact</param>
/// <param name="email">The email address of the contact</param>
/// <param name="phone">The phone number of the contact</param>
/// <param name="ddd">The area code (DDD) of the contact</param>
[method: JsonConstructor] public class Contact
( uint id
, string name
, string email
, string phone
, string ddd
, DateTime? createdAt = null
, DateTime? updatedAt = null)
{
    public uint Id { get; set; } = id;
    public string Name { get; private set; } = name;
    public string Email { get; private set; } = email;
    public string Phone { get; private set; } = phone;
    public string Ddd { get; private set; } = ddd;
    public DateTime? CreatedAt { get; set; } = createdAt;
    public DateTime? UpdatedAt { get; set; } = updatedAt;
    
    public Contact (ContactRequest request) :
        this ( request.Id
              , request.Name
              , request.Email
              , request.Phone
              , request.Ddd)
    {}
    
    public static Contact Create(ContactRequest request) 
        => new(request)
        {
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    
    public void Update(ContactRequest request)
    {
        Name = request.Name;
        Email = request.Email;
        Phone = request.Phone;
        Ddd = request.Ddd;
        UpdatedAt = DateTime.UtcNow;
    }
    public static implicit operator Contact(ContactRequest request)
        => new(request);
}