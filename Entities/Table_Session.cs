namespace Pt_For_Me.Entities
{
    public class Table_Session
    {
        public int ID { get; set; }
        public int  UserID {get; set;}
        public int TrainerID { get; set; }
        public string Review { get; set; }
        public int Rating { get; set; }
        public DateTime? date { get; set;}
        public DateTime? Start_Time { get; set; }
        public DateTime? End_Time { get; set; }
        public bool ?isAccepted { get; set; }

    }
}
