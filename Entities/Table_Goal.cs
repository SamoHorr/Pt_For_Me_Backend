namespace Pt_For_Me.Entities
{
    public class Table_Goal
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Description { get; set; }
        public int TargetWeight { get; set; }
        public DateTime Date { get; set; }
    }
}
