using System;
using System.Collections.Generic;

namespace SignalR1.Models;

public partial class ChatImg
{
    public int Id { get; set; }

    public int ChatLogId { get; set; }

    public string? Img { get; set; }
}
