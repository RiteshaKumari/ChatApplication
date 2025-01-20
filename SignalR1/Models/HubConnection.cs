using System;
using System.Collections.Generic;

namespace SignalR1.Models;

public partial class HubConnection
{
    public string Id { get; set; } = null!;

    public string ConnectionId { get; set; } = null!;

    public int UserId { get; set; }

    public int? ChatId { get; set; }

    public DateTime? EnteryDate { get; set; }
}
