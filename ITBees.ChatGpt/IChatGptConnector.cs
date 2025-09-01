using ITBees.ChatGpt;

public interface IChatGptConnector
{
    Task<string> AskChatGptAsync(string question, ChatGptModel model = ChatGptModel.Gpt4oMini);
}