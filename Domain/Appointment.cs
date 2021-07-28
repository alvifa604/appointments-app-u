using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Appointment
    {
        public int PatientId { get; set; }
        public User Patient { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}