namespace Medical_clinic.Models
{
    public class ServiceViewModel
    {
        public Service Service { get; set; } // Дані про послугу

        public List<string>? Doctors { get; set; } // Список імен лікарів

        public List<string>? Nurses { get; set; } // Список імен медсестер
    }
}
