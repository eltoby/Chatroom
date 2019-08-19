namespace ChatroomApi.Data
{
    using System;
    using ChatroomApi.Domain;
    using Microsoft.EntityFrameworkCore;

    public interface IChatContext: IDisposable
    {
        DbSet<Message> Messages { get; set; }

        DbSet<User> Users { get; set; }

        int SaveChanges();
    }
}