using ContactsManager.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ContactsManager.Core.DTO.PersonDTO
{
    public class PersonUpdateRequest
    {
        [Required]
        public Guid PersonID { get; set; }

        [Required(ErrorMessage = "Person Name can't be blank or empty.")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Email value should be a valid value")]
        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }


        public GenderOptions? Gender { get; set; }

        public Guid? CountryID { get; set; }

        public string? Address { get; set; }

        public bool ReceiveNewsLetters { get; set; }
       
    }
}

