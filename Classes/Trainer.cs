namespace Pt_For_Me.Classes
{
    public class Trainer
    {
        //(string firstname, string lastname, string username  , string password , string email, string bio, int experience, int specialty, string DeviceToken , string imageCertificateURL , string  imageCvURL)
        public int id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string profileURL { get; set; }
        public string bio { get; set; }
        public int experience { get; set; }
        public int specialty { get; set; }
        public string  deviceToken { get; set; }
        public string certificateUrl { get; set; }
        public string cvURL { get; set; }
        public string ?channelName { get; set; }
        public string ?token { get; set; }

    }
}
