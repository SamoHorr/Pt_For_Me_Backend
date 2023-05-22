namespace Pt_For_Me.Classes
{
    public class Session
    {
        public int SessionID { get; set; }
        public int TrainerID { get; set; }
        public int UserID { get;set; }
        public string review { get; set; }
        public int rating { get; set;  }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
