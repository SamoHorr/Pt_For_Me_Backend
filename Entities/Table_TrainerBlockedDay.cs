
namespace Pt_For_Me.Entities
{
    public class Table_TrainerBlockedDay
    {
       public int ID { get; set; }
        public int TrainerID { get; set; }
        public DateTime Day_BlockedStart { get; set; }
        public DateTime Day_BlockedEnd { get; set; }
    }
}
