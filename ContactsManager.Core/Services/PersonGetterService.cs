using CsvHelper;
using System.Globalization;
using OfficeOpenXml;
using Microsoft.Extensions.Logging;
using Serilog;
using ContactsManager.Core.ServiceContracts;
using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Core.DTO.PersonDTO;
using ContactsManager.Core.Domain.Entities;
using SerilogTimings;

namespace ContactsManager.Core.Services
{
    public class PersonGetterService : IPersonGetterService
    {


        private readonly IPeopleRepository _peopleRepository;

        private readonly ILogger<PersonGetterService> _logger;

        private readonly IDiagnosticContext _diagnosticContext;

        public PersonGetterService(IPeopleRepository peopleRepository,ILogger<PersonGetterService> logger, IDiagnosticContext diagnosticContext)
        {
            _peopleRepository = peopleRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }



        public async Task<List<PersonResponse>> GetAllPeople()
        {

            var people = await _peopleRepository.GetAllPeople();

            return  people.Select(temp => temp.ToPersonResponse()).ToList();
            //return _db.sp_GetAllPeople().Select(temp=>ConvertPersonIntoPersonResponse(temp)).ToList();
        }

        public async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
        {
            if (personID == null)
            {
                return null;
            }

            Person? person = await _peopleRepository.GetPersonByPersonID(personID);
            if (person == null)
            {
                return null;
            }

            return person.ToPersonResponse();
        }

        public async Task<List<PersonResponse>?> GetFilteredPeople(string searchBy, string? searchString)
        {
            _logger.LogInformation("GetFilteredPeople of PeopleService");

            List<Person> people;

            using (Operation.Time("Time for Filtered People from Database"))
            {
                people = searchBy switch
                {
                    nameof(PersonResponse.PersonName) =>
                     await _peopleRepository.GetFilteredPeople(temp =>
                     temp.PersonName.Contains(searchString)),

                    nameof(PersonResponse.Email) =>
                     await _peopleRepository.GetFilteredPeople(temp =>
                     temp.Email.Contains(searchString)),

                    nameof(PersonResponse.Gender) =>
                     await _peopleRepository.GetFilteredPeople(temp =>
                     temp.Gender.Contains(searchString)),

                    nameof(PersonResponse.CountryID) =>
                     await _peopleRepository.GetFilteredPeople(temp =>
                     temp.Country.CountryName.Contains(searchString)),

                    nameof(PersonResponse.Address) =>
                    await _peopleRepository.GetFilteredPeople(temp =>
                    temp.Address.Contains(searchString)),

                    _ => await _peopleRepository.GetAllPeople()
                };
            } //end of "using block" of serilog timings

            _diagnosticContext.Set("People", people);

            return people.Select(temp => temp.ToPersonResponse()).ToList();
        }

        public async Task<MemoryStream> GeneratePeopleCSV()
        {
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);
            CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture, leaveOpen: true);

            csvWriter.WriteHeader<PersonResponse>();
            csvWriter.NextRecord();

            List<PersonResponse> poeple = await GetAllPeople();
            await csvWriter.WriteRecordsAsync(poeple);

            memoryStream.Position = 0;
            return memoryStream;
        }

        public async Task<MemoryStream> GetPeopleExcel()
        {
            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.Add("PeopleSheet");
                workSheet.Cells["A1"].Value = "Person Name";
                workSheet.Cells["B1"].Value = "Email";
                workSheet.Cells["C1"].Value = "Date of Birth";
                workSheet.Cells["D1"].Value = "Age";
                workSheet.Cells["E1"].Value = "Gender";
                workSheet.Cells["F1"].Value = "Country";
                workSheet.Cells["G1"].Value = "Address";
                workSheet.Cells["H1"].Value = "Recieve News Letters";

                int row = 2;
                List<PersonResponse> poeple = await GetAllPeople();

                foreach (PersonResponse person in poeple)
                {
                    workSheet.Cells[$"A{row}"].Value = person.PersonName;
                    workSheet.Cells[$"B{row}"].Value = person.Email;
                    if (person.DateOfBirth.HasValue)
                    {
                        workSheet.Cells[$"C{row}"].Value = person.DateOfBirth.Value.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        workSheet.Cells[$"C{row}"].Value = "";
                    }
                    workSheet.Cells[$"D{row}"].Value = person.Age;
                    workSheet.Cells[$"E{row}"].Value = person.Gender;
                    workSheet.Cells[$"F{row}"].Value = person.Country;
                    workSheet.Cells[$"G{row}"].Value = person.Address;
                    workSheet.Cells[$"H{row}"].Value = person.ReceiveNewsLetters;
                    row++;
                }
                workSheet.Cells[$"A1:H{row}"].AutoFitColumns();

                await excelPackage.SaveAsync();
            }
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
