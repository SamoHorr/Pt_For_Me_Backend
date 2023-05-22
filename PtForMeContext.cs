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
        public virtual DbSet<Table_UserPackage> Table_UserPackage { get; set; }
        public virtual DbSet<Table_PaymentInfo> Table_PaymentInfo { get; set; }

        //trainer related tables
        public virtual DbSet<Table_Trainer> Table_Trainer { get; set; }
        public virtual DbSet<Table_Specialty> Table_Specialty { get; set; }
        public virtual DbSet<Table_TrainerBlockedDay> Table_TrainerBlockedDay { get; set; }


        //Tables related to both
        public virtual DbSet<Table_Session> Table_Session { get; set; }
        public virtual DbSet<Table_Package> Table_Package { get; set; }

        public virtual DbSet<Table_BookedSession> Table_BookedSession { get; set; }
        //models 
        public virtual DbSet<GetAllTrainers_Result> GetAllTrainers_Result { get; set; }
        public virtual DbSet<GetTrainerByTrainerID_Result> GetTrainersByTrainerID_Result { get; set; }
        public virtual DbSet<GetAllApprovedTrainers_Result> GetAllApprovedTrainers_Result { get; set; }
        public virtual DbSet<GetAllPendingTrainers_Result> GetAllPendingTrainers_Result { get; set; }
        public virtual DbSet<GetTrainerVerificationStatus_Result> GetTrainerVerificationStatus_Result { get; set; }
        public virtual DbSet<GetTrainerCountByExperience_Result> GetTrainerCountByExperience_Result { get; set; }
        public virtual DbSet<GetPackageByTrainerID_Result> GetPackageByTrainerID_Result { get; set; }
        public virtual DbSet<GetPackagesByTrainers_Result> GetPackagesByTrainers_Result { get; set; }
        public virtual DbSet<GetUserCountByAge_Result> GetUserCountByAge_Result { get; set; }
        public virtual DbSet<GetUserCountByGoals_Result> GetUserCountByGoals_Result { get; set; }
        public virtual DbSet<GetPendingReviews_Result> GetPendingReviews_Result { get; set; }
        public virtual DbSet<GetAcceptedReviewByTrainerID_Result> GetAcceptedReviewByTrainerID_Result { get; set; }
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
            modelBuilder.Entity<Table_Session>(entity =>
            {
                entity.HasKey(entity => entity.ID);
            });
            modelBuilder.Entity<Table_BookedSession>(entity =>
            {
                entity.HasKey(entity => entity.ID);
            });
            modelBuilder.Entity<Table_Health>(entity =>
            {
                entity.HasKey(entity => entity.ID);
            });
            modelBuilder.Entity<Table_Goal>(entity =>
            {
                entity.HasKey(entity => entity.ID);
            });
            modelBuilder.Entity<Table_Progress> (entity =>
            {
                entity.HasKey(entity => entity.ID);

            });
            modelBuilder.Entity<Table_TrainerBlockedDay>(entity =>
            {
                entity.HasKey(entity => entity.ID);

            });
            modelBuilder.Entity < Table_BookedSession>(entity =>
            {
                entity.HasKey(entity => entity.ID);

            });
            modelBuilder.Entity < Table_Specialty>(entity =>
            {
                entity.HasKey(entity => entity.ID);

            });
            //sp
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

                entity.Property(e => e.Trainer_Profile)
                .HasColumnName("ProfileURL");
            });

            modelBuilder.Entity<GetAllPendingTrainers_Result>(entity =>
            {
                entity.HasKey(entity => entity.TrainerID);
            });

            modelBuilder.Entity<GetTrainerVerificationStatus_Result>(entity =>
            {
                entity.HasKey(entity => entity.ID);
            });
            modelBuilder.Entity<GetTrainerCountByExperience_Result>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<GetPackageByTrainerID_Result>(entity =>
            {
                entity.HasKey(entity => entity.ID);
            });
            modelBuilder.Entity<GetPackageByTrainerID_Result>(entity =>
            {
                entity.HasKey(entity => entity.ID);
            });
            modelBuilder.Entity<GetUserCountByAge_Result>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<GetUserCountByGoals_Result>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<GetPendingReviews_Result>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<GetAcceptedReviewByTrainerID_Result>(entity =>
            {
                entity.HasNoKey();
            });
        }
    }
}
