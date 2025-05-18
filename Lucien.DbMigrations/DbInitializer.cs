using Lucien.Domain.Cards.Entities;
using Lucien.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Lucien.DbMigrations
{
    public class DbInitializer
    {
        private readonly LucienDbContext _dbContext;

        public DbInitializer(LucienDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            try
            {
                if (!_dbContext.Cards.Any())
                {
                    //_dbContext.Cards.Add(new Card
                    //{
                    //    Id = Guid.NewGuid(),
                    //    Name = "Khalid",
                    //    Gender = 1,
                    //    DateOfBirth = DateTime.SpecifyKind(new DateTime(1997, 3, 10), DateTimeKind.Utc)
                    //});

                    //_dbContext.SaveChanges();
                }
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine(dbEx.InnerException?.Message);
                throw;
            }
        }

    }
}
