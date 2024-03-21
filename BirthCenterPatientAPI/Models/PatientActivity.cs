using System.ComponentModel.DataAnnotations;

namespace BirthCenterPatientAPI.Models
{
    public class PatientActivity
    {
        [Key]
        public int Id { get; set; }

        public Guid PatientId { get; set; }

        [Required]
        public bool Active { get; set; }

        public DateTime StatusChangeDate { get; set; } = DateTime.Now;
    }
}
