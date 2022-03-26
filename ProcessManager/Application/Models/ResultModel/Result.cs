namespace ProcessManager.Application.Models;

/// <summary>
/// Result
/// </summary>
public class Result
{
    /// <summary>
    /// Success
    /// </summary>
    public bool Success => string.IsNullOrEmpty(Message?.Text);

    /// <summary>
    /// Message
    /// </summary>
    public Message Message { get; init; }

    /// <summary>
    /// Messages
    /// </summary>
    public List<Message> Messages { get; init; } = new List<Message>();

    public Result()
    {
    }

    protected Result(string message, MessageType messageType)
    {
        Message = new Message()
        {
            Text = message,
            Type = messageType
        };
    }

    protected Result(string message, List<Message> messages, MessageType messageType)
    {
        Messages = messages;
        Message = new Message()
        {
            Text = message,
            Type = messageType
        };
    }

    /// <summary>
    /// Create success
    /// </summary>
    /// <returns>Result</returns>
    public static Result CreateSuccess()
    {
        return new Result();
    }

    /// <summary>
    /// Create failed
    /// </summary>
    /// <returns>Result</returns>
    public static Result CreateFailed(string message, MessageType messageType = MessageType.Error)
    {
        return new Result(message, messageType);
    }

    /// <summary>
    /// Create failed
    /// </summary>
    /// <returns>Result</returns>
    public static Result CreateFailed(string message, List<Message> messages, MessageType messageType = MessageType.Error)
    {
        return new Result(message, messages, messageType);
    }
}

/// <summary>
/// Result
/// </summary>
public class Result<T> : Result
{
    /// <summary>
    /// Data
    /// </summary>
    public T Data { get; init; }

    public Result()
    {
    }

    private Result(T data)
    {
        Data = data;
    }

    private Result(string message, MessageType messageType, T data = default) : base(message, messageType)
    {
        Data = data;
    }

    private Result(string message, List<Message> messages, MessageType messageType, T data = default) : base(message, messages, messageType)
    {
        Data = data;
    }

    /// <summary>
    /// Create success
    /// </summary>
    /// <returns>Result</returns>
    public static Result<T> CreateSuccess(T data = default)
    {
        return new Result<T>(data);
    }

    /// <summary>
    /// Create failed
    /// </summary>
    /// <returns>Result</returns>
    public static Result<T> CreateFailed(string message, MessageType messageType = MessageType.Error, T data = default)
    {
        return new Result<T>(message, messageType, data);
    }

    /// <summary>
    /// Create failed
    /// </summary>
    /// <returns>Result</returns>
    public static Result<T> CreateFailed(string message, List<Message> messages, MessageType messageType = MessageType.Error, T data = default)
    {
        return new Result<T>(message, messages, messageType, data);
    }
}