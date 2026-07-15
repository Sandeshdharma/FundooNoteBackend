using Microsoft.EntityFrameworkCore;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Context
{
    public  class NotesContext : DbContext
    {

        public NotesContext(DbContextOptions<NotesContext> options) : base(options) { }

        public DbSet<Model.Entity.NotesModel> NotesTbl { get; set; }
        public DbSet<Model.Entity.UserModel> UserTbl { get; set; }
        public DbSet<CollaboratorModel> Collaborators { get; set; }

        public DbSet<LabelModel> Labels { get; set; }

        public DbSet<NoteLabelModel> NoteLabels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotesModel>()
                .HasOne(n => n.UserModel)
                .WithMany(u => u.Notes)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CollaboratorModel>()
    .HasOne(c => c.Note)
    .WithMany()
    .HasForeignKey(c => c.NoteId)
    .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CollaboratorModel>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);




            modelBuilder.Entity<LabelModel>()
    .HasOne(l => l.User)
    .WithMany(u => u.Labels)
    .HasForeignKey(l => l.UserId)
    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NoteLabelModel>()
     .HasOne(nl => nl.Note)
     .WithMany(n => n.NoteLabels)
     .HasForeignKey(nl => nl.NoteId)
     .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<NoteLabelModel>()
                .HasOne(nl => nl.Label)
                .WithMany(l => l.NoteLabels)
                .HasForeignKey(nl => nl.LabelId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }

}

