using Microsoft.EntityFrameworkCore;
using ProcessManager.Application.Models;

namespace ProcessManager.Infrastructure.Persistence;

public class CardDbContext: DbContext
{
    public CardDbContext(DbContextOptions<CardDbContext> options) : base(options)
    {
    }

    public DbSet<Card> Cards { get; set; }

}
