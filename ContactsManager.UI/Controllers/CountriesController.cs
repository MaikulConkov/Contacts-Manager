using ContactsManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.UI.Controllers
{

    [Route("[controller]")]
    public class CountriesController : Controller
    {

        private readonly ICountryService _countryService;

        public CountriesController(ICountryService countryService)
        {
            _countryService = countryService;
        }


        [Route("UploadFromExcel")]
        [HttpGet]
        public IActionResult UploadFromExcel()
        {
            return View();
        }

        [Route("UploadFromExcel")]
        [HttpPost]
        public async Task<IActionResult> UploadFromExcel(IFormFile excelFile)
        {
            if (excelFile == null|| excelFile.Length==0)
            {
                ViewBag.ErrorMessage = "Please select an xlsx file";
                return View();
            }
            if (!Path.GetExtension(excelFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.ErrorMessage = "Unsupported file. 'xlsx' is expected";
                return View();
            }
            int countriesResieved=await _countryService.UploadContriesFromExcelFile(excelFile);
            ViewBag.Message = $"{countriesResieved} Countries Uploaded";
            return View();
        }
    }
}
