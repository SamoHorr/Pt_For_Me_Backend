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
        public IActionResult CreateNewClient([FromBody] User user)
        {
            // string firstname , string lastname , string DOB , string username , string password , string email , string DeviceToken
            var result = _PtForMeRepository.CreateUser(user.FirstName, user.LastName, user.DOB, user.username, user.password, user.Email, user.DeviceToken);
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
        public IActionResult CreateNewTrainer([FromForm] Trainer trainer , [FromForm(Name = "Certificate")] IFormFile imageCertificate, [FromForm(Name = "CV")] IFormFile imageCV)
        {

            if (trainer == null)
            {
                return BadRequest();
            }

            //cnverting the images received into urls
            FileInfo fiCertificate = new FileInfo(imageCertificate.FileName);
            FileInfo fiCV = new FileInfo(imageCV.FileName);

            var newCertificateFileName = "Certificate_" + DateTime.Now.TimeOfDay.Milliseconds + fiCertificate.Extension;
            var newCvFileName = "CV" + DateTime.Now.TimeOfDay.Milliseconds + fiCV.Extension;
            var rootPath = Path.Combine(_enviroment.ContentRootPath, "App_Data");
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }
            var pathCertificate = Path.Combine("", _enviroment.ContentRootPath + "App_Data\\" + newCertificateFileName);
            var pathCV = Path.Combine("", _enviroment.ContentRootPath + "App_Data\\" + newCvFileName);
            //to store the images received in the App_data folder inside the project
            var streamImageCertificate = new FileStream(pathCertificate, FileMode.Create, FileAccess.Write);
            var streamImageCv = new FileStream(pathCV, FileMode.Create, FileAccess.Write);
            imageCertificate.CopyTo(streamImageCertificate);
            imageCV.CopyTo(streamImageCv);

            trainer.certificateUrl = pathCertificate;
            trainer.cvURL = pathCV;

            //(string firstname, string lastname, string username  , string password , string email, string bio, int experience, int specialty, string DeviceToken , string imageCertificateURL , string  imageCvURL)
            //using the function in repo to create user
            var result = _PtForMeRepository.CreateTrainer(trainer.firstname, trainer.lastname, trainer.username, trainer.password, trainer.email, trainer.bio, trainer.experience, trainer.specialty, trainer.deviceToken, trainer.certificateUrl, trainer.cvURL);
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
        [HttpPost]
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
