
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
    public class PassengerController : ControllerBase
    {
        private readonly PassengerService passengerService;
        private readonly ILogger<PassengerController> logger;
        private readonly IMemoryCache cache;

        public PassengerController(
            PassengerService service,
            ILogger<PassengerController> logger,
            IMemoryCache cache)
        {
            this.passengerService = service;
            this.logger = logger;
            this.cache = cache;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Passenger>>> GetAllPassengers()
        {
            const string cacheKey = "all_passengers";

            try
            {
                if (cache.TryGetValue(cacheKey, out IEnumerable<Passenger>? passengers))
                {
                    logger.LogInformation("Список пассажиров получен из кеша");
                    return Ok(passengers);
                }

                logger.LogInformation("Получение списка пассажиров из базы данных");

                passengers = await passengerService.GetAll();

                cache.Set(cacheKey, passengers, TimeSpan.FromMinutes(5));

                return Ok(passengers);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при получении списка пассажиров");
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Passenger>> GetPassengerById(int id)
        {
            string cacheKey = $"passenger_{id}";

            try
            {
                if (cache.TryGetValue(cacheKey, out Passenger? passenger))
                {
                    logger.LogInformation("Пассажир {PassengerId} получен из кеша", id);
                    return Ok(passenger);
                }

                logger.LogInformation(
                    "Получение пассажира {PassengerId} из базы данных", id);

                passenger = await passengerService.GetById(id);

                if (passenger == null)
                {
                    logger.LogWarning("Пассажир {PassengerId} не найден", id);
                    return NotFound();
                }

                cache.Set(cacheKey, passenger, TimeSpan.FromMinutes(5));

                return Ok(passenger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при получении пассажира {PassengerId}", id);
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<Passenger>> CreatePassenger(
            [FromBody] Passenger passenge)
        {
            try
            {
                await passengerService.Create(passenge);

                cache.Remove("all_passengers");

                logger.LogInformation(
                    "Создан пассажир {PassengerId}", passenge.IdPassenger);

                return CreatedAtAction(
                    nameof(GetPassengerById),
                    new { id = passenge.IdPassenger },
                    passenge);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при создании пассажира");
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Passenger>> UpdatePassenger(
            int id, [FromBody] Passenger passenge)
        {
            if (passenge.IdPassenger != id)
            {
                logger.LogWarning(
                    "ID пассажира в запросе не совпадает с ID объекта: {PassengerId}",
                    id);

                return BadRequest();
            }

            try
            {
                await passengerService.Update(passenge);

                cache.Remove("all_passengers");
                cache.Remove($"passenger_{id}");

                logger.LogInformation("Обновлён пассажир {PassengerId}", id);

                return Ok(passenge);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при обновлении пассажира {PassengerId}", id);
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await passengerService.Delete(id);

                cache.Remove("all_passengers");
                cache.Remove($"passenger_{id}");

                logger.LogInformation("Удалён пассажир {PassengerId}", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при удалении пассажира {PassengerId}", id);
                throw;
            }
        }
    }
}
