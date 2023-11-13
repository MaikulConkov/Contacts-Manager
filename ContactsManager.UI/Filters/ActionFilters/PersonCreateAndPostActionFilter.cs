using ContactsManager.Core.DTO.CountryDTO;
using ContactsManager.Core.ServiceContracts;
using ContactsManager.UI.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace ContactsManager.UI.Filters.ActionFilters
{
    public class PersonCreateAndPostActionFilter : IAsyncActionFilter
    {
        private readonly ICountryService _countryService;

        public PersonCreateAndPostActionFilter(ICountryService countriSurvice)
        {
            _countryService = countriSurvice;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(context.Controller is PeopleController personController)
            {
                if (!personController.ModelState.IsValid)
                {
                    List<CountryAddResponse> countries = await _countryService.GetAllCountries();
                    personController.ViewBag.Countries = countries.Select(temp => new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() });
                    personController.ViewBag.Errors = personController.ModelState.Values.SelectMany(v => v.Errors).SelectMany(e => e.ErrorMessage).ToList();
                    var personRequest = context.ActionArguments["personAddRequest"];
                    context.Result= personController.View(personRequest);//when we assign non-null value to context.Result we short-cercuit the code that means we return the Value(View) rather than call the await next();
                }
                else
                {
                    await next();
                }
            }
            else
            {
                await next();
            }
        }
    }
}
