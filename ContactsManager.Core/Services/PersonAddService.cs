using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Core.DTO.PersonDTO;
using ContactsManager.Core.Helpers;
using ContactsManager.Core.ServiceContracts;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ContactsManager.Core.Services
{
    public class PersonAddService : IPersonAddService
    {


        private readonly IPeopleRepository _peopleRepository;

        private readonly ILogger<PersonGetterService> _logger;

        private readonly IDiagnosticContext _diagnosticContext;

        public PersonAddService(IPeopleRepository peopleRepository,ILogger<PersonGetterService> logger, IDiagnosticContext diagnosticContext)
        {
            _peopleRepository = peopleRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }



        public async Task<PersonResponse> AddPerson(PersonAddRequest? person_request)
        {
            if (person_request == null)
            {
                throw new ArgumentNullException(nameof(person_request));
            }

            ValidationHelper.ModelValidation(person_request);

            Person person = person_request.ToPerson();

            person.PersonID = Guid.NewGuid();

            await _peopleRepository.AddPerson(person);

            return person.ToPersonResponse();

        }

    }
}
