using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Core.ServiceContracts;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ContactsManager.Core.Services
{
    public class PersonDeleteService : IPersonDeleteService
    {


        private readonly IPeopleRepository _peopleRepository;

        private readonly ILogger<PersonGetterService> _logger;

        private readonly IDiagnosticContext _diagnosticContext;

        public PersonDeleteService(IPeopleRepository peopleRepository,ILogger<PersonGetterService> logger, IDiagnosticContext diagnosticContext)
        {
            _peopleRepository = peopleRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }



        public async Task<bool> DeletePerson(Guid? personID)
        {
            if (personID == null)
            {
                throw new ArgumentNullException(nameof(personID));
            }

            Person? person = await _peopleRepository.GetPersonByPersonID(personID.Value);
            if (person == null)
                return false;

            await _peopleRepository.DeletePersonByPersonID(personID.Value);

            return true;
        }

    }
}
