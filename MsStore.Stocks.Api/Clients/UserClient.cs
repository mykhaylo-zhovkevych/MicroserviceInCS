namespace MsStore.Stocks.Api.Clients;

public record UserStyledResponse(int Id, string FullName, string Email);

// As for right now I can have access to the another service, by using it's url, this is only one option
public class UserClient(HttpClient http)
{
    public async Task<UserStyledResponse?> GetUserByIdAsync(int userId)
    {
        HttpResponseMessage res = await http.GetAsync($"api/users/{userId}");
        if (!res.IsSuccessStatusCode)
        {
            return null;
        }
        return await res.Content.ReadFromJsonAsync<UserStyledResponse>();
    }
}
