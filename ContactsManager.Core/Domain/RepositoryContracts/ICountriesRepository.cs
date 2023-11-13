using ContactsManager.Core.Domain.Entities;

namespace ContactsManager.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Represents Fata access Logic for managing Country Entity
    /// </summary>
    public interface ICountriesRepository
    {
        Task<Country> AddCountry(Country country);

        Task<ICollection<Country>> GetAllCountries();

        Task<Country> GetCountryByCountryID(Guid? countryID);

        Task<Country> GetCountryByCountryName(string countryName);

    }
}