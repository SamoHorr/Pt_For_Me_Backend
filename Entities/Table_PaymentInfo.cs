namespace Pt_For_Me.Entities
{
    public class Table_PaymentInfo
    {
        public int ID  { get; set; }
        public int UserID { get; set; }
        public int CardNB { get; set; }
        public int CVV { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
