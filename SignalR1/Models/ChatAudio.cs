using System;
using System.Collections.Generic;

namespace SignalR1.Models;

public partial class ChatAudio
{
    public int Id { get; set; }

    public int ChatLogId { get; set; }

    public string? Audio { get; set; }
}
