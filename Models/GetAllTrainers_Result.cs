﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Pt_For_Me.Models
{
    public class GetAllTrainers_Result
    {
        public int TrainerID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Bio { get; set; }
        public int Experience { get; set; }
        public string specialty { get; set; }
        [NotMapped]
        public object ProfileURL { get; internal set; }
    }
}
