using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PlatformService.DTOs;
namespace PlatformService.SyncDataServices.Http
{
    public class CommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public CommandDataClient(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }
        async Task ICommandDataClient.SendPlatformToCommand(PlatformReadDto platformReadDto)
        {
            var httpClient = new StringContent(
                JsonSerializer.Serialize(platformReadDto),
                Encoding.UTF8,
                "application/json"
            );
            var response = await _client.PostAsync($"{_configuration["CommandService"]}/api/c/Platform", httpClient);
            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync post to CommandService was ok");
            }else
            {
                Console.WriteLine("--> Sync post to CommandService failed");
            }
        }
    }
}