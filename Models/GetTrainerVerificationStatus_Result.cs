namespace Pt_For_Me.Models
{
    public class GetTrainerVerificationStatus_Result
    {
        public int ID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public bool? isAccepted { get; set; }
        public bool Status { get; set; }
    }
}
