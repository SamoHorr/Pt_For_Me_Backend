namespace Pt_For_Me.Models
{
    public class GetSessionInfoByUserID_Result
    {
        public int UserID { get; set; }
        public string Trainer_Name { get; set;}
        public int RoomID { get; set; }
        public DateTime Start_Time { get; set; }
        public DateTime End_Time { get; set; }
    }
}
