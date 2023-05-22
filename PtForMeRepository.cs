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
            }
            catch
            {
                response.IsSuccess = false;
                return response;
            }
        }

        public ResponseModel<object> GetGeneralPackages()
        {
            ResponseModel<object> response = new ResponseModel<object>();
            try
            {
                response.Data = _context.Table_Package.ToList();
                response.IsSuccess = true;
                return response;
            }
            catch
            {
                response.IsSuccess = false;
                return response;
            }
        }
        public ResponseModel<object> GetTrainerBlockedTime(int TrainerID)
        {
            ResponseModel<object> response = new ResponseModel<object>();
            try
            {
                response.Data = _context.Table_Package.ToList();
                response.IsSuccess = true;
                return response;
            }catch
            {
                response.IsSuccess = false;
                return response;
            }
        }
        /*        public ResponseModel<int> GetCallerIDByUserID(int UserID)
                {
                    ResponseModel<int> response = new ResponseModel<int>();
                    try
                    {
                        var user = _context.Table_User.FirstOrDefault(u => u.ID == UserID);
                        if (user != null)
                        {
                            response.Data = user.CallerID;
                            response.Message = "User CallerID found";
                            response.IsSuccess = true;
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Message = "User not found";
                        }
                        return response;
                    }
                    catch
                    {
                        response.IsSuccess = false;
                        return response;
                    }
                }*/
        public ResponseModel<bool> CreateUser(string firstname, string lastname, DateTime DOB, string username, string password, string profileURL, string email, string DeviceToken)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            try
            {
                response.Data = false;
                response.IsSuccess = true;
                response.Message = "ERROR OCCURED - INVALID INFORMATION ";

                Table_User user = _context.Table_User.Where(u => u.Device_Token == DeviceToken).FirstOrDefault();
                //checking for similar usernames in db 
                if (_context.Table_User.Any(u => u.Username == username))
                {
                    response.Message = "Username already exists";
                    return response;
                }

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

        public ResponseModel<bool> CreateTrainer(string firstname, string lastname, string username, string password, string email, string profileUrl, string bio, int experience, int specialty, string DeviceToken, string imageCertificateURL, string imageCvURL)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            try
            {
                response.Data = false;
                response.IsSuccess = true;
                response.Message = "ERROR OCCURED - INVALID INFORMATION";

                Table_Trainer trainer = _context.Table_Trainer.Where(t => t.Device_Token == DeviceToken).FirstOrDefault();
                //checking for similar usernames in db 
                if (_context.Table_User.Any(u => u.Username == username))
                {
                    response.Message = "Username already exists";
                    return response;
                }

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
                        isAccepted = false,
                        Status = false,

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

                var user = _context.Table_User.FirstOrDefault(u => u.Username == username && u.Password == password);
                if (user != null)
                {
                    // generating a GUID token for authentication also after one hour session expires
                    var authToken = Guid.NewGuid();
                    var expirationTime = DateTime.UtcNow.AddHours(1);

                    //saving it in db
                    user.AuthToken = authToken;
                    user.Expiration_Date = expirationTime;
                    _context.SaveChanges();

                    response.Data = 1;
                    response.Message = "CLIENT LOGIN SUCCESSFUL";
                }
                else if (username == "admin" && password == "admin")
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
        public bool CheckAuthenticationTokenValidity(string authToken)
        {
            var user = _context.Table_User.FirstOrDefault(u => u.AuthToken.ToString() == authToken);
            if (user != null && user.Expiration_Date.HasValue && user.Expiration_Date.Value > DateTime.UtcNow)
            {
                //valid token
                return true;
            }

            //invalid token
            return false;
        }

        public ResponseModel<bool> LogoutWebsite(string authToken)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            try
            {
                response.Data = false;
                response.IsSuccess = false;
                response.Message = "LOGOUT FAILED";

                var user = _context.Table_User.FirstOrDefault(u => u.AuthToken.ToString() == authToken);
                if (user != null)
                {
                    // Invalidatting the token by setting it to null
                    user.AuthToken = null;
                    _context.SaveChanges();

                    response.Data = true;
                    response.IsSuccess = true;
                    response.Message = "LOGOUT SUCCESSFUL";
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
                var obj = _context.GetAllTrainers_Result.FromSqlInterpolated<GetAllTrainers_Result>($"EXECUTE SP_GetAllTrainers");

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
                                 Trainer = Group.Select(t => new { t.TrainerID, t.Firstname, t.Lastname, t.Bio, t.specialty, t.Experience })
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


        public ResponseModel<object> GetAllApprovedTrainers()
        {
            ResponseModel<object> response = new ResponseModel<object>();

            try
            {
                var obj = _context.GetAllApprovedTrainers_Result.FromSqlInterpolated<GetAllApprovedTrainers_Result>($"EXECUTE SP_GetAllApprovedTrainers");
                var result = from row in obj.AsEnumerable()
                             select new
                             {
                                 row.TrainerID,
                                 row.Firstname,
                                 row.Lastname,
                                 row.Bio,
                                 row.Experience,
                                 row.Specialty,
                                 row.Trainer_Profile,
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

        public ResponseModel<object> GetAllPendingTrainers()
        {
            ResponseModel<object> response = new ResponseModel<object>();

            try
            {
                var obj = _context.GetAllPendingTrainers_Result.FromSqlInterpolated<GetAllPendingTrainers_Result>($"EXECUTE SP_GetAllPendingTrainers");

                /* var result = from row in obj.AsEnumerable()
                              group row by (new
                              {
                                  row.TrainerID,
                                  row.Firstname,
                                  row.Lastname,
                                  row.Profile,
                                  row.Certificate,
                                  row.CV,
                              }) into Group
                              let row = Group.First()
                              select new
                              {
                                  TrainerInfo = Group.Key,
                              };
                 response.Data = result.ToList().Take(50);*/
                var result = from row in obj.AsEnumerable()
                             select new
                             {
                                 row.TrainerID,
                                 row.Firstname,
                                 row.Lastname,
                                 row.Profile,
                                 row.Certificate,
                                 row.CV
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
        public ResponseModel<object> GetTrainerVerificationStatus(int TrainerID)
        {
            ResponseModel<object> response = new ResponseModel<object>();
            try
            {
                var obj = _context.GetTrainerVerificationStatus_Result.FromSqlInterpolated<GetTrainerVerificationStatus_Result>($"Execute SP_GetTrainerVerificationStatus {TrainerID}");
                var result = from row in obj.AsEnumerable()
                             group row by (new
                             {
                                 row.ID,
                                 row.Firstname,
                                 row.Lastname,
                                 row?.isAccepted,
                                 row.Status,

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

        public ResponseModel<object> GetTrainerByTrainerID(int TrainerID)
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
                                 Trainer = Group.Select(t => new { t.TrainerID, t.Firstname, t.Lastname, t.Bio, t.specialty, t.Experience })
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

        public ResponseModel<bool> AcceptTrainer(int TrainerID)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            try
            {
                response.Data = false;
                response.IsSuccess = true;
                response.Message = "Unable to accept the trainer, try again later";

                Table_Trainer trainer = _context.Table_Trainer.Where(t => t.ID == TrainerID).FirstOrDefault();
                if (trainer != null)
                {


                    trainer.isAccepted = true;
                    trainer.Status = true;
                    _context.SaveChanges();

                    response.Message = "Trainer Accepted Successfully!";
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

        public ResponseModel<bool> DeclineTrainer(int TrainerID)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            try
            {
                response.Data = false;
                response.IsSuccess = true;
                response.Message = "Unable to decline the trainer, try again later";

                Table_Trainer trainer = _context.Table_Trainer.Where(t => t.ID == TrainerID).FirstOrDefault();
                if (trainer != null)
                {
                    if (trainer != null)
                    {
                        trainer.isAccepted = false;
                        trainer.Status = true;
                        _context.SaveChanges();

                        response.Message = "Trainer Declined Successfully!";
                        response.Data = true;
                    }
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

        public ResponseModel<object> GetTrainerCountByExperience()
        {
            ResponseModel<object> response = new ResponseModel<object>();
            response.Message = "Unable to retrieve trainer count";
            try
            {
                var obj = _context.GetTrainerCountByExperience_Result.FromSqlInterpolated<GetTrainerCountByExperience_Result>($"EXECUTE SP_GetTrainerCountByExperience");

                var result = from row in obj.AsEnumerable()
                             select new
                             {

                                 row.ExperienceCategory,
                                 row.TrainerCount,
                             };

                response.Data = result.ToList();
                response.IsSuccess = true;
                response.Message = "Retrieved trainer count successfully!";
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }

        public ResponseModel<object> GetPackageByTrainerID(int TrainerID)
        {
            ResponseModel<object> response = new ResponseModel<object>();
            response.Message = "Unable to get the package for this trainer";
            try
            {
                var obj = _context.GetPackageByTrainerID_Result.FromSqlInterpolated<GetPackageByTrainerID_Result>($"EXECUTE SP_GetPackageByTrainerID {TrainerID}");
                var result = from row in obj.AsEnumerable()
                             select new
                             {
                                 row.ID,
                                 row.Trainer,
                                 row.AverageRating,
                                 row.Pricing,
                                 row.PackageType,
                                 row.Bundle
                             };
                response.Data = result.ToList();
                response.IsSuccess = true;
                response.Message = "Retrieved the package for the trainer";
                return response;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }
        public ResponseModel<object> GetPackagesByTrainers()
        {
            ResponseModel<object> response = new ResponseModel<object>();
            response.Message = "Unable to get the package for this trainer";
            try
            {
                var obj = _context.GetPackagesByTrainers_Result.FromSqlInterpolated<GetPackagesByTrainers_Result>($"EXECUTE SP_GetPackagesByTrainers");
                var result = from row in obj.AsEnumerable()
                             select new
                             {
                                 row.ID,
                                 row.Trainer,
                                 row.AverageRating,
                                 row.Pricing,
                                 row.PackageType,
                                 row.Bundle
                             };
                response.Data = result.ToList();
                response.IsSuccess = true;
                response.Message = "Retrieved available packages for the trainers";
                return response;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }
        public ResponseModel<bool> AddUserPacakgesByUserID(int UserID, int PackageID, int TrainerID, int Bundle)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            try
            {
                response.Data = false;
                response.IsSuccess = true;
                response.Message = "ERROR OCCURED - INVALID INFORMATION ";

                //Table_UserPackage package = _context.Table_UserPackage.Where(p => p.UserID == UserID && p.TrainerID = TrainerID).FirstOrDefault();
                Table_UserPackage package = _context.Table_UserPackage.FirstOrDefault(p => p.UserID == UserID && p.TrainerID == TrainerID);


                //variable randomly generated for room id
                Random random = new Random();
                int min = 100000; // Minimum 6-digit number
                int max = 999999; // Maximum 6-digit number
                int randomNumber = random.Next(min, max + 1);

                //if null adding the info to the appropriate fields
                if (package == null)
                {
                    Table_UserPackage newPackage = new Table_UserPackage

                    {
                        UserID = UserID,
                        PackageID = PackageID,
                        Bundle = Bundle,
                        RoomID = randomNumber,
                    };
                    //saving the new user to the db
                    _context.Table_UserPackage.Add(newPackage);
                    _context.SaveChanges();

                    response.Message = "User Package Created Successfully";
                    response.Data = true;
                }
                else if (package != null)
                {
                    response.Message = "You already have a bought package with the trainer";
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
        public ResponseModel<bool> AddBookedSessionByUserID(int UserID, DateTime startTime, DateTime endTime)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();
            try
            {
                response.Data = false;
                response.IsSuccess = true;
                response.Message = "Cannot book a session currently";

                // to check if the user has any/or some hours left
                Table_UserPackage userPackage = _context.Table_UserPackage.FirstOrDefault(p => p.UserID == UserID);

                if (userPackage != null)
                {
                    var bundle = userPackage.Bundle;
                    var trainerID = userPackage.TrainerID;
                    var userPackageID = userPackage.ID;
                    var roomID = userPackage.RoomID;

                    // Check if the TrainerID exists in the Table_Trainer table
                    var trainerExists = _context.Table_Trainer.Any(t => t.ID == trainerID);

                    if (!trainerExists)
                    {
                        response.Message = "Invalid Trainer ID";
                        response.Data = false;
                        return response;
                    }

                    if (bundle != null && bundle != 0)
                    {
                        var trainerBlockedDays = _context.Table_TrainerBlockedDay.Where(t => t.TrainerID == trainerID).ToList();
                        bool isTimeSlotAvailable = true;

                        foreach (var trainerBlockedDay in trainerBlockedDays)
                        {
                            var blocked_startTime = trainerBlockedDay.Day_BlockedStart;
                            var blocked_endTime = trainerBlockedDay.Day_BlockedEnd;

                            if (blocked_startTime == startTime && blocked_endTime == endTime)
                            {
                                isTimeSlotAvailable = false;
                                break; // exit the loop since the time slot is already taken
                            }
                        }

                        if (isTimeSlotAvailable)
                        {
                            Table_BookedSession bookedSession = new Table_BookedSession()
                            {
                                UserPackageID = userPackageID,
                                UserID = UserID,
                                TrainerID = trainerID,
                                RoomID = roomID,
                                Start_Time = startTime,
                                End_Time = endTime
                            };

                            _context.Table_BookedSession.Add(bookedSession);
                            userPackage.Bundle = bundle - 1;

                            Table_TrainerBlockedDay newtrainerBlock = new Table_TrainerBlockedDay()
                            {
                                TrainerID = trainerID,
                                Day_BlockedStart = startTime,
                                Day_BlockedEnd = endTime
                            };
                            _context.Table_TrainerBlockedDay.Add(newtrainerBlock);

                            _context.SaveChanges();
                            response.Message = "User Session Booked Successfully";
                            response.Data = true;
                        }
                        else
                        {
                            response.Message = "That time slot is already taken";
                            response.Data = false;
                        }
                    }
                    else
                    {
                        response.Message = "You don't have any hours left";
                        response.Data = false;
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message += " Inner Exception: " + ex.InnerException.Message;
                return response;
            }
        }

        public ResponseModel<bool> AddBlockedTimeByTrainerID(int TrainerID, DateTime startTime, DateTime endTime)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();
            try
            {
                response.Data = false;
                response.IsSuccess = true;
                response.Message = "ERROR OCCURED - INVALID INFORMATION";


                Table_TrainerBlockedDay blockedDay = new Table_TrainerBlockedDay();
                blockedDay.TrainerID = TrainerID;
                blockedDay.Day_BlockedStart = startTime;
                blockedDay.Day_BlockedEnd = endTime;

                _context.Table_TrainerBlockedDay.Add(blockedDay);
                _context.SaveChanges();



                response.Message = "Trainer successfully blocked time !";
                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }

        }
        /*        public ResponseModel<bool> AddBookedSessionByUserID(int UserID ,DateTime startTime , DateTime endTime)
                {
                    ResponseModel<bool> response = new ResponseModel<bool>();
                    try
                    {
                        response.Data = false;
                        response.IsSuccess = true;
                        response.Message = "Cannot book a session currently";

                        //to check that the user has any/or some hours left
                        Table_UserPackage userPackage = _context.Table_UserPackage.Where(p => p.UserID == UserID).FirstOrDefault();
                        {


                            var bundle = _context.Table_UserPackage.Where(p => p.UserID == UserID).FirstOrDefault().Bundle;
                            //params from UserPackage
                            var trainerID = _context.Table_UserPackage.Where(p => p.UserID == UserID).FirstOrDefault().TrainerID;
                            var userPackageID = _context.Table_UserPackage.Where(p => p.UserID == UserID).FirstOrDefault().ID;
                            var roomID = _context.Table_UserPackage.Where(p => p.UserID == UserID).FirstOrDefault().RoomID;

                            if (bundle != null && bundle != 0)
                            {
                                var trainerBlockedDays = _context.Table_TrainerBlockedDay.Where(t => t.TrainerID == trainerID).ToList();
                                {
                                    //because we could have same trainer id with several different timing
                                    foreach (var trainerBlockedDay in trainerBlockedDays)
                                    {

                                        //need to check that the traine doesnt have anything blocked/booked off then
                                        var blocked_startTime = _context.Table_TrainerBlockedDay.Where(t => t.TrainerID == trainerID).FirstOrDefault().Day_BlockedStart;
                                        var blocked_endTime = _context.Table_TrainerBlockedDay.Where(t => t.TrainerID == trainerID).FirstOrDefault().Day_BlockedEnd;

                                        if (blocked_startTime != startTime && blocked_endTime != endTime)
                                        {
                                            Table_BookedSession bookedSession = new Table_BookedSession();
                                            {
                                                bookedSession.UserPackageID = userPackageID;
                                                bookedSession.UserID = UserID;
                                                bookedSession.TrainerID = trainerID;
                                                bookedSession.RoomID = roomID;
                                                bookedSession.Start_Time = startTime;
                                                bookedSession.End_Time = endTime;
                                            }
                                            //to save the booked session info
                                            _context.Table_BookedSession.Add(bookedSession);
                                            //to substract an hour from the package bought
                                            userPackage.Bundle = bundle - 1;



                                            //table trainer blocked needed so we can block hours in the table blocked hours 
                                            Table_TrainerBlockedDay newtrainerBlock = new Table_TrainerBlockedDay();
                                            newtrainerBlock.TrainerID = trainerID;
                                            newtrainerBlock.Day_BlockedStart = startTime;
                                            newtrainerBlock.Day_BlockedEnd = endTime;
                                            _context.Table_TrainerBlockedDay.Add(newtrainerBlock);

                                            _context.SaveChanges();
                                            response.Message = "User Session Booked Successfully";
                                            response.Data = true;
                                        }
                                        else
                                        {
                                            response.Message = "That time slot is already taken";
                                            response.Data = false;
                                        }
                                    }
                                }

                            }else
                            {
                                response.Message = "You dont have any hours left";
                                response.Data = false;
                            }

                        }

                        return response;

                    }
                    catch(Exception ex)
                    {
                        response.IsSuccess = false;
                        response.Message += " Inner Exception: " + ex.InnerException.Message;
                        return response;
                    }
                }*/
        public ResponseModel<object> GetSessionInfoByTrainerID(int TrainerID)
        {
            ResponseModel<object> response = new ResponseModel<object>();
            try
            {
                var obj = _context.GetSessionInfoByTrainerID_Result.FromSqlInterpolated<GetSessionInfoByTrainerID_Result>($"Execute SP_GetSessionInfoByTrainerID {TrainerID}");
                var result = from row in obj.AsEnumerable()
                             select new
                             {
                                 row.TrainerID,
                                 row.Client_Name,
                                 row.HealthRisk,
                                 row.Injury,
                                 row.RoomID,
                                 row.Start_Time,
                                 row.End_Time,
                             };
                response.Data = result.ToList();
                response.IsSuccess = true;
                response.Message = "Retrieved the session info for the trainer";
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }

        }
        public ResponseModel<object> GetSessionInfoByUserID(int UserID)
        {
            ResponseModel<object> response = new ResponseModel<object>();
            try
            {
                var obj = _context.GetSessionInfoByUserID_Result.FromSqlInterpolated<GetSessionInfoByUserID_Result>($"Execute SP_GetSessionInfoByUserID {UserID}");
                var result = from row in obj.AsEnumerable()
                             select new
                             {
                                 row.UserID,
                                 row.Trainer_Name,
                                 row.RoomID,
                                 row.Start_Time,
                                 row.End_Time,
                             };
                response.Data = result.ToList();
                response.IsSuccess = true;
                response.Message = "Retrieved the session info for the user";
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }

        }
        public ResponseModel<bool> AddClientHealthRiskOrInjury(int UserID, string healthRisk, string injury)
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
        public ResponseModel<object> GetUserCountByAge()
        {
            ResponseModel<object> response = new ResponseModel<object>();
            response.Message = "Unable to retrieve user count";
            try
            {
                var obj = _context.GetUserCountByAge_Result.FromSqlInterpolated<GetUserCountByAge_Result>($"EXECUTE SP_GetUserCountByAge");

                var result = from row in obj.AsEnumerable()
                             select new
                             {

                                 row.YearOfBirth,
                                 row.UserCount,
                             };

                response.Data = result.ToList();
                response.IsSuccess = true;
                response.Message = "Retrieved user count successfully!";
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }

        public ResponseModel<object> GetUserCountByGoal()
        {
            ResponseModel<object> response = new ResponseModel<object>();
            response.Message = "Unable to retrieve user count";
            try
            {
                var obj = _context.GetUserCountByGoals_Result.FromSqlInterpolated<GetUserCountByGoals_Result>($"EXECUTE SP_GetUserCountByGoals");

                var result = from row in obj.AsEnumerable()
                             select new
                             {
                                 row.Goal,
                                 row.UserCount,
                             };

                response.Data = result.ToList();
                response.IsSuccess = true;
                response.Message = "Retrieved user count successfully!";
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }
        public ResponseModel<object> GetPendingReviews()
        {
            ResponseModel<object> response = new ResponseModel<object>();
            response.Message = "Unable to retrieve the reviews";
            try
            {
                var obj = _context.GetPendingReviews_Result.FromSqlInterpolated<GetPendingReviews_Result>($"EXECUTE SP_GetPendingReviews");
                var result = from row in obj.AsEnumerable()
                             select new
                             {
                                 row.SessionID,
                                 row.Client_Name,
                                 row.Review,
                             };
                response.Data = result.ToList();
                response.IsSuccess = true;
                response.Message = "Retrieved reviews successfully!";
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }
        public ResponseModel<bool> AcceptReview(int SessionID)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            try
            {
                response.Data = false;
                response.IsSuccess = true;
                response.Message = "Unable to accept the review, try again later";

                Table_Session session = _context.Table_Session.Where(r => r.ID == SessionID).FirstOrDefault();
                if (session != null)
                {


                    session.isAccepted = true;
                    _context.SaveChanges();

                    response.Message = "Review Accepted Successfully!";
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

        public ResponseModel<bool> DeclineReview(int SessionID)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            try
            {
                response.Data = false;
                response.IsSuccess = true;
                response.Message = "Unable to accept the trainer, try again later";

                Table_Session session = _context.Table_Session.Where(r => r.ID == SessionID).FirstOrDefault();
                if (session != null)
                {


                    session.isAccepted = false;
                    _context.SaveChanges();

                    response.Message = "Review Declined Successfully!";
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

        public ResponseModel<object> GetAcceptedReviewByTrainerID(int TrainerID)
        {
            ResponseModel<object> response = new ResponseModel<object>();
            response.Message = "Unable to get the package for this trainer";
            try
            {
                var obj = _context.GetAcceptedReviewByTrainerID_Result.FromSqlInterpolated<GetAcceptedReviewByTrainerID_Result>($"EXECUTE SP_GetAcceptedReviewByTrainerID {TrainerID}");
                var result = from row in obj.AsEnumerable()
                             select new
                             {
                                 row.Client_Name,
                                 row.Review,
                                 row.Trainer_Name
                             };
                response.Data = result.ToList();
                response.IsSuccess = true;
                response.Message = "Retrieved the reviews for the trainer";
                return response;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }

        public ResponseModel<object> GetClientInfoByUserID(int UserID)
        {
            ResponseModel<object> response = new ResponseModel<object>();
            response.Message = "Unable to get theinformation for this user";
            try
            {
                var obj = _context.GetClientInfoByUserID_Result.FromSqlInterpolated<GetClientInfoByUserID_Result>($"EXECUTE SP_GetClientInfoByUserID {UserID}");
                var result = from row in obj.AsEnumerable()
                             select new
                             {
                                 row.UserID,
                                 row.Username,
                                 row.Password,
                                 row.Profile_Url,
                                 row.Goal,
                                 row.TargetWeight,
                                 row.Health_Risk,
                                 row.Injury
                             };
                response.Data = result.ToList();
                response.IsSuccess = true;
                response.Message = "Retrieved user information sucessfully";
                return response;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }
        public ResponseModel<object> GetTrainersBySpecialtyKeyword(string Keyword)
        {
            ResponseModel<object> response = new ResponseModel<object>();
            response.Message = "Unable to find the trainer with the specified keyword";
            try
            {
                var obj = _context.GetTrainersBySpecialtyKeyword_Result.FromSqlInterpolated<GetTrainersBySpecialtyKeyword_Result>($"Execute SP_GetTrainersBySpecialtyKeyword {Keyword}");
                var result = from row in obj.AsEnumerable()
                             select new
                             {
                                 row.TrainerID,
                                 row.Firstname,
                                 row.Lastname,
                                 row.Bio,
                                 row.Experience,
                                 row.ProfileURL,
                                 row.Specialty,
                             };
                response.Data = result.ToList();
                response.IsSuccess = true;
                response.Message = "Retrieved specified trainers";
                return response;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }
        public ResponseModel<string> GetDeviceIDFromTrainerID(int TrainerID)
        {
            ResponseModel<string> response = new ResponseModel<String>();
            try
            {

                response.Data = _context.Table_Trainer.Where(t => t.ID == TrainerID).FirstOrDefault().Device_Token;
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

        public ResponseModel<string> GetChannelNameIDFromTrainerID(int TrainerID)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            try
            {
                response.Data = _context.Table_Trainer.Where(t => t.ID == TrainerID).FirstOrDefault().FirstName + " channel";
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
        public ResponseModel<int> GetTrainerRating(int TrainerID)
        {
            ResponseModel<int> response = new ResponseModel<int>();
            response.Message = "Unable to get the trainer Rating ";
            try
            {
                var obj = _context.GetTrainerRating_Result.FromSqlInterpolated<GetTrainerRating_Result>($"EXECUTE SP_GetTrainerRating {TrainerID}");
                var result = from row in obj.AsEnumerable()
                             select new
                             {
                                 row.AverageRating,
                             };
                response.Data = result.FirstOrDefault()?.AverageRating ?? 0;
                response.IsSuccess = true;
                response.Message = "Retrieved user information sucessfully";
                return response;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }

        public ResponseModel<Guid> GetUserAuthToken(string username)
        {
            //we can use username since we dont allow same usernames
            ResponseModel<Guid> response = new ResponseModel<Guid>();
            response.Message = "Unable to get the user token";
            try
            { 
                var token = _context.Table_User.Where(u => u.Username == username).FirstOrDefault().AuthToken;
                //at the beginning authToken needs to be empty so ?GUID the ??Guid is to stop casting issue
                response.Data = token ?? Guid.Empty;
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
    }
}
