using MyCursS.Models;
using Microsoft.EntityFrameworkCore;
using MyCursS.Services;

namespace MyCursS.Services
{
    public class FlightService : IService<Flight>
    {
        private readonly AviaIarnoContext db;
        public FlightService(AviaIarnoContext _db) => this.db = _db;
        public async Task<IEnumerable<Flight>> GetAll()
        {
            return await db.Flights.ToListAsync();
        }
        public async Task<Flight> GetById(int id)
        {
            return await db.Flights.FindAsync(id);
        }
        public async Task Create(Flight entity)
        {
            db.Flights.Add(entity);
            await db.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            var chit = await db.Flights.FindAsync(id);
            if (chit != null)
            {
                db.Flights.Remove(chit);
                await db.SaveChangesAsync();
            }
        }
        public async Task Update(Flight entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            db.Flights.Update(entity);
            await db.SaveChangesAsync();
        }
    }
}