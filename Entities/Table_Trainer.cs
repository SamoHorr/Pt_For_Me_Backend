namespace Pt_For_Me.Entities
{
    public class Table_Trainer
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ProfileURL { get; set; } 
        public int SpecialityID { get; set; }
        public string Bio { get; set; }
        public string Email { get; set; }
        public int Experience { get; set; }
        public string Device_Token { get; set; }


        //images for the documents
        //public IFormFile CertificateUri { get; set; }
        public string CertificateURL { get; set; }
        //public IFormFile CVUri { get; set; }
        public string CVURL { get; set; }  

    }
}
