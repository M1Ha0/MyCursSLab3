
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using MyCursS.Models;
using MyCursS.Services;

namespace MyCursS.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class FlightController : ControllerBase
    {
        private readonly FlightService flightService;
        private readonly ILogger<FlightController> logger;
        private readonly IMemoryCache cache;

        public FlightController(
            FlightService service,
            ILogger<FlightController> logger,
            IMemoryCache cache)
        {
            this.flightService = service;
            this.logger = logger;
            this.cache = cache;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Flight>>> GetAllFlights()
        {
            const string cacheKey = "all_flights";

            try
            {
                if (cache.TryGetValue(cacheKey, out IEnumerable<Flight>? flights))
                {
                    logger.LogInformation("Получен список рейсов из кеша");
                    return Ok(flights);
                }

                logger.LogInformation("Получение списка рейсов из базы данных");

                flights = await flightService.GetAll();

                cache.Set(cacheKey, flights, TimeSpan.FromMinutes(5));

                return Ok(flights);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при получении списка рейсов");
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Flight>> GetFlightById(int id)
        {
            string cacheKey = $"flight_{id}";

            try
            {
                if (cache.TryGetValue(cacheKey, out Flight? flight))
                {
                    logger.LogInformation("Рейс {FlightId} получен из кеша", id);
                    return Ok(flight);
                }

                logger.LogInformation("Получение рейса {FlightId} из базы данных", id);

                flight = await flightService.GetById(id);

                if (flight == null)
                {
                    logger.LogWarning("Рейс {FlightId} не найден", id);
                    return NotFound();
                }

                cache.Set(cacheKey, flight, TimeSpan.FromMinutes(5));

                return Ok(flight);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при получении рейса {FlightId}", id);
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<Flight>> CreateFlight(
            [FromBody] Flight flight)
        {
            try
            {
                await flightService.Create(flight);

                cache.Remove("all_flights");

                logger.LogInformation(
                    "Создан рейс {FlightId}", flight.NumberFlight);

                return CreatedAtAction(
                    nameof(GetFlightById),
                    new { id = flight.NumberFlight },
                    flight);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при создании рейса");
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Flight>> UpdateFlight(
            int id, [FromBody] Flight flight)
        {
            if (flight.NumberFlight != id)
            {
                logger.LogWarning(
                    "ID рейса в запросе не совпадает с ID в объекте: {FlightId}",
                    id);

                return BadRequest();
            }

            try
            {
                await flightService.Update(flight);

                cache.Remove("all_flights");
                cache.Remove($"flight_{id}");

                logger.LogInformation("Обновлён рейс {FlightId}", id);

                return Ok(flight);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при обновлении рейса {FlightId}", id);
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await flightService.Delete(id);

                cache.Remove("all_flights");
                cache.Remove($"flight_{id}");

                logger.LogInformation("Удалён рейс {FlightId}", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при удалении рейса {FlightId}", id);
                throw;
            }
        }
    }
}
