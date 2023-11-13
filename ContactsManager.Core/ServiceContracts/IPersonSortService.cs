using ContactsManager.Core.DTO.PersonDTO;
using ContactsManager.Core.Enums;

namespace ContactsManager.Core.ServiceContracts
{
    public interface IPersonSortService
    {
        Task<List<PersonResponse>> GetSortedPeople(List<PersonResponse> people, string sortBy, SortOrderEnum order);

    }
}
