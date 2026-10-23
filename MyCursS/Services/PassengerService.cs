using MyCursS.Models;
using Microsoft.EntityFrameworkCore;
using MyCursS.Services;

namespace MyCursS.Services
{
    public class PassengerService : IService<Passenger>
    {
        private readonly AviaIarnoContext db;
        public PassengerService(AviaIarnoContext _db) => this.db = _db;
        public async Task<IEnumerable<Passenger>> GetAll()
        {
            return await db.Passengers.ToListAsync();
        }
        public async Task<Passenger> GetById(int id)
        {
            return await db.Passengers.FindAsync(id);
        }
        public async Task Create(Passenger entity)
        {
            db.Passengers.Add(entity);
            await db.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            var pasen = await db.Passengers.FindAsync(id);
            if (pasen != null)
            {
                db.Passengers.Remove(pasen);
                await db.SaveChangesAsync();
            }
        }
        public async Task Update(Passenger entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            db.Passengers.Update(entity);
            await db.SaveChangesAsync();
        }
    }
}