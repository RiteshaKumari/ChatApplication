using System;
using System.Collections.Generic;

namespace SignalR1.Models;

public partial class ChatLog
{
    public int ChatLogId { get; set; }

    public int ChatId { get; set; }

    public int SenderId { get; set; }

    public DateTime? Timestamp { get; set; }

    public bool? IsDeletedForEveryone { get; set; }

    public string? Typeofmessage { get; set; }

    public virtual ChatInfo Chat { get; set; } = null!;

    public virtual ICollection<DeletedMessage> DeletedMessages { get; set; } = new List<DeletedMessage>();

    public virtual User Sender { get; set; } = null!;
}
