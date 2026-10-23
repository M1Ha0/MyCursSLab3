using Microsoft.EntityFrameworkCore;


namespace MyCursS.Models
{
    public static class SeedData
    {
        public static void SeedDatabase(AviaIarnoContext context)
        {
            context.Database.Migrate();
            if (context.Persons.Count() == 0)
            {
                Person user = new Person { Email = "admin@mail.ru", Password = "1234" };
                user.Password = AuthOptions.GetHash(user.Password);
                context.Persons.Add(user);
                context.SaveChanges();
            }
        }
    }
}
