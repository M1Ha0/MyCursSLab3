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
    public class PassengerService : BaseService<Passenger>
    {
        private HttpClient httpClient;
        public PassengerService()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization",
               "Bearer " + RegisterUser.access_token);
        }
        public override async Task Add(Passenger obj)
        {
            try
            {
                JsonContent content = JsonContent.Create(obj);
                using var response = await httpClient.PostAsync("https://localhost:7034/api/Passenger", content);
                string responseText = await response.Content.ReadAsStringAsync();
                if (responseText != null)
                {
                    Passenger resp = JsonSerializer.Deserialize<Passenger>(responseText!)!;
                    if (resp == null) MessageBox.Show(responseText);
                }
            }
            catch { }
        }
        public override async Task Delete(Passenger obj)
        {
            using var response = await httpClient.DeleteAsync($"https://localhost:7034/api/Passenger/{obj.IdPassenger}");
        }
        public override async Task<List<Passenger>> GetAll()
        {
            return (await httpClient.GetFromJsonAsync<List<Passenger>>("https://localhost:7034/api/Passenger"))!;
        }
        public override Task<List<Passenger>> Search(string str)
        {
            throw new NotImplementedException();
        }
        public override async Task Update(Passenger obj)
        {
            try
            {
                JsonContent content = JsonContent.Create(obj);
                using var response = await httpClient.PutAsync($"https://localhost:7034/api/Passenger/{obj.IdPassenger}", content);
                string responseText = await response.Content.ReadAsStringAsync();
                if (responseText != null)
                {
                    Passenger resp = JsonSerializer.Deserialize<Passenger>(responseText!)!;
                    if (resp == null) MessageBox.Show(responseText);
                }

            }
            catch { }
        }
    }
}