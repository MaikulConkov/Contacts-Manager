using ContactsManager.Core.DTO.CountryDTO;
using ContactsManager.Core.DTO.PersonDTO;
using ContactsManager.Core.Enums;
using ContactsManager.Core.ServiceContracts;
using ContactsManager.Core.Services;
using ContactsManager.UI.Filters.ActionFilters;
using ContactsManager.UI.Filters.ExceptionFilteres;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;

namespace ContactsManager.UI.Controllers
{

    [Route("[controller]")]
    [TypeFilter(typeof(HandleExceptionFilter))]
    [ResponseHeaderFilterFactory("my-key","my-value",1)]
    public class PeopleController : Controller
    {
        private readonly ICountryService _countryService;
        private readonly IPersonGetterService _personGetterService;
        private readonly IPersonAddService _personAddService;
        private readonly IPersonDeleteService _personDeleteService;
        private readonly IPersonUpdateService _personUpdateService;
        private readonly IPersonSortService _personSortService;
        private readonly ILogger<PeopleController> _logger;

        public PeopleController(ICountryService countryService, ILogger<PeopleController> logger, IPersonGetterService personGetterService, IPersonAddService personAddService, IPersonDeleteService personDeleteService, IPersonUpdateService personUpdateService, IPersonSortService personSortService)
        {
            _countryService = countryService;
            _logger = logger;
            _personGetterService = personGetterService;
            _personAddService = personAddService;
            _personDeleteService = personDeleteService;
            _personUpdateService = personUpdateService;
            _personSortService = personSortService;
        }

        [AllowAnonymous]
        [Route("[action]")]
        [Route("/")]
        [TypeFilter(typeof(PeopleListActionFilter),Order =4 )]
        [ResponseHeaderFilterFactory("My-Key-From-Action","My-Value-From-Action",1)]
        public async Task<IActionResult> Index(string searchBy,
            string? searchString,
            string sortBy = nameof(PersonResponse.PersonName),
            SortOrderEnum sortOrderOptions = SortOrderEnum.Ascending
            )
        {
            List<PersonResponse>? people = await _personGetterService.GetFilteredPeople(searchBy, searchString);

            
            List<PersonResponse>? sortedPeopleList =await _personSortService.GetSortedPeople(people, sortBy, sortOrderOptions);

            return View(sortedPeopleList);
        }

        [Route("[action]")]
        [HttpGet]
        [TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "my-key", "my-value", 4 })]
        public async Task<IActionResult> Create()
        {

            List<CountryAddResponse> countries = await _countryService.GetAllCountries();
            ViewBag.Countries = countries.Select(c =>
            new SelectListItem()
            { Text = c.CountryName, Value = c.CountryID.ToString() });

            return View();
        }

        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(PersonCreateAndPostActionFilter))]
        public async Task<IActionResult> Create(PersonAddRequest personRequest)
        {
            PersonResponse personResponse = await _personAddService.AddPerson(personRequest);
            return RedirectToAction("Index", "People");
        }

        [Route("[action]/{personID}")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid personID)
        {

            PersonResponse? personResponse = await _personGetterService.GetPersonByPersonID(personID);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();

            List<CountryAddResponse> countries = await _countryService.GetAllCountries();
            ViewBag.Countries = countries.Select(temp =>
            new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() });

            return View(personUpdateRequest);
        }

        [HttpPost]
        [Route("[action]/{personID}")]
        [TypeFilter(typeof(PersonCreateAndPostActionFilter))]
        public async Task<IActionResult> Edit(PersonUpdateRequest personRequest)
        {
            PersonResponse? personResponse = await _personGetterService.GetPersonByPersonID(personRequest.PersonID);

            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }
             PersonResponse updatedPerson = await _personUpdateService.UpdatePerson(personRequest);
             return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("[action]/{personID}")]
        public async Task<IActionResult> Delete(Guid? personID)
        {
            PersonResponse? personResponse = await _personGetterService.GetPersonByPersonID(personID);
            if (personResponse == null)
                return RedirectToAction("Index");

            return View(personResponse);
        }

        [HttpPost]
        [Route("[action]/{personID}")]
        public async Task<IActionResult> Delete(PersonUpdateRequest personUpdateResult)
        {
            PersonResponse? personResponse = await _personGetterService.GetPersonByPersonID(personUpdateResult.PersonID);
            if (personResponse == null)
                return RedirectToAction("Index");

            await _personDeleteService.DeletePerson(personUpdateResult.PersonID);
            return RedirectToAction("Index");
        }

        [Route("PeoplePDF")]
        public async Task<IActionResult> PeoplePDF()
        {
            //Get list of people
            List<PersonResponse> people = await _personGetterService.GetAllPeople();
            return new ViewAsPdf("PeoplePDF", people, ViewData) {
                PageMargins = new Rotativa.AspNetCore.Options.Margins()
                {
                    Top = 20, Right = 20, Bottom = 20, Left = 20
                },
                PageOrientation=Rotativa.AspNetCore.Options.Orientation.Landscape,
                
            };
        }

        [Route("PeopleCSV")]
        public async Task<IActionResult> PeopleCSV()
        {
            MemoryStream memoryStream = await _personGetterService.GeneratePeopleCSV();
            return File(memoryStream, "application/octet-stream", "people.csv");
        }

        [Route("PeopleExcel")]
        public async Task<IActionResult> PeopleExcel()
        {
            MemoryStream memoryStream = await _personGetterService.GetPeopleExcel();
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "people.xlsx");
        }


    }
}
