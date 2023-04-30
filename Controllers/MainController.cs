using Microsoft.AspNetCore.Mvc;
using Pt_For_Me.Interfaces;
using Pt_For_Me.Classes;
using Microsoft.AspNetCore.Cors;

namespace Pt_For_Me.Controllers
{

    [Route("APIS/[controller]")]
    public class MainController : ControllerBase
    {
        private readonly IPtForMeRepository _PtForMeRepository;
        private readonly PtForMeContext _context;
        public IWebHostEnvironment _enviroment; 
        public MainController(IPtForMeRepository PtForMeRepository, PtForMeContext context, IWebHostEnvironment enviroment)
        {
            _PtForMeRepository = PtForMeRepository;
            _context = context;
            _enviroment = enviroment;
        }

        [Route("GetUsers")]
        [HttpGet]
        public IActionResult GetUsers()
        {
            try
            {
                var result = _PtForMeRepository.GetUsers();
                return Ok(result);
            } catch (Exception e)
            {
                return Ok(e.Message);
            }

        }

        [Route("CreateNewClient")]
        [HttpPost]
        public IActionResult CreateNewClient([FromForm] User user , [FromForm(Name = "ProfilePic")] IFormFile imageProfile)

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
            var imagePath = "/api/profilesUser/" + newCertificateFileName;
            user.profileURL = imagePath;
           
            var result = _PtForMeRepository.CreateUser(user.FirstName, user.LastName, user.DOB, user.username, user.password, user.profileURL ,  user.Email, user.DeviceToken);
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
        //[EnableCors(AllowAllOrigins)]
        public IActionResult LoginWebsite([FromBody] User user)
        {
            var result = _PtForMeRepository.LoginWebsite(user.username, user.password);
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
        [Route ("CreateNewTrainer")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public IActionResult CreateNewTrainer([FromForm] Trainer trainer , [FromForm(Name = "Certificate")] IFormFile imageCertificate, [FromForm(Name = "CV")] IFormFile imageCV , [FromForm(Name = "ProfilePic")] IFormFile imageProfile)
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
            var imageProfilePath = "/api/profilesUser/" + newProfileFileName;
            var imageCVPath = "/api/profilesUser/" + newCvFileName;
            var imageCertificatePath = "/api/profilesUser/" + newCertificateFileName;
            //db
            trainer.certificateUrl = imageCertificatePath;
            trainer.cvURL = imageCVPath;
            trainer.profileURL = imageProfilePath;
            //(string firstname, string lastname, string username  , string password , string email, string bio, int experience, int specialty, string DeviceToken , string imageCertificateURL , string  imageCvURL)
            //using the function in repo to create user
            var result = _PtForMeRepository.CreateTrainer(trainer.firstname, trainer.lastname, trainer.username, trainer.password, trainer.email, trainer.profileURL ,  trainer.bio, trainer.experience, trainer.specialty, trainer.deviceToken, trainer.certificateUrl, trainer.cvURL);
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

        [Route ("GetAllApprovedTrainers")]
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
        public IActionResult GetTrainerVerificationStatus([FromBody] Trainer trainer )
        {
            var result = _PtForMeRepository.GetTrainerVerificationStatus(trainer.id);
            return Ok(result);
        }

        [Route("GetTrainerByTrainerID")]
        [HttpGet]
        public IActionResult GetTrainerByTrainerID([FromBody] Trainer trainer)
        {
            var result = _PtForMeRepository.GetTrainerByTrainerID(trainer.id);
            return Ok(result);
        }

        [Route("ClientAddHealthIssue")]
        [HttpPost]
        public IActionResult AddClientHealthRiskOrInjury([FromBody] Health health)
        {
            var result = _PtForMeRepository.AddClientHealthRiskOrInjury(health.userID, health.healthRisk, health.Injury);
            return Ok(result);
        }

        [Route("ClientAddGoal")]
        [HttpPost]
        public IActionResult AddClientGoal([FromBody] Goal goal)
        {
            var result = _PtForMeRepository.AddClientGoal(goal.userID , goal.description , goal.targetWeight , goal.date);
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
    }
}
