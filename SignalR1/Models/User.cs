using System;
using System.Collections.Generic;

namespace SignalR1.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public string? Role { get; set; }

    public virtual ICollection<ChatInfo> ChatInfos { get; set; } = new List<ChatInfo>();

    public virtual ICollection<ChatLog> ChatLogs { get; set; } = new List<ChatLog>();

    public virtual ICollection<ChatParticipant> ChatParticipants { get; set; } = new List<ChatParticipant>();

    public virtual ICollection<DeletedMessage> DeletedMessages { get; set; } = new List<DeletedMessage>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
