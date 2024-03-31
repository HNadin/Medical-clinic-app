namespace Medical_clinic.Models
{
    public class AppointmentViewModel
    {
        public int ServiceId { get; set; }
        public string Doctor { get; set; }
        public DateTime DateTime { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }

}
