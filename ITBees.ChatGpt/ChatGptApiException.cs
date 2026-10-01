using System.Net;

namespace ITBees.ChatGpt;

/// <summary>
/// Thrown when a ChatGPT API call does not produce a usable answer: the request could not be sent,
/// the API answered with a non-success status code, or the response carried no answer text.
/// </summary>
public class ChatGptApiException : Exception
{
    public ChatGptApiException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }

    public ChatGptApiException(string message, HttpStatusCode statusCode, string? responseBody)
        : base(message)
    {
        StatusCode = statusCode;
        ResponseBody = responseBody;
    }

    /// <summary>
    /// HTTP status code returned by the API, null when no response was received.
    /// </summary>
    public HttpStatusCode? StatusCode { get; }

    /// <summary>
    /// Raw response body returned by the API, null when no response was received.
    /// </summary>
    public string? ResponseBody { get; }
}
