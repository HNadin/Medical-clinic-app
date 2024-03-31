using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medical_clinic.Models
{
    public class Nurse
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(200)]
        public string Education { get; set; }

        [MaxLength(200)]
        public string? CertificationTraining { get; set; }

        [Required]
        [MaxLength(100)]
        public string Specialization { get; set; }

        [Required]
        [MaxLength(80)]
        public string WorkScheduleData { get; set; }

        [NotMapped]
        public IFormFile? PhotoPath { get; set; }
    }
}
