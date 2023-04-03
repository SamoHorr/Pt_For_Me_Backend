using Microsoft.AspNetCore.Mvc;
using Pt_For_Me.Interfaces;
using Pt_For_Me.Classes;

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
            user.profileURL = pathProfile;
           
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

            trainer.certificateUrl = pathCertificate;
            trainer.cvURL = pathCV;
            trainer.profileURL = pathProfile;
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
            var result = _PtForMeRepository.GetAllTrainer();
            return Ok(result);
        }

        [Route("GetTrainerByTrainerID")]
        [HttpGet]
        public IActionResult GetTrainerByTrainerID([FromBody] Trainer trainer)
        {
            var result = _PtForMeRepository.GetTrainerByTrainerID(trainer.id);
            return Ok(result);
        }
    }
}
