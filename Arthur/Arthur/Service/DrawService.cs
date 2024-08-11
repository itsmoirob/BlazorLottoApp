using Arthur.Models;
using System.Text.Json;

namespace Arthur.Service;

public class DrawService : IDrawService
{
    private static List<Draw> _draws = [];
    private IHttpClientFactory _httpClientFactory;
    private string _FilePath = "data/draws.json";

    public DrawService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory; 
    }

    public async Task<List<Draw>> GetDraws()
    {


        if(_draws.Count > 0)
        {
            return _draws;
        }

        HttpClient client = _httpClientFactory.CreateClient("lotto");
        string response = await client.GetStringAsync(_FilePath);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var jsonDrawResponse = JsonSerializer.Deserialize<DrawsResponse>(response, options);
        _draws =  jsonDrawResponse!.Draws;

        return _draws;
    }

    public Draw? GetDrawById(string Id)
    {
        return _draws.FirstOrDefault(d => d.Id == Id);
    }
}
