using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SignalR1.Models;

public partial class MyChatDatabseContext : DbContext
{
    public MyChatDatabseContext()
    {
    }

    public MyChatDatabseContext(DbContextOptions<MyChatDatabseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChatAudio> ChatAudios { get; set; }

    public virtual DbSet<ChatDoc> ChatDocs { get; set; }

    public virtual DbSet<ChatImg> ChatImgs { get; set; }

    public virtual DbSet<ChatInfo> ChatInfos { get; set; }

    public virtual DbSet<ChatLog> ChatLogs { get; set; }

    public virtual DbSet<ChatMsg> ChatMsgs { get; set; }

    public virtual DbSet<ChatParticipant> ChatParticipants { get; set; }

    public virtual DbSet<ChatVideo> ChatVideos { get; set; }

    public virtual DbSet<DeletedMessage> DeletedMessages { get; set; }

    public virtual DbSet<HubConnection> HubConnections { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
       // => optionsBuilder.UseSqlServer("Server=RITESHA\\MSSQLSERVER01;Database=MyChatDatabse;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChatAudio>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ChatAudio");

            entity.Property(e => e.Audio)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ChatLogId).HasColumnName("ChatLogID");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
        });

        modelBuilder.Entity<ChatDoc>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ChatDoc");

            entity.Property(e => e.ChatLogId).HasColumnName("ChatLogID");
            entity.Property(e => e.Doc)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("DOC");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
        });

        modelBuilder.Entity<ChatImg>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ChatImg");

            entity.Property(e => e.ChatLogId).HasColumnName("ChatLogID");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Img)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ChatInfo>(entity =>
        {
            entity.HasKey(e => e.ChatId).HasName("PK__ChatInfo__A9FBE6262EFAF3C7");

            entity.ToTable("ChatInfo");

            entity.Property(e => e.ChatId).HasColumnName("ChatID");
            entity.Property(e => e.ChatType)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ChatInfos)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__ChatInfo__Create__656C112C");
        });

        modelBuilder.Entity<ChatLog>(entity =>
        {
            entity.HasKey(e => e.ChatLogId).HasName("PK__ChatLog__454604C49F956458");

            entity.ToTable("ChatLog");

            entity.Property(e => e.ChatLogId).HasColumnName("ChatLogID");
            entity.Property(e => e.ChatId).HasColumnName("ChatID");
            entity.Property(e => e.IsDeletedForEveryone).HasDefaultValue(false);
            entity.Property(e => e.SenderId).HasColumnName("SenderID");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Typeofmessage)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("typeofmessage");

            entity.HasOne(d => d.Chat).WithMany(p => p.ChatLogs)
                .HasForeignKey(d => d.ChatId)
                .HasConstraintName("FK__ChatLog__ChatID__75A278F5");

            entity.HasOne(d => d.Sender).WithMany(p => p.ChatLogs)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChatLog__SenderI__76969D2E");
        });

        modelBuilder.Entity<ChatMsg>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ChatMsg");

            entity.Property(e => e.ChatLogId).HasColumnName("ChatLogID");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Msg)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ChatParticipant>(entity =>
        {
            entity.HasKey(e => e.ParticipantId).HasName("PK__ChatPart__7227997E07487F73");

            entity.Property(e => e.ParticipantId).HasColumnName("ParticipantID");
            entity.Property(e => e.ChatId).HasColumnName("ChatID");
            entity.Property(e => e.Role)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Chat).WithMany(p => p.ChatParticipants)
                .HasForeignKey(d => d.ChatId)
                .HasConstraintName("FK__ChatParti__ChatI__693CA210");

            entity.HasOne(d => d.User).WithMany(p => p.ChatParticipants)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChatParti__UserI__6A30C649");
        });

        modelBuilder.Entity<ChatVideo>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ChatVideo");

            entity.Property(e => e.ChatLogId).HasColumnName("ChatLogID");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Video)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DeletedMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DeletedM__3214EC275687F4B6");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ChatLogId).HasColumnName("ChatLogID");
            entity.Property(e => e.DeletedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.ChatLog).WithMany(p => p.DeletedMessages)
                .HasForeignKey(d => d.ChatLogId)
                .HasConstraintName("FK__DeletedMe__ChatL__02084FDA");

            entity.HasOne(d => d.User).WithMany(p => p.DeletedMessages)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DeletedMe__UserI__02FC7413");
        });

        modelBuilder.Entity<HubConnection>(entity =>
        {
            entity.ToTable("HubConnection");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ChatId).HasColumnName("ChatID");
            entity.Property(e => e.ConnectionId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EnteryDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Student__32C52A791C18F666");

            entity.ToTable("Student");

            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Sts)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("Active");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Students)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Student__UserID__5BE2A6F2");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("PK__Teacher__EDF25944B4218817");

            entity.ToTable("Teacher");

            entity.Property(e => e.TeacherId).HasColumnName("TeacherID");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Sts)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("Active");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Teachers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Teacher__UserID__60A75C0F");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCAC25F79BCC");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__A9D105345853BB95").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
