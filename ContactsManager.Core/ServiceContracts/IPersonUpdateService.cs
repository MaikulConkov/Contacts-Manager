using ContactsManager.Core.DTO.PersonDTO;

namespace ContactsManager.Core.ServiceContracts
{
    public interface IPersonUpdateService
    {
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? person_request);
    }
}
