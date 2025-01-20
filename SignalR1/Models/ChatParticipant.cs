using System;
using System.Collections.Generic;

namespace SignalR1.Models;

public partial class ChatParticipant
{
    public int ParticipantId { get; set; }

    public int ChatId { get; set; }

    public int UserId { get; set; }

    public string Role { get; set; } = null!;

    public virtual ChatInfo Chat { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
