using Microsoft.EntityFrameworkCore;
using Pt_For_Me.Entities;
using Pt_For_Me.Interfaces;
using Pt_For_Me.Models;

namespace Pt_For_Me
{
    public class PtForMeContext : DbContext , IPtForMeContext
    {

    public bool Save()
        {
            return SaveChanges() > 0;
        }

        public PtForMeContext(DbContextOptions<PtForMeContext> options) : base(options) { }

        //TABLES
        //user related tables
        public virtual DbSet<Table_User> Table_User { get; set; }
        public virtual DbSet<Table_Health> Table_Health { get; set; }
        public virtual DbSet<Table_Goal> Table_Goal { get; set; }
        public virtual DbSet<Table_UserPackage> Table_UserPackages { get; set; }
        public virtual DbSet<Table_PaymentInfo> Table_PaymentInfo { get; set; }

        //trainer related tables
        public virtual DbSet<Table_Trainer> Table_Trainer { get; set; }
        public virtual DbSet<Table_Specialty> Table_Specialty { get; set; }
        public virtual DbSet<Table_TrainerBlockedDay> Table_TrainerBlockedDays { get; set; }


        //Tables related to both
        public virtual DbSet<Table_Session> Table_Session { get; set; }
        public virtual DbSet<Table_Package> Table_Package { get; set; }


        //models for client display
        public virtual DbSet<GetAllTrainers_Result> GetAllTrainers_Result { get; set; }
        public virtual DbSet<GetTrainerByTrainerID_Result> GetTrainersByTrainerID_Result { get; set; }
        public virtual DbSet<GetAllApprovedTrainers_Result> GetAllApprovedTrainers_Result { get; set; }
        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Table_User>(entity =>
            {
                entity.HasKey(e => e.ID);
            });

            modelBuilder.Entity<Table_Trainer>(entity =>
            {
                entity.HasKey(entity => entity.ID);
            });

            modelBuilder.Entity<GetAllTrainers_Result>(entity =>
            {
                entity.HasKey(entity => entity.TrainerID);
            });

            modelBuilder.Entity<GetTrainerByTrainerID_Result>(entity =>
            {
                entity.HasKey(entity => entity.TrainerID);
            });

            modelBuilder.Entity<GetAllApprovedTrainers_Result>(entity =>
            {
                entity.HasKey(entity =>entity.TrainerID);
            });
        }
    }
}
