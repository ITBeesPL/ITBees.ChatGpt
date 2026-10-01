using ITBees.ChatGpt;

public interface IChatGptConnector
{
    /// <summary>
    /// Asks the model a question and returns its answer.
    /// </summary>
    /// <exception cref="ChatGptApiException">The call failed or the API returned no answer.</exception>
    Task<string> AskChatGptAsync(string question, ChatGptModel model = ChatGptModel.Gpt4oMini);
}