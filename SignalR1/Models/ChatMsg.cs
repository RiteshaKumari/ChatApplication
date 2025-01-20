using System;
using System.Collections.Generic;

namespace SignalR1.Models;

public partial class ChatMsg
{
    public int Id { get; set; }

    public int ChatLogId { get; set; }

    public string? Msg { get; set; }
}
