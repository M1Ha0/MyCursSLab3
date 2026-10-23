using KursClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace KursClient.Services
{
    public class FlightService : BaseService<Flight>
    {
        private HttpClient httpClient;
        public FlightService()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization",
               "Bearer " + RegisterUser.access_token);
        }
        public override async Task Add(Flight obj)
        {
            try
            {
                JsonContent content = JsonContent.Create(obj);
                using var response = await httpClient.PostAsync("https://localhost:7034/api/Flight", content);
                string responseText = await response.Content.ReadAsStringAsync();
                if (responseText != null)
                {
                    Flight resp = JsonSerializer.Deserialize<Flight>(responseText!)!;
                    if (resp == null) MessageBox.Show(responseText);
                }
            }
            catch { }
        }
        public override async Task Delete(Flight obj)
        {
            using var response = await httpClient.DeleteAsync($"https://localhost:7034/api/Flight/{obj.NumberFlight}");
        }
        public override async Task<List<Flight>> GetAll()
        {
            return (await httpClient.GetFromJsonAsync<List<Flight>>("https://localhost:7034/api/Flight"))!;
        }
        public override Task<List<Flight>> Search(string str)
        {
            throw new NotImplementedException();
        }
        public override async Task Update(Flight obj)
        {
            try
            {
                JsonContent content = JsonContent.Create(obj);
                using var response = await httpClient.PutAsync($"https://localhost:7034/api/Flight/{obj.NumberFlight}", content);
                string responseText = await response.Content.ReadAsStringAsync();
                if (responseText != null)
                {
                    Flight resp = JsonSerializer.Deserialize<Flight>(responseText!)!;
                    if (resp == null) MessageBox.Show(responseText);
                }

            }
            catch { }
        }
    }
}