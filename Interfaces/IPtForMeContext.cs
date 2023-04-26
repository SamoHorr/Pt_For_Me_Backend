using Microsoft.EntityFrameworkCore;
using Pt_For_Me.Entities;
using Pt_For_Me.Models;

namespace Pt_For_Me.Interfaces
{
public interface IPtForMeContext
{
    //TABLES

    //user related tables
    DbSet<Table_User> Table_User { get; set; }
        DbSet<Table_Health> Table_Health { get; set; }
        DbSet<Table_Goal> Table_Goal { get; set; }
        DbSet<Table_UserPackage> Table_UserPackages { get; set; }
    DbSet<Table_PaymentInfo> Table_PaymentInfo { get; set; }

    //trainer related tables
        DbSet<Table_Trainer> Table_Trainer { get; set; }
        DbSet<Table_Specialty> Table_Specialty { get; set; }
        DbSet<Table_TrainerBlockedDay> Table_TrainerBlockedDays { get; set; }


    //Tables related to both
        DbSet<Table_Session> Table_Session { get; set; }
        DbSet<Table_Package> Table_Package { get; set; }

    //STORED PROCEDURES
    //for client display
    DbSet<GetAllTrainers_Result> GetAllTrainers_Result { get; set; }
    DbSet<GetTrainerByTrainerID_Result> GetTrainersByTrainerID_Result { get; set; }
        DbSet<GetAllApprovedTrainers_Result> GetAllApprovedTrainers_Result { get; set; }

        DbSet<GetAllPendingTrainers_Result> GetAllPendingTrainers_Result { get; set; }
}
}
