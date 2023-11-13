using ContactsManager.Core.DTO.PersonDTO;
using ContactsManager.UI.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManager.UI.Filters.ActionFilters
{
    public class PeopleListActionFilter : IActionFilter
    {
        private readonly ILogger<PeopleListActionFilter> _logger;

        public PeopleListActionFilter(ILogger<PeopleListActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("PeopleListActionFilter method OnActionExecuted");
            PeopleController peopleController = (PeopleController)context.Controller;

            IDictionary<string, object?>? parameters = (IDictionary<string,object?>?)peopleController.HttpContext.Items["arguments"];

            if (parameters!=null)
            {
                if (parameters.ContainsKey("searchBy"))
                {

                    peopleController.ViewData["CurrentSearchBy"] = Convert.ToString(parameters["searchBy"]);
                }

                if (parameters.ContainsKey("searchString"))
                {

                    peopleController.ViewData["CurrentSearchString"] = Convert.ToString(parameters["searchString"]);
                }
                if (parameters.ContainsKey("sortBy"))
                {

                    peopleController.ViewData["CurrentSortBy"] = Convert.ToString(parameters["sortBy"]);
                }
                if (parameters.ContainsKey("sortOrder"))
                {

                    peopleController.ViewData["CurrentSortOrder"] = Convert.ToString(parameters["sortOrder"]);
                }
            }
            peopleController.ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(PersonResponse.PersonName), "Person Name" },
                { nameof(PersonResponse.Email), "Email" },
                { nameof(PersonResponse.Gender), "Gender" },
                { nameof(PersonResponse.CountryID), "Country" },
                { nameof(PersonResponse.Address), "Address" },
            };
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //before logic
            _logger.LogInformation("PeopleListActionFilter method OnActionExecuting");

            context.HttpContext.Items["arguments"] = context.ActionArguments;

            if (context.ActionArguments.ContainsKey("searchBy"))
            {
                string? searchBy = Convert.ToString(context.ActionArguments["searchBy"]);

                if (!string.IsNullOrEmpty(searchBy))
                {
                    var searchByOptions = new List<string>
                    {
                        nameof(PersonResponse.PersonName),
                        nameof(PersonResponse.Email),
                        nameof(PersonResponse.DateOfBirth),
                        nameof(PersonResponse.Gender),
                        nameof(PersonResponse.CountryID),
                        nameof(PersonResponse.Address),
                    };
                    if (searchByOptions.Any(temp=>temp==searchBy)==false)
                    {
                        _logger.LogInformation("SearchBy value is {searchBy}", searchBy);
                        context.ActionArguments["searchBy"]=nameof(PersonResponse.PersonName);
                        _logger.LogInformation("SearchBy updated value is {searchBy}", searchBy);
                    }
                }

            }
        }
    }
}
