namespace NvsMarketFlow.Domain.Entities;

public class Notification
{
    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    public string Title { get; private set; }
    public string Message { get; private set; }
    public bool Read { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public Notification()
    {
    }

    public Notification(Guid userId, string title, string message)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty;");

        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be empty;");

        Id = Guid.NewGuid();
        UserId = userId;
        Title = title;
        Message = message;
        Read = false;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsRead()
    {
        if (Read)
            throw new InvalidOperationException("Notification is already marked as read.");

        Read = true;
    }
}