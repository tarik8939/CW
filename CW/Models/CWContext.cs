using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace CW.Models
{
    public partial class CWContext : DbContext
    {
        public CWContext()
        {
        }

        public CWContext(DbContextOptions<CWContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<BusType> BusTypes { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Route> Routes { get; set; }
        public virtual DbSet<RouteStop> RouteStops { get; set; }
        public virtual DbSet<Salary> Salaries { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<Transport> Transports { get; set; }
        public virtual DbSet<Worker> Workers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=CW;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Address>(entity =>
            {
                entity.Property(e => e.AddressId).HasColumnName("AddressID");

                entity.Property(e => e.AddressName).HasMaxLength(50);

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Addresses_Cities");
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.HasIndex(e => e.BrandName, "IX_Brands")
                    .IsUnique();

                entity.Property(e => e.BrandId).HasColumnName("BrandID");

                entity.Property(e => e.BrandName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<BusType>(entity =>
            {
                entity.HasIndex(e => e.TypeName, "IX_BusTypes")
                    .IsUnique();

                entity.Property(e => e.BusTypeId).HasColumnName("BusTypeID");

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.City1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("City");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.DeptId)
                    .HasName("PK_Department");

                entity.HasIndex(e => e.Address, "IX_Departments")
                    .IsUnique();

                entity.Property(e => e.DeptId).HasColumnName("DeptID");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Departments)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Departments_Сities");
            });

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.Property(e => e.PurchaseId).HasColumnName("PurchaseID");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.TicketId).HasColumnName("TicketID");

                entity.Property(e => e.TotalPrice).HasColumnType("money");

                entity.Property(e => e.WorkerId).HasColumnName("WorkerID");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK_Purchases_Clients");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Purchases_Departments");

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(d => d.TicketId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Purchases_Ticket");

                entity.HasOne(d => d.Worker)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(d => d.WorkerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Purchases_Workers1");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(e => e.Role1, "IX_Roles")
                    .IsUnique();

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.Role1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Role");
            });

            modelBuilder.Entity<Route>(entity =>
            {
                entity.Property(e => e.RouteId).HasColumnName("RouteID");

                entity.Property(e => e.NumberOfRoute)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.CityFromNavigation)
                    .WithMany(p => p.RouteCityFromNavigations)
                    .HasForeignKey(d => d.CityFrom)
                    .HasConstraintName("FK_Routes_Сities");

                entity.HasOne(d => d.CityToNavigation)
                    .WithMany(p => p.RouteCityToNavigations)
                    .HasForeignKey(d => d.CityTo)
                    .HasConstraintName("FK_Routes_Сities1");
            });

            modelBuilder.Entity<RouteStop>(entity =>
            {
                entity.HasKey(e => e.StopId);

                entity.ToTable("RouteStop");

                entity.Property(e => e.StopId).HasColumnName("StopID");

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.RouteId).HasColumnName("RouteID");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.RouteStops)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RouteStop_Сities");

                entity.HasOne(d => d.Route)
                    .WithMany(p => p.RouteStops)
                    .HasForeignKey(d => d.RouteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RouteStop_Routes");
            });

            modelBuilder.Entity<Salary>(entity =>
            {
                entity.ToTable("Salary");

                entity.Property(e => e.SalaryId).HasColumnName("SalaryID");

                entity.Property(e => e.Salary1).HasColumnName("Salary");

                entity.Property(e => e.WorkerId).HasColumnName("WorkerID");

                entity.HasOne(d => d.Worker)
                    .WithMany(p => p.Salaries)
                    .HasForeignKey(d => d.WorkerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Salary_Workers");
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("Schedule");

                entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");

                entity.Property(e => e.RouteId).HasColumnName("RouteID");

                entity.Property(e => e.TransportId).HasColumnName("TransportID");

                entity.Property(e => e.WorkerId).HasColumnName("WorkerID");

                entity.HasOne(d => d.Route)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.RouteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Schedule_Routes");

                entity.HasOne(d => d.Transport)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.TransportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Schedule_Models");

                entity.HasOne(d => d.Worker)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.WorkerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Schedule_Workers");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.Property(e => e.TicketId).HasColumnName("TicketID");

                entity.Property(e => e.ArrivalTime).HasColumnType("datetime");

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.HasOne(d => d.RouteStopFromNavigation)
                    .WithMany(p => p.TicketRouteStopFromNavigations)
                    .HasForeignKey(d => d.RouteStopFrom)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ticket_RouteStop");

                entity.HasOne(d => d.RouteStopToNavigation)
                    .WithMany(p => p.TicketRouteStopToNavigations)
                    .HasForeignKey(d => d.RouteStopTo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ticket_RouteStop1");

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.ScheduleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ticket_Schedule");
            });

            modelBuilder.Entity<Transport>(entity =>
            {
                entity.Property(e => e.TransportId).HasColumnName("TransportID");

                entity.Property(e => e.BrandId).HasColumnName("BrandID");

                entity.Property(e => e.BusTypeId).HasColumnName("BusTypeID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.ModelName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PricePerKm).HasColumnType("money");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Transports)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Models_Brand");

                entity.HasOne(d => d.BusType)
                    .WithMany(p => p.Transports)
                    .HasForeignKey(d => d.BusTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Models_BusType");
            });

            modelBuilder.Entity<Worker>(entity =>
            {
                entity.HasIndex(e => e.Email, "IX_Workers")
                    .IsUnique();

                entity.HasIndex(e => e.PhoneNumber, "IX_Workers_1")
                    .IsUnique();

                entity.Property(e => e.WorkerId).HasColumnName("WorkerID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Workers)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Workers_Roles");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
