using FIAP.Domain.Models;
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
}