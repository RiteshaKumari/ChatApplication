using System;
using System.Collections.Generic;

namespace SignalR1.Models;

public partial class ChatVideo
{
    public int Id { get; set; }

    public int ChatLogId { get; set; }

    public string? Video { get; set; }
}
