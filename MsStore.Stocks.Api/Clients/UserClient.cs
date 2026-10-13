using System.Text.Json.Serialization;
using MsStore.Stocks.Api.Domain.Enums;

namespace MsStore.Stocks.Api.Clients;

// Users.Api sends the name as "name", it is called FullName on this side
public record UserResponse(int Id, [property: JsonPropertyName("name")] string FullName, string? Email, UserType Type);

// As for right now I can have access to the another service, by using it's url, this is only one option
public class UserClient(HttpClient http)
{
    public async Task<UserResponse?> GetUserByIdAsync(int userId)
    {
        try
        {
            HttpResponseMessage res = await http.GetAsync($"api/users/{userId}");
            return res.IsSuccessStatusCode ? await res.Content.ReadFromJsonAsync<UserResponse>() : null;
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    public async Task<List<UserResponse>?> GetUsersAsync()
    {
        try
        {
            return await http.GetFromJsonAsync<List<UserResponse>>("api/users");
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }
}
