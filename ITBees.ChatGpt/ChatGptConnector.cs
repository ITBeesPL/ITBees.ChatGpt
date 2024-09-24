using System.Text;
using ITBees.Interfaces.Platforms;
using Newtonsoft.Json;

public class ChatGptConnector : IChatGptConnector
{
    private readonly HttpClient _httpClient;
    private readonly IPlatformSettingsService _platformSettingsService;
    private readonly string _apiUrl = "https://api.openai.com/v1/chat/completions";
    private readonly string _apiKey;

    public ChatGptConnector(HttpClient httpClient, IPlatformSettingsService platformSettingsService)
    {
        _httpClient = httpClient;
        _platformSettingsService = platformSettingsService;
        _apiKey = platformSettingsService.GetSetting("ChatGptApiKey");
        if (_apiKey == null || string.IsNullOrEmpty(_apiKey))
        {
            throw new Exception("ChatGptApiKey key and value must be set in config.json for the application to work");
        }
    }

    public async Task<string> AskChatGptAsync(string question)
    {
        try
        {
            var requestData = new
            {
                model = "gpt-4",
                messages = new[]
                {
                    new { role = "user", content = question }
                }
            };

            var jsonContent = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _httpClient.PostAsync(_apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();

                var responseJson = JsonConvert.DeserializeObject<ChatGptResponse>(responseString);
                string reply = responseJson.choices[0].message.content;

                return reply;
            }
            else
            {
                var message = string.Empty;
                try
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    message = responseString;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                
                return $"API call failed with status code: {response.StatusCode} \r\n{message}";
            }
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"API call failed: {ex.Message}");
        }
    }
}