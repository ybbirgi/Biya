using Microsoft.EntityFrameworkCore;

namespace Biya.EntityFrameworkCore;

public interface IDbContextProvider<out TDbContext>
    where TDbContext : DbContext
{
    TDbContext GetDbContext();
}