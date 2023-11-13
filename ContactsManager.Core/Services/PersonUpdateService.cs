using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Core.DTO.PersonDTO;
using ContactsManager.Core.Helpers;
using ContactsManager.Core.ServiceContracts;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ContactsManager.Core.Services
{
    public class PersonUpdateService : IPersonUpdateService
    {


        private readonly IPeopleRepository _peopleRepository;

        private readonly ILogger<PersonGetterService> _logger;

        private readonly IDiagnosticContext _diagnosticContext;

        public PersonUpdateService(IPeopleRepository peopleRepository,ILogger<PersonGetterService> logger, IDiagnosticContext diagnosticContext)
        {
            _peopleRepository = peopleRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }




        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? person_request)
        {
            if (person_request == null)
                throw new ArgumentNullException(nameof(Person));

            //validation
            ValidationHelper.ModelValidation(person_request);
            //get matching person object to update
            Person? matchingPerson = await _peopleRepository.GetPersonByPersonID(person_request.PersonID);
            if (matchingPerson == null)
            {
                throw new ArgumentException("Given person id doesn't exist");
            }


            //update all details
            matchingPerson.PersonName = person_request.PersonName;
            matchingPerson.Email = person_request.Email;
            matchingPerson.DateOfBirth = person_request.DateOfBirth;
            matchingPerson.Gender = person_request.Gender.ToString();
            matchingPerson.CountryID = person_request.CountryID;
            matchingPerson.Address = person_request.Address;
            matchingPerson.ReceiveNewsLetters = person_request.ReceiveNewsLetters;

            await _peopleRepository.UpdatePerson(matchingPerson); //UPDATE


            return matchingPerson.ToPersonResponse();
        }

    }
}
