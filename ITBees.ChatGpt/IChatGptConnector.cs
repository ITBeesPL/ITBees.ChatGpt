public interface IChatGptConnector
{
    Task<string> AskChatGptAsync(string question);
}