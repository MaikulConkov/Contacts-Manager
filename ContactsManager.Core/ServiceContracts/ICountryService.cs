using ContactsManager.Core.DTO.CountryDTO;
using Microsoft.AspNetCore.Http;

namespace ContactsManager.Core.ServiceContracts
{
    public interface ICountryService
    {
        Task<CountryAddResponse> AddCountry(CountryAddRequest? countryRequest);

        Task<List<CountryAddResponse>> GetAllCountries();

        Task<CountryAddResponse?> GetCountryByCountryID(Guid? countryID);


        Task<int> UploadContriesFromExcelFile(IFormFile formFile);
       
    }
}
