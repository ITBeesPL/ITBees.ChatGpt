namespace ITBees.ChatGpt;

public static class ChatGptModelExtensions
{
    // Map enum values to API string identifiers
    public static string ToApiString(this ChatGptModel model)
    {
        switch (model)
        {
            case ChatGptModel.Gpt4: return "gpt-4";
            case ChatGptModel.Gpt4o: return "gpt-4o";
            case ChatGptModel.Gpt4oMini: return "gpt-4o-mini";
            case ChatGptModel.Gpt4oNano: return "gpt-4o-nano";
            case ChatGptModel.Gpt41: return "gpt-4.1";
            case ChatGptModel.Gpt41Mini: return "gpt-4.1-mini";
            case ChatGptModel.Gpt41Nano: return "gpt-4.1-nano";
            case ChatGptModel.Gpt5: return "gpt-5";
            case ChatGptModel.Gpt5Mini: return "gpt-5-mini";
            case ChatGptModel.Gpt5Nano: return "gpt-5-nano";
            case ChatGptModel.O3: return "o3";
            case ChatGptModel.O4Mini: return "o4-mini";
            default: return "gpt-4o-mini"; // default safe choice
        }
    }
}