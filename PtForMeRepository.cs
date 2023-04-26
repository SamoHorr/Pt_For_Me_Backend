using Pt_For_Me.Interfaces;
using Pt_For_Me.Classes;
using Pt_For_Me.Entities;
using Pt_For_Me.Models;
using Microsoft.EntityFrameworkCore;

namespace Pt_For_Me
{
    public class PtForMeRepository : IPtForMeRepository
    {
        private readonly PtForMeContext _context;
        public PtForMeRepository(PtForMeContext context)
        {
            _context = context;
        }

        public ResponseModel<object> GetUsers()
        {
            ResponseModel<object> response = new ResponseModel<object>();

            try
            {

                response.Data = _context.Table_User.ToList();
                response.IsSuccess = true;
                return response;
            } catch
            {
                response.IsSuccess = false;
                return response;
            }
        }

        public ResponseModel<bool> CreateUser(string firstname, string lastname, string DOB, string username, string password, string profileURL ,string email, string DeviceToken)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            try
            {
                response.Data = false;
                response.IsSuccess = true;
                response.Message = "ERROR OCCURED - INVALID INFORMATION ";

                Table_User user = _context.Table_User.Where(u => u.Device_Token == DeviceToken).FirstOrDefault();

                //if null adding the info to the appropriate fields
                if (user == null)
                {
                    Table_User newUser = new Table_User

                    {
                        Device_Token = DeviceToken,
                        Username = username,
                        Password = password,
                        Email = email,
                        FirstName = firstname,
                        LastName = lastname,
                        ProfileURL = profileURL,
                        DOB = DOB,
                    };
                    //saving the new user to the db
                    _context.Table_User.Add(newUser);
                    _context.SaveChanges();

                    response.Message = "User Created Successfully";
                    response.Data = true;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }

        public ResponseModel<bool> CreateTrainer(string firstname, string lastname, string username, string password, string email, string profileUrl ,string bio, int experience, int specialty, string DeviceToken, string imageCertificateURL, string imageCvURL)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            try
            {
                response.Data = false;
                response.IsSuccess = true;
                response.Message = "ERROR OCCURED - INVALID INFORMATION";


                Table_Trainer trainer = _context.Table_Trainer.Where(t => t.Device_Token == DeviceToken).FirstOrDefault();

                //if null adding the info to the appropriate fields
                if (trainer == null)
                {

                    Table_Trainer newTrainer = new Table_Trainer

                    {
                        Username = username,
                        Password = password,
                        FirstName = firstname,
                        LastName = lastname,
                        Email = email,
                        Bio = bio,
                        SpecialityID = specialty,
                        Experience = experience,
                        Device_Token = DeviceToken,
                        //the images
                        ProfileURL = profileUrl,
                        CertificateURL = imageCertificateURL,
                        CVURL = imageCvURL,

                    };
                    //saving the new user to the db
                    _context.Table_Trainer.Add(newTrainer);
                    _context.SaveChanges();

                    response.Message = "Trainer Created Successfully";
                    response.Data = true;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }

        }

        public ResponseModel<bool> LoginUser(string username, string password)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            try
            {
                response.Data = false;
                response.IsSuccess = true;
                response.Message = "INVALID INFORMATION";

                if (_context.Table_User.Where(u => u.Username == username && u.Password == password).FirstOrDefault() != null)
                {
                    response.Data = true;
                    response.Message = "CLIENT LOGIN SUCCESSFUL";
                }

                return response;


            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }

        public ResponseModel<bool> LoginTrainer(string username, string password)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            try
            {
                response.Data = false;
                response.IsSuccess = true;
                response.Message = "INVALID INFORMATION";

                if (_context.Table_Trainer.Where(t => t.Username == username && t.Password == password).FirstOrDefault() != null)
                {
                    response.Data = true;
                    response.Message = "TRAINER LOGIN SUCCSSEFUL";
                }

                return response;


            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }

        public ResponseModel<int> LoginWebsite(string username, string password)
        {
            ResponseModel<int> response = new ResponseModel<int>();

            try
            {
                response.Data = 0;
                response.IsSuccess = true;
                response.Message = "INVALID INFORMATION";

                if (_context.Table_User.Where(u => u.Username == username && u.Password == password).FirstOrDefault() != null)
                {
                    response.Data = 1;
                    response.Message = "CLIENT LOGIN SUCCESSFUL";
                }  else if ( username == "admin" && password == "admin")
                {
                    response.Data = 2;
                    response.Message = "ADMIN LOGIN SUCCESSFUL";
                }

                return response;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }
        public ResponseModel<bool> CheckUser(string DeviceID, string username, string password)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            try
            {
                response.Data = false;
                response.IsSuccess = true;
                response.Message = "CLIENT NOT FOUND";

                if (_context.Table_User.Where(u => u.Device_Token == DeviceID).FirstOrDefault() != null)
                {
                    response.Data = true;
                    response.Message = "CLIENT FOUND";
                }

                return response;


            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }

        public ResponseModel<bool> CheckTrainer(string DeviceID, string username, string password)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            try
            {
                response.Data = false;
                response.IsSuccess = true;
                response.Message = "CLIENT NOT FOUND";

                if (_context.Table_Trainer.Where(u => u.Device_Token == DeviceID).FirstOrDefault() != null)
                {
                    response.Data = true;
                    response.Message = "CLIENT FOUND";
                }

                return response;


            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }


        public ResponseModel<object> GetAllTrainers()
        {
            ResponseModel<object> response = new ResponseModel<object>();

            try
            {
                var obj = _context.GetAllTrainers_Result.FromSqlInterpolated<GetAllTrainers_Result>($"EXECUTE SP_GetAllTrainers ");

                var result = from row in obj.AsEnumerable()
                             group row by (new
                             {
                                 row.TrainerID,
                                 row.Firstname,
                                 row.Lastname,
                                 row.Bio,
                                 row.Experience,
                                 row.specialty,
                             }) into Group
                             let row = Group.First()
                             select new
                             {
                                 TrainerInfo = Group.Key,
                                 Trainer = Group.Select(t => new { t.TrainerID ,  t.Firstname, t.Lastname, t.Bio, t.specialty , t.Experience })
                             };
                response.Data = result.ToList().Take(50);
                response.IsSuccess = true;
                return response;
            }catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }

        }

        public ResponseModel<object> GetAllApprovedTrainers()
        {
            ResponseModel<object> response = new ResponseModel<object>();

            try
            {
                var obj = _context.GetAllTrainers_Result.FromSqlInterpolated<GetAllTrainers_Result>($"EXECUTE SP_GetAllApprovedTrainers ");

                var result = from row in obj.AsEnumerable()
                             group row by (new
                             {
                                 row.TrainerID,
                                 row.Firstname,
                                 row.Lastname,
                                 row.Bio,
                                 row.Experience,
                                 row.specialty,
                             }) into Group
                             let row = Group.First()
                             select new
                             {
                                 TrainerInfo = Group.Key,
                             };
                response.Data = result.ToList().Take(50);
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }

        }

        public  ResponseModel<object> GetTrainerByTrainerID(int TrainerID)
        {

            ResponseModel<object> response = new ResponseModel<object>();

            try
            {
                var obj = _context.GetAllTrainers_Result.FromSqlInterpolated<GetAllTrainers_Result>($"EXECUTE SP_GetTrainerByTrainerID {TrainerID}");

                var result = from row in obj.AsEnumerable()
                             group row by (new
                             {
                                 row.TrainerID,
                                 row.Firstname,
                                 row.Lastname,
                                 row.Bio,
                                 row.Experience,
                                 row.specialty,
                             }) into Group
                             let row = Group.First()
                             select new
                             {
                                 Trainer = Group.Select(t => new { t.TrainerID , t.Firstname, t.Lastname, t.Bio , t.specialty , t.Experience })
                             };
                response.Data = result.ToList();
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }

        public ResponseModel<bool> AddClientHealthRiskOrInjury(int UserID , string healthRisk , string injury)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            try
            {
                response.Data = false;
                response.IsSuccess = true;
                response.Message = "Unable to save the health risk added to this client. Try again later.";

                Table_Health goal = _context.Table_Health.Where(h => h.UserID == UserID).FirstOrDefault();
                if (goal == null)
                {

                    Table_Health newHealth = new Table_Health
                    {
                        UserID = UserID,
                        HealthRisk = healthRisk,
                        Injury = injury,

                    };
                    //saving the new user to the db
                    _context.Table_Health.Add(newHealth);
                    _context.SaveChanges();

                    response.Data = true;
                    response.Message = "Health risk and/or injury added successfully";
                }
                return response;


            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }

        public ResponseModel<bool> AddClientGoal(int UserID, string description, int targetWeight, DateTime date)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            try
            {
                response.Data = false;
                response.IsSuccess = true;
                response.Message = "Unable to save the goals to this client. Try again later.";

                Table_Goal goal = _context.Table_Goal.Where(g => g.UserID == UserID).FirstOrDefault();
                if (goal == null)
                {

                    Table_Goal newHealth = new Table_Goal
                    {
                        UserID = UserID,
                        Description = description,
                        TargetWeight = targetWeight,
                        Date = date,

                    };
                    //saving the  user to the db
                    _context.Table_Goal.Add(newHealth);
                    _context.SaveChanges();

                    response.Data = true;
                    response.Message = "Goal added successfully";
                }
                return response;


            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }
    }
}
