﻿using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalMgrSystem.Model
{
    public class Consultant
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Gender { get; set; }
        public int? Age { get; set; }
        public int? isSystemDr { get; set; }
        public string? ContectNumber { get; set; }
        public string? Email { get; set; }
        public int? SpecialistId { get; set; }
        public Specialist? Specialist { get; set; }
        public string? Address { get; set; }
        public int? Status { get; set; }
        public int? CreateUser { get; set; }
        public int? ModifiedUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public decimal? DoctorFee { get; set; }
        public decimal? HospitalFee { get; set; }

		[NotMapped]
        public int ptCount { get; set; }
        [NotMapped]
        public decimal hospitalIncome { get; set; }
        [NotMapped]
        public decimal doctorIncome { get; set; }
    }
}
