using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ContactsManager.Infrastructure.Repositories
{
    public class CountriesRepository : ICountriesRepository
    {
        private readonly AppDbContext _dbContext;

        public CountriesRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Country> AddCountry(Country country)
        {
           await _dbContext.Countries.AddAsync(country);
           await _dbContext.SaveChangesAsync();
           return country;            
        }

        public async Task<ICollection<Country>> GetAllCountries()
        {
            return await _dbContext.Countries.ToListAsync();
        }

        public async Task<Country> GetCountryByCountryID(Guid? countryID)
        {
             Country countryToReturn= await _dbContext.Countries.FirstOrDefaultAsync(temp => temp.CountryID == countryID);
             return countryToReturn;
        }

        public async Task<Country> GetCountryByCountryName(string countryName)
        {
            Country countryToReturn = await _dbContext.Countries.FirstOrDefaultAsync(temp => temp.CountryName == countryName);
            return countryToReturn;
        }
    }
}