using System.Text.Json.Serialization;
using FIAP.Domain.Models;

namespace FIAP.Domain.Responses.ModelsResponses;

/// <summary>
/// A response model representing a contact's information.
/// </summary>
/// <param name="Id">The unique identifier of the contact.</param>
/// <param name="Name">The full name of the contact.</param>
/// <param name="Email">The email address of the contact.</param>
/// <param name="Phone">The phone number of the contact.</param>
/// <param name="Ddd">The DDD (area code) of the contact's phone number.</param>
/// <param name="CreatedAt">The creation date and time of the contact record.</param>
/// <param name="UpdatedAt">The last update date and time of the contact record.</param>
[method: JsonConstructor] public record ContactResponse
( [property: JsonPropertyName("id")] uint Id
, [property: JsonPropertyName("nome")] string Name
, [property: JsonPropertyName("email")] string Email
, [property: JsonPropertyName("telefone")] string Phone
, [property: JsonPropertyName("ddd")] string Ddd
, [property: JsonPropertyName("CriadoEm")] DateTime? CreatedAt
, [property: JsonPropertyName("AtualizadoEm")] DateTime? UpdatedAt)
{
    public ContactResponse(Contact contact) :
        this ( contact.Id
             , contact.Name
             , contact.Email
             , contact.Phone
             , contact.Ddd
             , contact.CreatedAt
             , contact.UpdatedAt)
    { }
    
    public static implicit operator ContactResponse(Contact contact)
        => new(contact);
}