using Microsoft.EntityFrameworkCore;

using AMNApi.Entities;

namespace AMNApi.Data;

public partial class AMNDbContext : DbContext
{
    public AMNDbContext()
    {
    }

    public AMNDbContext(DbContextOptions<AMNDbContext> options) : base(options)
    {
    }

    //Tables 
    public virtual DbSet<Address> Address { get; set; }

    public virtual DbSet<Appointment> Appointment { get; set; }

    public virtual DbSet<Consultory> Consultory { get; set; }

    public virtual DbSet<Doctor> Doctor { get; set; }

    public virtual DbSet<MapLocation> MapLocation { get; set; }

    public virtual DbSet<Patient> Patient { get; set; }

    public virtual DbSet<PatientAddress> PatientAddress { get; set; }

    public virtual DbSet<UserAccount> UserAccount { get; set; }

    public virtual DbSet<ActiveUserAccountDoctor> ActiveUserAccountDoctor { get; set; }

    public virtual DbSet<ActiveUserAccountPatient> ActiveUserAccountPatient { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            option => option.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
        );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AMNDbContext).Assembly);
    }
}
