using System.Text;
using ITBees.Interfaces.Platforms;
using Newtonsoft.Json;

namespace ITBees.ChatGpt;

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
        if (string.IsNullOrEmpty(_apiKey))
        {
            throw new Exception("ChatGptApiKey key and value must be set in config.json for the application to work");
        }
    }

    // Added optional parameter for model selection
    // The returned string is always the model's answer. Every failure (transport error, non-success
    // status code, unreadable or empty response) is signalled by ChatGptApiException, never by the return value.
    public async Task<string> AskChatGptAsync(string question, ChatGptModel model = ChatGptModel.Gpt4oMini)
    {
        HttpResponseMessage response;
        string responseString;
        try
        {
            var requestData = new
            {
                model = model.ToApiString(),
                messages = new[]
                {
                    new { role = "user", content = question }
                }
            };

            var jsonContent = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);

            response = await _httpClient.PostAsync(_apiUrl, content);
            responseString = await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            throw new ChatGptApiException($"API call failed: {ex.Message}", ex);
        }

        if (!response.IsSuccessStatusCode)
        {
            throw new ChatGptApiException(
                $"API call failed with status code: {response.StatusCode} \r\n{responseString}",
                response.StatusCode, responseString);
        }

        ChatGptResponse? responseJson;
        try
        {
            responseJson = JsonConvert.DeserializeObject<ChatGptResponse>(responseString);
        }
        catch (JsonException ex)
        {
            throw new ChatGptApiException($"API call failed: {ex.Message}", ex);
        }

        var answer = responseJson?.choices?.FirstOrDefault()?.message?.content;
        if (string.IsNullOrWhiteSpace(answer))
        {
            throw new ChatGptApiException("Empty response from API.", response.StatusCode, responseString);
        }

        return answer;
    }
}