using Microsoft.EntityFrameworkCore;

namespace IntershipWebApp.Data
{
    public class DBObjects
    {
        public static void Initial(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
                dbContext.Database.Migrate();

                //dbContext.Seed();
            }
        }
    }
}
