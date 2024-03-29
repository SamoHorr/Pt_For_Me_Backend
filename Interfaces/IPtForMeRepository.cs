﻿using Pt_For_Me.Classes;
using Pt_For_Me.Entities;

namespace Pt_For_Me.Interfaces
{
    public interface IPtForMeRepository
    {
        ResponseModel<object> GetUsers();
        ResponseModel<bool> CreateUser(string firstname, string lastname, DateTime DOB, string username, string password, string profileURL, string email, string DeviceToken);
        ResponseModel<bool> CreateTrainer(string firstname, string lastname, string username, string password, string email, string profileURL, string bio, int experience, int specialty, string DeviceToken, string imageCertificateURL, string imageCvURL);
        ResponseModel<bool> LoginUser(string username, string password);
        ResponseModel<bool> LoginTrainer(string username, string password);
        ResponseModel<int> LoginWebsite(string username, string password);
        public bool CheckAuthenticationTokenValidity(string authToken);
        ResponseModel<bool> LogoutWebsite(string authToken);
        ResponseModel<bool> CheckUser(string DeviceID, string username, string password);
        ResponseModel<bool> CheckTrainer(string DeviceID, string username, string password);
        ResponseModel<object> GetAllTrainers();
        ResponseModel<object> GetAllApprovedTrainers();
        ResponseModel<object> GetAllPendingTrainers();
        ResponseModel<object> GetTrainerVerificationStatus(string deviceToken);
        ResponseModel<object> GetTrainerByTrainerID(int TrainerID);
        ResponseModel<bool> AddClientHealthRiskOrInjury(string Device_Token, string healthRisk, string injury);
        ResponseModel<bool> AddClientGoal(string Device_Token , string description, int targetWeight, DateTime date);
        ResponseModel<object> GetGeneralPackages();
        ResponseModel<bool> AcceptTrainer(int TrainerID);
        ResponseModel<bool> DeclineTrainer(int TrainerID);
        ResponseModel<object> GetTrainerCountByExperience();
        ResponseModel<object> GetPackageByTrainerID(int TrainerID);
        ResponseModel<object> GetPackagesByTrainers();
        ResponseModel<object> GetUserCountByAge();
        ResponseModel<object> GetUserCountByGoal();
        ResponseModel<string> GetDeviceIDFromTrainerID(int TrainerID);
        ResponseModel<string> GetChannelNameIDFromTrainerID(int TrainerID);
        ResponseModel<object> GetPendingReviews();
        ResponseModel<bool> DeclineReview(int SessionID);
        ResponseModel<bool> AcceptReview(int SessionID);
        ResponseModel<object> GetAcceptedReviewByTrainerID(int TrainerID);
        // ResponseModel<int> GetCallerIDByUserID(int UserID);
        ResponseModel<bool> AddUserPacakgesByUserID(string DeviceToken, int PackageID, int TrainerID, int Bundle);
        //ResponseModel<bool> AddBookedSessionByUserID(int UserID);
        ResponseModel<bool> AddBookedSessionByUserID(string Device_Token , DateTime startTime, DateTime endTime);
        // ResponseModel<object> GetSessionInfoByTrainerID (int TrainerID);
        ResponseModel<object> GetTrainersBySpecialtyKeyword(string Keyword);
        ResponseModel<object> GetClientInfoByUserID(string username);
        ResponseModel<bool> AddBlockedTimeByTrainerID(string deviceToken, DateTime startTime, DateTime endTime);
        ResponseModel<object> GetSessionInfoByTrainerID(string Device_Toke);
        ResponseModel<object> GetSessionInfoByUserID(string Device_Toke);
        ResponseModel <int> GetTrainerRating(int TrainerID);
        ResponseModel<object> GetTrainerBlockedTime(string Device_Token);
        ResponseModel<Guid> GetUserAuthToken(string username);
        ResponseModel<bool> AddSessionInfoByUserID(string UserID, int TrainerID, string Review, int Rating);
    }



}
