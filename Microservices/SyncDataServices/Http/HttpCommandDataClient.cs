using Microservices.Dtos;
using System.Text;
using System.Text.Json;

namespace PlatformsService.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataCilent
    {
        private HttpClient _httpClient;
        private IConfiguration _configuration;

        public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task SendPlatfromToCommand(PlatformReadDto platformReadDto)
        {
            var httpContent = new StringContent(
                    JsonSerializer.Serialize(platformReadDto),
                    Encoding.UTF8,
                    "application/json"
                );

            var response = await _httpClient.PostAsync($"{_configuration["CommandService"]}", httpContent);

            if (response.IsSuccessStatusCode) Console.WriteLine("--> Sync POST to CommandService was OK!");
            else Console.WriteLine("--> Sync POST to CommandService was failure!");
        }
    }
}
