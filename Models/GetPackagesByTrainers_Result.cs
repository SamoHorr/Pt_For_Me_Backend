namespace Pt_For_Me.Models
{
    public class GetPackagesByTrainers_Result
    {
        public int ID { get; set; }
        public string Trainer { get; set; }
        public int AverageRating { get; set; }
        public decimal Pricing { get; set; }
        public string PackageType { get; set; }
        public int Bundle { get; set; }
    }
}
