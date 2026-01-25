using System.Data.Common;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<TaskModel> Tasks { get; set;}
    public DbSet<StatusModel> Status { get; set;}
    public DbSet<UserModel> User { get; set;}

    public AppDbContext(DbContextOptions<AppDbContext>options) :base(options) {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskModel>(entity =>
        {
            entity.ToTable("Task", "Tasks");
            entity.HasKey(e => e.id);
            entity.Property(e => e.title).IsRequired().HasMaxLength(100);
            entity.Property(e => e.description).HasMaxLength(200);
            entity.HasOne(e => e.status)
            .WithMany()  // If StatusModel has no Tasks collection, use WithMany()
            .HasForeignKey(e => e.status_id)
            .OnDelete(DeleteBehavior.Cascade);
            entity.Navigation(e => e.status).AutoInclude();
        });

        modelBuilder.Entity<UserModel>(entity =>
        {
            entity.ToTable("User", "Users");
            entity.HasKey(e => e.id);
            entity.Property(e => e.username).IsRequired().HasMaxLength(20);
            entity.Property(e => e.email).IsRequired().HasMaxLength(50);
            entity.Property(e => e.password).IsRequired();
        });

        //auxiliars
        modelBuilder.Entity<StatusModel>(entity =>
        {
            entity.ToTable("Status", "Auxiliars");
            entity.HasKey(e => e.id);
            entity.Property(e => e.name).IsRequired().HasMaxLength(50);
        });

        base.OnModelCreating(modelBuilder);
    }
}