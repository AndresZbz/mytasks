using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using tasksApi;

public class AppDbContext : DbContext
{
    public DbSet<TaskModel> Tasks { get; set;}
    public DbSet<StatusModel> Status { get; set;}
    public DbSet<UserModel> User { get; set;}
    public DbSet<Role> Role { get; set;}
    public DbSet<Permission> Permission { get; set;}
    public DbSet<RolePermissions> RolePermissions { get; set;}


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


        //User + RBAC
        modelBuilder.Entity<UserModel>(entity =>
        {
            entity.ToTable("User", "Users");
            entity.HasKey(e => e.id);
            entity.Property(e => e.username).IsRequired().HasMaxLength(20);
            entity.Property(e => e.email).IsRequired().HasMaxLength(50);
            entity.Property(e => e.password).IsRequired();

            entity.HasOne(e => e.role)
            .WithMany()
            .HasForeignKey(e => e.role_id)
            .OnDelete(DeleteBehavior.Restrict);

        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role", "Rbac");
            entity.HasKey(e => e.id);
            entity.Property(e => e.name).IsRequired().HasMaxLength(20);

            entity.HasMany(e => e.role_permissions)
                .WithOne(e => e.role)
                .HasForeignKey(e => e.role_id) 
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Permission>(entity =>
        {
           entity.ToTable("Permission", "Rbac");
           entity.HasKey(e => e.id);
           entity.Property(e => e.name).IsRequired().HasMaxLength(20);
           entity.Property(e => e.codename).IsRequired().HasMaxLength(40);

            entity.HasMany(e => e.role_permissions)
                .WithOne(e => e.permission)
                .HasForeignKey(e => e.role_id) 
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<RolePermissions>(entity =>
        {
            entity.ToTable("RolePermissions", "Rbac");
            entity.HasKey(e => e.id);
            
            entity.HasOne(e => e.role)
                .WithMany(e => e.role_permissions)
                .HasForeignKey(e => e.role_id)
                .OnDelete(DeleteBehavior.Cascade);
            entity.Navigation(e => e.role);
                
            entity.HasOne(e => e.permission) 
                .WithMany(e => e.role_permissions)
                .HasForeignKey(e => e.permission_id)
                .OnDelete(DeleteBehavior.Cascade);
            entity.Navigation(e => e.permission);
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