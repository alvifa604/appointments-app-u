using System;
using System.Collections.Generic;

namespace Domain
{
    public class User 
    {
        public int Id { get; set; }
        public string IdDocument { get; set; }
        public string Name { get; set; }
        public string FirstLastname { get; set; }
        public string SecondLastname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}