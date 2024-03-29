﻿using Microsoft.AspNetCore.Mvc;
using Pt_For_Me.Interfaces;
using Pt_For_Me.Classes;
using Microsoft.AspNetCore.Cors;
using Pt_For_Me.Entities;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text;

namespace Pt_For_Me.Controllers
{

    [Route("APIS/[controller]")]
    public class MainController : ControllerBase
    {
        private readonly IPtForMeRepository _PtForMeRepository;
        private readonly PtForMeContext _context;
        public IWebHostEnvironment _enviroment;
        private readonly IHttpClientFactory _httpClientFactory;
        public MainController(IPtForMeRepository PtForMeRepository, PtForMeContext context, IWebHostEnvironment enviroment, IHttpClientFactory httpClientFactory)
        {
            _PtForMeRepository = PtForMeRepository;
            _context = context;
            _enviroment = enviroment;
            _httpClientFactory = httpClientFactory;
        }

        [Route("GetUsers")]
        [HttpGet]
        public IActionResult GetUsers()
        {
            try
            {
                var result = _PtForMeRepository.GetUsers();
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }

        }

        [Route("GetGeneralPackages")]
        [HttpGet]
        public IActionResult GetGeneralPackages()
        {
            try
            {
                var result = _PtForMeRepository.GetGeneralPackages();
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [Route("GetTrainerBlockedTime")]
        [HttpPost]
        public IActionResult GetTrainerBlockedTime([FromBody] Trainer trainer)
        {
            try
            {
                var result = _PtForMeRepository.GetTrainerBlockedTime(trainer.deviceToken);
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [Route("GetUserAuthToken")]
        [HttpPost]
        public IActionResult GetUserAuthToken([FromBody] User user)
        {
            
                var result = _PtForMeRepository.GetUserAuthToken(user.username);
                return Ok(result);
            
        }

        [Route("CreateNewClient")]
        [HttpPost]
        public IActionResult CreateNewClient([FromForm] User user, [FromForm(Name = "ProfilePic")] IFormFile imageProfile)

        {
            //changing the profile pictures received names and creating the necessary paths
            FileInfo fiProfile = new FileInfo(imageProfile.FileName);
            var newCertificateFileName = "ProfilePic_" + DateTime.Now.TimeOfDay.Milliseconds + fiProfile.Extension;
            var rootPath = Path.Combine(_enviroment.ContentRootPath, "App_Data/UserProfile");
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }
            var pathProfile = Path.Combine("", _enviroment.ContentRootPath + "App_Data\\UserProfile\\" + newCertificateFileName);

            //to store the images received in the App_data folder inside the project
            var streamImageCertificate = new FileStream(pathProfile, FileMode.Create, FileAccess.Write);
            imageProfile.CopyTo(streamImageCertificate);

            //for the methode
            var imagePath = "/profilesUser/" + newCertificateFileName;
            user.profileURL = imagePath;

            var result = _PtForMeRepository.CreateUser(user.FirstName, user.LastName, user.DOB, user.username, user.password, user.profileURL, user.Email, user.DeviceToken);
            return Ok(result);
        }

        [Route("UserLogin")]
        [HttpPost]
        public IActionResult LoginUser([FromBody] User user)
        {
            var result = _PtForMeRepository.LoginUser(user.username, user.password);
            return Ok(result);
        }

        [Route("WebsiteLogin")]
        [HttpPost]
        [EnableCors("AllowAllOrigins")]
        public IActionResult LoginWebsite([FromBody] User user)
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:4000");
            Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
            var result = _PtForMeRepository.LoginWebsite(user.username, user.password);
            //  var result = _PtForMeRepository.LoginWebsite(user);
            return Ok(result);
        }

        [Route("CheckAuthenticationTokenValidity")]
        [HttpPost]
        [EnableCors("AllowAllOrigins")]
        public IActionResult CheckAuthenticationTokenValidity([FromBody] TokenRequest tokenRequest)
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:4000");
            Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");

            var isValid = _PtForMeRepository.CheckAuthenticationTokenValidity(tokenRequest.AuthToken);

            if (isValid)
            {
                return Ok(new { message = "Token is valid." });// return Ok("Token is valid.");
            }
            else
            {
                return Unauthorized(new { message = "Token is invalid or expired." });// return Unauthorized("Token is invalid or expired.");
            }
        }


        [Route("WebsiteLogout")]
        [HttpPost]
        public IActionResult LogoutWebsite([FromBody] string authToken)
        {
            var result = _PtForMeRepository.LogoutWebsite(authToken);
            return Ok(result);
        }
        //[Route("CreateNewTrainer")]
        // [HttpPost]
        //public IActionResult CreateNewTrainer([FromBody] Trainer trainer)
        //{
        // string firstname , string lastname , string DOB , string username , string password , string email , string DeviceToken
        //var result = _PtForMeRepository.CreateUser(user.FirstName, user.LastName, user.DOB, user.username, user.password, user.Email, user.DeviceToken);
        //var result = _PtForMeRepository.CreateTrainer(T)
        // var resu; 
        //return Ok(result);
        //}
        [Route("CreateNewTrainer")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public IActionResult CreateNewTrainer([FromForm] Trainer trainer, [FromForm(Name = "Certificate")] IFormFile imageCertificate, [FromForm(Name = "CV")] IFormFile imageCV, [FromForm(Name = "ProfilePic")] IFormFile imageProfile)
        {

            if (trainer == null)
            {
                return BadRequest();
            }

            //converting the images received into urls
            FileInfo fiCertificate = new FileInfo(imageCertificate.FileName);
            FileInfo fiCV = new FileInfo(imageCV.FileName);
            FileInfo fiProfile = new FileInfo(imageProfile.FileName);

            var newCertificateFileName = "Certificate_" + DateTime.Now.TimeOfDay.Milliseconds + fiCertificate.Extension;
            var newCvFileName = "CV_" + DateTime.Now.TimeOfDay.Milliseconds + fiCV.Extension;
            var newProfileFileName = "Profile_" + DateTime.Now.TimeOfDay.Milliseconds + fiProfile.Extension;

            var rootPath = Path.Combine(_enviroment.ContentRootPath, "App_Data");
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }
            var pathCertificate = Path.Combine("", _enviroment.ContentRootPath + "App_Data\\TrainerCertificate\\" + newCertificateFileName);
            var pathCV = Path.Combine("", _enviroment.ContentRootPath + "App_Data\\TrainerCV\\" + newCvFileName);
            var pathProfile = Path.Combine("", _enviroment.ContentRootPath + "App_Data\\TrainerProfile\\" + newProfileFileName);
            //to store the images received in the App_data folder inside the project
            var streamImageCertificate = new FileStream(pathCertificate, FileMode.Create, FileAccess.Write);
            var streamImageCv = new FileStream(pathCV, FileMode.Create, FileAccess.Write);
            var streamImageProfile = new FileStream(pathProfile, FileMode.Create, FileAccess.Write);
            imageCertificate.CopyTo(streamImageCertificate);
            imageCV.CopyTo(streamImageCv);
            imageProfile.CopyTo(streamImageProfile);

            //for the api methode
            var imageProfilePath = "/profileTrainer/" + newProfileFileName;
            var imageCVPath = "/cvTrainer/" + newCvFileName;
            var imageCertificatePath = "/certificateTrainer/" + newCertificateFileName;
            //db
            trainer.certificateUrl = imageCertificatePath;
            trainer.cvURL = imageCVPath;
            trainer.profileURL = imageProfilePath;
            //(string firstname, string lastname, string username  , string password , string email, string bio, int experience, int specialty, string DeviceToken , string imageCertificateURL , string  imageCvURL)
            //using the function in repo to create user
            var result = _PtForMeRepository.CreateTrainer(trainer.firstname, trainer.lastname, trainer.username, trainer.password, trainer.email, trainer.profileURL, trainer.bio, trainer.experience, trainer.specialty, trainer.deviceToken, trainer.certificateUrl, trainer.cvURL);
            return Ok(result);
        }

        [Route("TrainerLogin")]
        [HttpPost]
        public IActionResult LoginTrainer([FromBody] Trainer trainer)
        {
            var result = _PtForMeRepository.LoginTrainer(trainer.username, trainer.password);
            return Ok(result);
        }

        [Route("GetAllAvailableTrainers")]
        [HttpGet]
        public IActionResult GetAllTrainer()
        {
            var result = _PtForMeRepository.GetAllTrainers();
            return Ok(result);
        }

        [Route("GetAllApprovedTrainers")]
        [HttpGet]
        public IActionResult GetAllApprovedTrainers()
        {
            var result = _PtForMeRepository.GetAllApprovedTrainers();
            return Ok(result);
        }

        [Route("GetAllPendingTrainers")]
        [HttpGet]
        public IActionResult GetAllPendingTrainers()
        {
            var result = _PtForMeRepository.GetAllPendingTrainers();
            return Ok(result);
        }

        [Route("GetTrainerVerificationStatus")]
        [HttpGet]
        public IActionResult GetTrainerVerificationStatus([FromBody] Trainer trainer)
        {
            var result = _PtForMeRepository.GetTrainerVerificationStatus(trainer.deviceToken);
            return Ok(result);
        }

        [Route("AcceptTrainer")]
        [HttpPut]
        public IActionResult AcceptTrainer([FromBody] Trainer trainer)
        {
            var result = _PtForMeRepository.AcceptTrainer(trainer.id);
            return Ok(result);
        }


        [Route("DeclineTrainer")]
        [HttpPut]
        public IActionResult DeclineTrainer([FromBody] Trainer trainer)
        {
            var result = _PtForMeRepository.DeclineTrainer(trainer.id);
            return Ok(result);
        }

        [Route("GetPendingReviews")]
        [HttpGet]
        public IActionResult GetPendingReviews()
        {
            var result = _PtForMeRepository.GetPendingReviews();
            return Ok(result);
        }

        [Route("AcceptReview")]
        [HttpPut]
        public IActionResult AcceptReview([FromBody] Session session)
        {
            var result = _PtForMeRepository.AcceptReview(session.SessionID);
            return Ok(result);
        }

        [Route("DeclineReview")]
        [HttpPut]
        public IActionResult DeclineReview([FromBody] Session session)
        {
            var result = _PtForMeRepository.DeclineReview(session.SessionID);
            return Ok(result);
        }

        [Route("GetAcceptedReviewByTrainerID")]
        [HttpPost]
        public IActionResult GetAcceptedReviewByTrainerID([FromBody] Session session)
        {
            var result = _PtForMeRepository.GetAcceptedReviewByTrainerID(session.TrainerID);
            return Ok(result);
        }


        [Route("AddSessionInfoByUserID")]
        [HttpPost]
        public IActionResult AddSessionInfoByUserID([FromBody] Session session)
        {
            var result = _PtForMeRepository.AddSessionInfoByUserID(session.Device_Token , session.TrainerID , session.review , session.rating);
            return Ok(result);
        }

        [Route("GetTrainerByTrainerID")]
        [HttpPost]
        public IActionResult GetTrainerByTrainerID([FromBody] Trainer trainer)
        {
            var result = _PtForMeRepository.GetTrainerByTrainerID(trainer.id);
            return Ok(result);
        }

        [Route("ClientAddHealthIssue")]
        [HttpPost]
        public IActionResult AddClientHealthRiskOrInjury([FromBody] Health health)
        {
            var result = _PtForMeRepository.AddClientHealthRiskOrInjury(health.DeviceToken, health.healthRisk, health.Injury);
            return Ok(result);
        }

        [Route("ClientAddGoal")]
        [HttpPost]
        public IActionResult AddClientGoal([FromBody] Goal goal)
        {
            var result = _PtForMeRepository.AddClientGoal(goal.DeviceToken , goal.description, goal.targetWeight, goal.date);
            return Ok(result);
        }

        /*        [Route("GetCallerIDByUserID")]
                [HttpGet]
                public IActionResult GetCallerIDByUserID([FromBody] User user)
                {
                    var result = _PtForMeRepository.GetCallerIDByUserID(user.id);
                    return Ok(result);
                }*/

        [Route("GetTrainerCountByExperience")]
        [HttpGet]
        public IActionResult GetTrainerCountByExperience()
        {
            var result = _PtForMeRepository.GetTrainerCountByExperience();
            return Ok(result);
        }

        [Route("GetPackageByTrainerID")]
        [HttpPost]
        public IActionResult GetPackageByTrainerID([FromBody] Trainer trainer)
        {
            var result = _PtForMeRepository.GetPackageByTrainerID(trainer.id);
            return Ok(result);
        }

        [Route("GetPackagesByTrainers")]
        [HttpGet]
        public IActionResult GetPackagesByTrainers()
        {
            var result = _PtForMeRepository.GetPackagesByTrainers();
            return Ok(result);
        }

        [Route("GetUserCountByGoals")]
        [HttpGet]
        public IActionResult GetUserCountByGoal()
        {
            var result = _PtForMeRepository.GetUserCountByGoal();
            return Ok(result);
        }

        [Route("GetUserCountByAge")]
        [HttpGet]
        public IActionResult GetUserCountByAge()
        {
            var result = _PtForMeRepository.GetUserCountByAge();
            return Ok(result);
        }

        //api for images same logic just different folders (allows to call images as api solves issue of images filepath local to one pc)
        [Route("profilesUser/{fileName}")]
        [HttpGet]
        public IActionResult GetProfileImage(string fileName)
        {
            var filePath = Path.Combine(_enviroment.ContentRootPath, "App_Data/UserProfile", fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "image/jpeg");
        }


        [Route("profileTrainer/{fileName}")]
        [HttpGet]
        public IActionResult GetTrainerProfileImage(string fileName)
        {
            var filePath = Path.Combine(_enviroment.ContentRootPath, "App_Data/TrainerProfile", fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "image/jpeg");
        }

        [Route("cvTrainer/{fileName}")]
        [HttpGet]
        public IActionResult GetTrainerCVImage(string fileName)
        {
            var filePath = Path.Combine(_enviroment.ContentRootPath, "App_Data/TrainerCV", fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "image/jpeg");
        }

        [Route("certificateTrainer/{fileName}")]
        [HttpGet]
        public IActionResult GetTrainerDiplomaImage(string fileName)
        {
            var filePath = Path.Combine(_enviroment.ContentRootPath, "App_Data/TrainerCertificate", fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "image/jpeg");
        }

        [Route("AddUserPackageByUserID")]
        [HttpPost]
        public IActionResult AddUserPackageByUserID(UserPackage userPackage)
        {
            //int UserID , int PackageID , int TrainerID ,  DateTime startime , DateTime endtime , int Bundle
            var result = _PtForMeRepository.AddUserPacakgesByUserID(userPackage.DeviceToken, userPackage.PackageID, userPackage.TrainerID, userPackage.Bundle);
            return Ok(result);
        }
        [Route("AddBookedSessionByUserID")]
        [HttpPost]
        public IActionResult AddBookedSessionByUserID(BookedSession bookedSession)
        {
            //int UserID , int PackageID , int TrainerID ,  DateTime startime , DateTime endtime , int Bundle
            var result = _PtForMeRepository.AddBookedSessionByUserID(bookedSession.DeviceToken, bookedSession.startTime, bookedSession.endTime);
            return Ok(result);
        }

        [Route("GetTrainersBySpecialtyKeyword")]
        [HttpPost]
        public IActionResult GetTrainersBySpecialtyKeyword([FromBody] KeywordRequest request)
        {
            var result = _PtForMeRepository.GetTrainersBySpecialtyKeyword(request.Keyword);
            return Ok(result);
        }

        [Route("GetClientInfoByUserID")]
        [HttpPost]
        public IActionResult GetClientInfoByUserID([FromBody] User user)
        {
            var result = _PtForMeRepository.GetClientInfoByUserID(user.username);
            return Ok(result);
        }

        [Route("GetSessionInfoByTrainerID")]
        [HttpPost]
        public IActionResult GetSessionInfoByTrainerID([FromBody] Trainer trainer)
        {
            var result = _PtForMeRepository.GetSessionInfoByTrainerID(trainer.deviceToken);
            return Ok(result);
        }

        [Route("GetSessionInfoByUserID")]
        [HttpPost]
        public IActionResult GetSessionInfoByUserID([FromBody] User user)
        {
            var result = _PtForMeRepository.GetSessionInfoByUserID(user.DeviceToken);
            return Ok(result);
        }


        [Route("AddBlockedTimeByTrainerID")]
        [HttpPost]
        public IActionResult AddBlockedTimeByTrainerID([FromBody] TrainerBlock trainer)
        {
            var result = _PtForMeRepository.AddBlockedTimeByTrainerID(trainer.deviceToken, trainer.Day_BlockedStart, trainer.Day_BlockedEnd);
            return Ok(result);
        }

        [Route("GetTrainerRating")]
        [HttpPost]
        public IActionResult GetTrainerRating([FromBody] Trainer trainer)
        {
            var result = _PtForMeRepository.GetTrainerRating(trainer.id);
            return Ok(result);
        }
        //this api will allow us to generate tokens for trainer for agora that we are using for the videocalling (to be able to have a token for each trainer )

        /*[HttpPost("GenerateAgoraToken")]
        public async Task<IActionResult> GenerateAgoraToken([FromBody] Trainer trainer)
        {
        
            var agoraTokenEndpoint = "https://api.agora.io/dev/v1/projects/c3926878911c4246a0c0c9edf62a4bd0/rtctoken";
            var apiKey = "c3926878911c4246a0c0c9edf62a4bd0";
            var apiSecret = "3f1b453503aa4e42bdc61301bbb65259";
            var appId = "773e2d4f5d4f4129860676ead8ca1206";
            var appCertificate = "c959892711f74569adfe2ba7856191a8";
            var httpClient = new HttpClient();

            //needed parameters from repo function
            string uid = _PtForMeRepository.GetDeviceIDFromTrainerID(trainer.id).Data;
            string channelName = _PtForMeRepository.GetChannelNameIDFromTrainerID(trainer.id).Data;
           
            //the agoraTokenEndpoint provided by agora needs credentiels
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{apiKey}:{apiSecret}")));

            var request = new
            {
                app_id = appId,
                app_certificate = appCertificate,
                channel_name = channelName,
                user_account = uid,
                role = "publisher",
                privilege_expired_ts = (long)(DateTime.UtcNow.AddYears(2) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds
            };

            var response = await httpClient.PostAsJsonAsync(agoraTokenEndpoint, request);
            var token = await response.Content.ReadAsStringAsync();

            return Ok(token);
        }*/


    }

}

