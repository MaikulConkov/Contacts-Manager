using ContactsManager.Core.DTO.PersonDTO;

namespace ContactsManager.Core.ServiceContracts
{
    public interface IPersonAddService
    {
        Task<PersonResponse> AddPerson(PersonAddRequest? person_request);
    }
}
