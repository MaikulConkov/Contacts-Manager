using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;


namespace Repositories
{
    public class PeopleRepository : IPeopleRepository
    {

        private readonly AppDbContext _db;
        private readonly ILogger<PeopleRepository> _logger; 
        public PeopleRepository(AppDbContext db, ILogger<PeopleRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<Person> AddPerson(Person person)
        {
            _db.People.Add(person);
            await _db.SaveChangesAsync();

            return person;
        }

        public async Task<bool> DeletePersonByPersonID(Guid? personID)
        {
            _db.People.RemoveRange(_db.People.Where(temp => temp.PersonID == personID));
            int rowsDeleted = await _db.SaveChangesAsync();

            return rowsDeleted > 0;
        }

        public async Task<List<Person>> GetAllPeople()
        {
            return await _db.People.Include("Country").ToListAsync();
        }

        public async Task<List<Person>> GetFilteredPeople(Expression<Func<Person, bool>> predicate)
        {
            return await _db.People.Include("Country")
            .Where(predicate)
            .ToListAsync();
        }

        public async Task<Person?> GetPersonByPersonID(Guid? personID)
        {
            return await _db.People.Include("Country")
             .FirstOrDefaultAsync(temp => temp.PersonID == personID);
        }

        public async Task<Person> UpdatePerson(Person person)
        {
            Person? matchingPerson = await _db.People.FirstOrDefaultAsync(temp => temp.PersonID == person.PersonID);

            if (matchingPerson == null)
                return person;

            matchingPerson.PersonName = person.PersonName;
            matchingPerson.Email = person.Email;
            matchingPerson.DateOfBirth = person.DateOfBirth;
            matchingPerson.Gender = person.Gender;
            matchingPerson.CountryID = person.CountryID;
            matchingPerson.Address = person.Address;
            matchingPerson.ReceiveNewsLetters = person.ReceiveNewsLetters;

            int countUpdated = await _db.SaveChangesAsync();

            return matchingPerson;
        }
    }
}
