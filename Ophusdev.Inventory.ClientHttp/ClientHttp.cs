using System.Globalization;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;

using Inventory.ClientHttp.Abstraction;
using Inventory.Shared;

namespace Inventory.ClientHttp
{
    public class ClientHttp(HttpClient httpClient) : IClientHttp
    {
        public async Task<string?> CreateRoomAsync(RoomDto room, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.PostAsync($"/Room/CreateRoom", JsonContent.Create(room), cancellationToken);
            return await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<string>(cancellationToken: cancellationToken);
        }

        public async Task<List<RoomDto>?> GetAllRoomAsync(CancellationToken cancellationToken = default)
        {
            var response = await httpClient.GetAsync($"/Room/ReadAllRooms", cancellationToken);
           
            return await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<List<RoomDto>>(cancellationToken: cancellationToken);
        }

        public async Task<RoomDto?> ReadRoomAsync(int idRoom, CancellationToken cancellationToken = default)
        {
            var queryString = QueryString.Create(new Dictionary<string, string?>() {
            { "idRoom", idRoom.ToString(CultureInfo.InvariantCulture) }
            });
            var response = await httpClient.GetAsync($"/Room/ReadRoom{queryString}", cancellationToken);
            return await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<RoomDto?>(cancellationToken: cancellationToken);
        }
    }
}
