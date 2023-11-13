using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Core.DTO.PersonDTO;
using ContactsManager.Core.Enums;
using ContactsManager.Core.ServiceContracts;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ContactsManager.Core.Services
{
    public class PersonSortService : IPersonSortService
    {


        private readonly IPeopleRepository _peopleRepository;

        private readonly ILogger<PersonGetterService> _logger;

        private readonly IDiagnosticContext _diagnosticContext;
        public PersonSortService(IPeopleRepository peopleRepository,ILogger<PersonGetterService> logger, IDiagnosticContext diagnosticContext)
        {
            _peopleRepository = peopleRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }


        public async Task<List<PersonResponse>> GetSortedPeople(List<PersonResponse> people, string sortBy, SortOrderEnum order)
        {
            _logger.LogInformation("GetSortedPeople of PeopleService");

            if (string.IsNullOrEmpty(sortBy))
                return people;

            List<PersonResponse> sortedPeople = (sortBy, order) switch
            {
                (nameof(PersonResponse.PersonName), SortOrderEnum.Ascending) => people.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonName), SortOrderEnum.Descending) => people.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderEnum.Ascending) => people.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderEnum.Descending) => people.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderEnum.Ascending) => people.OrderBy(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderEnum.Descending) => people.OrderByDescending(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonResponse.Age), SortOrderEnum.Ascending) => people.OrderBy(temp => temp.Age).ToList(),

                (nameof(PersonResponse.Age), SortOrderEnum.Descending) => people.OrderByDescending(temp => temp.Age).ToList(),

                (nameof(PersonResponse.Gender), SortOrderEnum.Ascending) => people.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Gender), SortOrderEnum.Descending) => people.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Country), SortOrderEnum.Ascending) => people.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Country), SortOrderEnum.Descending) => people.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderEnum.Ascending) => people.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderEnum.Descending) => people.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderEnum.Ascending) => people.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderEnum.Descending) => people.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),

                _ => people
            };

            return sortedPeople;

        }
     
    }
}
