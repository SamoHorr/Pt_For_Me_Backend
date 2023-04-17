using Pt_For_Me.Classes;
using Pt_For_Me.Entities;

namespace Pt_For_Me.Interfaces
{
    public interface IPtForMeRepository
    {
        ResponseModel<object> GetUsers();
        ResponseModel<bool> CreateUser(string firstname, string lastname, string DOB, string username, string password, string profileURL , string email, string DeviceToken);
        ResponseModel<bool> CreateTrainer(string firstname, string lastname, string username , string password , string email, string profileURL ,  string bio, int experience , int specialty , string DeviceToken, string imageCertificateURL, string imageCvURL);
        ResponseModel<bool> LoginUser(string username, string password);
        ResponseModel<bool> LoginTrainer(string username, string password);
        ResponseModel<bool> CheckUser(string DeviceID, string username, string password);
        ResponseModel<bool> CheckTrainer(string DeviceID, string username, string password);
        ResponseModel<object> GetAllTrainer();
        ResponseModel<object> GetTrainerByTrainerID(int TrainerID);
        ResponseModel<bool> AddClientHealthRiskOrInjury(int UserID, string healthRisk, string injury);
        ResponseModel<bool> AddClientGoal(int UserID , string description, int targetWeight, DateTime date); 
    }

    
}
