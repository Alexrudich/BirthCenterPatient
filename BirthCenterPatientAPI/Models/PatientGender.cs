using System.ComponentModel.DataAnnotations;

namespace BirthCenterPatientAPI.Models
{
    public class PatientGender
    {
        public int Id { get; set; }

        [Required]
        public string? Value { get; set; }
    }
}
