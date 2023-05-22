namespace Pt_For_Me.Models
{
    public class GetSessionInfoByTrainerID_Result
    {
        public int TrainerID { get; set; }
        public string Client_Name { get; set; }
        public string ?HealthRisk { get; set; }
        public string ?Injury { get; set; }
        public int RoomID { get; set; }
        public DateTime Start_Time{get; set;}
        public DateTime End_Time{get; set;}
    }
}
