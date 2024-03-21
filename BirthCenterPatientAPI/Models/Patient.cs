using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using BirthCenterPatientAPI.Enums;

namespace BirthCenterPatientAPI.Models
{
    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The first name is required.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "The last name is required.")]
        public string? LastName { get; set; }

        public string? Patronymic { get; set; }

        public Gender Gender { get; set; } = Gender.Unknown;

        [Required(ErrorMessage = "The birth date is required.")]
        public DateTime BirthDate { get; set; }

        public bool Active { get; set; }
    }
}
