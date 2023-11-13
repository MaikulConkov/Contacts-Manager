using ContactsManager.Core.DTO.PersonDTO;

namespace ContactsManager.Core.ServiceContracts
{
    public interface IPersonGetterService
    {
        Task<List<PersonResponse>> GetAllPeople();

        Task<PersonResponse?> GetPersonByPersonID(Guid? personID);

        Task<List<PersonResponse>?> GetFilteredPeople(string searchBy, string? searchString);
        public Task<MemoryStream> GeneratePeopleCSV();

        public Task<MemoryStream> GetPeopleExcel();

    }
}
