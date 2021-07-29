using System;

namespace Application.Operations.Appointments
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string Service { get; set; }
        public DateTime Date { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsCompleted { get; set; }
    }
}