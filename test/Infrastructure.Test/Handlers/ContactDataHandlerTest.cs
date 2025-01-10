using FIAP.Domain.Models;
using FIAP.Domain.Requests;
using FIAP.Domain.Requests.ModelRequests;
using FIAP.Infrastructure.Context;
using FIAP.Infrastructure.Handlers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Test.Handlers;

public class ContactDataHandlerTest : IDisposable
{
    private readonly MainDbContext _context;
    private readonly ContactDataHandler _dbHandler;

    public ContactDataHandlerTest()
    {
        var options = new DbContextOptionsBuilder<MainDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new MainDbContext(options);
        _dbHandler = new ContactDataHandler(_context);
        SeedData().GetAwaiter().GetResult();
    }

    public async void Dispose()
    {
        try
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();
            await _context.DisposeAsync();
            GC.SuppressFinalize(this);
        }
        catch (Exception e)
        {
            Assert.Fail(e.Message);
        }
    }

    private async Task SeedData()
    {
        await _context.Contacts.AddAsync(new Contact
            (0
            , "João das Coves"
            , "teste@company.com"
            , "91244-1234"
            , "21"
            , DateTime.UtcNow
            , DateTime.UtcNow));
        await _context.SaveChangesAsync();
    }

    [Theory]
    [InlineData("João das Coves", "teste@teste.com", "98765-4321", "21")]
    [InlineData("João das Coves", "teste@teste.com", "3456-7890", "21")]
    public async Task CreateAsync_WhenCalled_MustCreateContact
        (string name, string email, string phone, string ddd)
    {
        // Arrange
        var request = new ContactRequest(0, name, email, phone, ddd);
        
        // Act
        var response = await _dbHandler.CreateAsync(request);
        
        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsSuccess);
    }

    [Theory]
    [InlineData(1, "João das Coves", "teste@teste.com", "98765-4321", "21", "")]
    [InlineData(11, "João das Coves", "teste@teste.com", "98765-4321", "21", "Contato não encontrado!")]
    public async Task UpdateAsync_WhenCalled_MustUpdateContact_or_thrown_Error
        (uint id, string name, string email, string phone, string ddd, string expectedError)
    {
        try
        {
            // Arrange
            var request = new ContactRequest(id, name, email, phone, ddd);
            
            // Act 
            var response = await _dbHandler.UpdateAsync(request);
            
            // Assert
            Assert.NotNull(response.Data);
            Assert.True(response.IsSuccess);
            Assert.Equal(request.Email, response.Data!.First().Email);
        }
        catch (Exception ex)
        {
            Assert.NotNull(ex);
            Assert.Equal(expectedError, ex.Message);
        }
    }

    [Theory]
    [InlineData(1, "")]
    [InlineData(11, "Contato não encontrado!")]
    public async Task DeleteAsync_WhenCalled_Return_NoContentResponse_or_thrown_Error
        (uint id, string expectedError)
    {
        try
        {
            // Arrange
            var request = new Request(id);
            
            // Act
            var response = await _dbHandler.DeleteAsync(request);
            
            // Assert
            Assert.NotNull(response.Data);
            Assert.True(response.IsSuccess);
        }
        catch (Exception ex)
        {
            Assert.NotNull(ex);
            Assert.Equal(expectedError, ex.Message);
        }
    }

    [Theory]
    [InlineData(1, "")]
    [InlineData(11, "Contato não encontrado!")]
    public async Task GetByIdAsync_WhenCalled_ReturnContact_or_thrown_Error
        (uint id, string expectedError)
    {
        try
        {
            // Arrange
            var request = new Request(id);
            
            // Act
            var response = await _dbHandler.GetByIdAsync(request);
            
            // Assert
            Assert.NotNull(response.Data);
            Assert.True(response.IsSuccess);
        }
        catch (Exception ex)
        {
            Assert.NotNull(ex);
            Assert.Equal(expectedError, ex.Message);
        }
    }
    
    [Theory]
    [InlineData(0, 10)]
    public async Task GetAllAsync_WhenCalled_ReturnsContactResponseAsync
        (int page, int limit)
    {
        // Arrange
        var request = new PagedRequest(page, limit);

        // Act
        var response = await _dbHandler.GetAllAsync(request);

        // Assert
        Assert.NotNull(response.Data);
        Assert.True(response.IsSuccess);
    }
}