using System;
using System.Collections.Generic;

namespace SignalR1.Models;

public partial class ChatDoc
{
    public int Id { get; set; }

    public int ChatLogId { get; set; }

    public string? Doc { get; set; }
}
