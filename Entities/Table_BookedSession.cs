namespace Pt_For_Me.Entities
{
    public class Table_BookedSession
    {
        public int ID { get; set; }
        public int UserPackageID { get; set; }
        public int RoomID { get; set;  }
        public int UserID { get; set;  }
        public int TrainerID { get; set; }
        public DateTime Start_Time { get; set; }
        public DateTime End_Time { get; set; }
    }
}
