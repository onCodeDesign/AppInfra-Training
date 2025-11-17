using Microsoft.EntityFrameworkCore;
using PersonsManagement.DataModel.Persons;

namespace PersonsManagement.Infrastructure;

public class PersonsDbContext : DbContext
{
    public PersonsDbContext(DbContextOptions<PersonsDbContext> options) : base(options)
    {
    }

    public DbSet<Person> Persons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new PersonBuilder());
    }
}
