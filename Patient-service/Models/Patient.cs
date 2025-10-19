﻿using System;
using System.Collections.Generic;

namespace Patient_service.Models
{
    public partial class Patient
    {
        public string Id { get; set; } = null!;

        public string PatientCode { get; set; } = null!;

        public string Fullname { get; set; } = null!;

        public DateOnly? Dob { get; set; }

        public string? Gender { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? BloodType { get; set; }

        public string? IdentityNumber { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? Medical_History {  get; set; }
        public string? Createby { get; set; }

        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();


    }
}
