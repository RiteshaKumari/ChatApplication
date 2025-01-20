using System;
using System.Collections.Generic;

namespace SignalR1.Models;

public partial class ChatInfo
{
    public int ChatId { get; set; }

    public int CreatedBy { get; set; }

    public string ChatType { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<ChatLog> ChatLogs { get; set; } = new List<ChatLog>();

    public virtual ICollection<ChatParticipant> ChatParticipants { get; set; } = new List<ChatParticipant>();

    public virtual User CreatedByNavigation { get; set; } = null!;
}
