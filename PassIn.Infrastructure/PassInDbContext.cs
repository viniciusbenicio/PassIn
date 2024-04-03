﻿using Microsoft.EntityFrameworkCore;
using PassIn.Infrastructure.Entites;

namespace PassIn.Infrastructure
{
    public class PassInDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Attendee> Attendees { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=D:\\Projetos\\Pessoal\\PassInDb.db");
        }
    }
}
