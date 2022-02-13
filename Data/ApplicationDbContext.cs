using EaTSWebAPI.Models;
using EaTSWebAPI.Models.Agencyes;
using EaTSWebAPI.Models.Equipments;
using EaTSWebAPI.Models.Repairs;
using Microsoft.EntityFrameworkCore;

namespace EaTSWebAPI.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Agency> Agency { get; set; }
        public DbSet<AgencyType> AgencyType { get; set; }

        public DbSet<EquipmentKind> EquipmentKind { get; set; }
        public DbSet<EquipmentClass> EquipmentClass { get; set; }
        public DbSet<EquipmentType> EquipmentType { get; set; }

        public DbSet<Repair> Repair { get; set; }
        public DbSet<RepairEvent> RepairEvent { get; set; }

        public DbSet<User> User { get; set; }
    }
}
