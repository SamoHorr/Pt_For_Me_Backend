namespace Pt_For_Me.Classes
{
    public class BookedSession
    {
        public int ID { get; set; }
        public int UserPackageID { get; set;}
        public int UserID { get; set;  }
        public int TrainerID { get; set; }
        public int RoomID { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
    }
}
