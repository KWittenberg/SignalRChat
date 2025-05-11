namespace ChatContracts;

public class ChatMessage
{
    public string? FromUserId { get; set; } = string.Empty;

    public string? ToUserId { get; set; } = string.Empty;

    public string? Message { get; set; } = string.Empty;

    public bool Unread { get; set; } = true;
}