using ContactsManager.Infrastructure.DBContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CRUDTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
 {
  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
   base.ConfigureWebHost(builder);

   builder.UseEnvironment("Test");

   builder.ConfigureServices(services => {
    var descripter = services.SingleOrDefault(temp => temp.ServiceType == typeof(DbContextOptions<AppDbContext>));

    if (descripter != null)
    {
     services.Remove(descripter);
    }
    services.AddDbContext<AppDbContext>(options =>
    {
     options.UseInMemoryDatabase("DatbaseForTesting");
    });
   });
  }
 }
}
