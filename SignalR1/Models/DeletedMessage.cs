using System;
using System.Collections.Generic;

namespace SignalR1.Models;

public partial class DeletedMessage
{
    public int Id { get; set; }

    public int ChatLogId { get; set; }

    public int UserId { get; set; }

    public DateTime? DeletedDate { get; set; }

    public virtual ChatLog ChatLog { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
