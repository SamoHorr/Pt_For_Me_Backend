﻿namespace Pt_For_Me.Entities
{
    public class Table_UserPackage
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int PackageID { get; set; }
        public DateTime Start_Time { get; set; }
        public DateTime End_Time { get; set; }
    }
}