using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Core.DTO.CountryDTO;
using ContactsManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

namespace ContactsManager.Core.Services
{
    public class CountryService : ICountryService
    {

        private List<Country> countries;
        private readonly ICountriesRepository _countriesRepository;

        public CountryService(ICountriesRepository countriesRepository)
        {

            _countriesRepository = countriesRepository;     
        }

        public async Task<CountryAddResponse> AddCountry(CountryAddRequest? countryRequest)
        {
            if (countryRequest == null)
            {
                throw new ArgumentNullException(nameof(countryRequest));
            }

            if (countryRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryRequest.CountryName));
            }

            if(await _countriesRepository.GetCountryByCountryName(countryRequest.CountryName) != null)
            {
                throw new ArgumentException("Given country name already exists");
            }
            Country country = countryRequest.ToCountry();
            country.CountryID = Guid.NewGuid();
            
             await _countriesRepository.AddCountry(country);
            return country.ToCountryResponse();

        }

        public async Task<List<CountryAddResponse>> GetAllCountries()
        {
            var countries = await _countriesRepository.GetAllCountries();

            return countries.Select(country => country.ToCountryResponse()).ToList();

        }

        public async Task<CountryAddResponse?> GetCountryByCountryID(Guid? countryID)
        {
            if (countryID == null)
                return null;

            Country? country_response_from_list = await _countriesRepository.GetCountryByCountryID(countryID);

            if (country_response_from_list == null)
                return null;

            return country_response_from_list.ToCountryResponse();
        }

        public async Task<int> UploadContriesFromExcelFile(IFormFile formFile)
        {
            MemoryStream memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            int countriesInserted = 0;
            using(ExcelPackage excelPackage= new ExcelPackage(memoryStream)) 
            {
                ExcelWorksheet workSheet=excelPackage.Workbook.Worksheets["Contries"];

                int rowCount = workSheet.Dimension.Rows;

                for(int i=2; i <= rowCount; i++)
                {
                    string? cellValue= Convert.ToString(workSheet.Cells[i, 1].Value);

                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        string countryName = cellValue;
                        CountryAddRequest countryRequest = new CountryAddRequest() { CountryName = countryName };
                        await AddCountry(countryRequest);

                        countriesInserted++;
                    }
                }
               
            }
            return countriesInserted;
        }
    }
}