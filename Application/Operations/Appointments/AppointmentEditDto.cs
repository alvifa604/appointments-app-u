namespace Application.Operations.Appointments
{
    public class AppointmentEditDto
    {
        public int AppointmentId { get; set; }
        public bool Canceled { get; set; }
        public bool Completed { get; set; }
    }
}