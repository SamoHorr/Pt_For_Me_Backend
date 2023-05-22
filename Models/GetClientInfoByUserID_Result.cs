namespace Pt_For_Me.Models
{
    public class GetClientInfoByUserID_Result
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ?Profile_Url { get; set; }
        public string ?Goal { get; set; }
        public double TargetWeight { get; set; }
        public string ?Health_Risk { get; set; }
        public string ?Injury { get; set; }
     
    }
}
