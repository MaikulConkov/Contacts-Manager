using AutoFixture;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ContactsManager.Core.ServiceContracts;
using ContactsManager.UI.Controllers;
using ContactsManager.Core.DTO.PersonDTO;
using ContactsManager.Core.Enums;
using ContactsManager.Core.DTO.CountryDTO;

namespace CRUDTests
{
    public class PeopleControllerTest
 {
  private readonly IPersonGetterService _peopleGetterService;
  private readonly IPersonAddService _peopleAdderService;
  private readonly IPersonDeleteService _peopleDeleterService;
  private readonly IPersonUpdateService _peopleUpdaterService;
  private readonly IPersonSortService _peopleSorterService;

  private readonly ICountryService _countriesService;
  private readonly ILogger<PeopleController> _logger;

  private readonly Mock<ICountryService> _countriesServiceMock;
  private readonly Mock<IPersonGetterService> _peopleGetterServiceMock;
  private readonly Mock<IPersonAddService> _peopleAdderServiceMock;
  private readonly Mock<IPersonUpdateService> _peopleUpdaterServiceMock;
  private readonly Mock<IPersonSortService> _peopleSorterServiceMock;
  private readonly Mock<IPersonDeleteService> _peopleDeleterServiceMock;


  private readonly Mock<ILogger<PeopleController>> _loggerMock;

  private readonly Fixture _fixture;


  public PeopleControllerTest()
  {
   _fixture = new Fixture();

   _countriesServiceMock = new Mock<ICountryService>();
   _peopleGetterServiceMock = new Mock<IPersonGetterService>();
   _peopleAdderServiceMock = new Mock<IPersonAddService>();
   _peopleDeleterServiceMock = new Mock<IPersonDeleteService>();
   _peopleUpdaterServiceMock = new Mock<IPersonUpdateService>();
   _peopleSorterServiceMock = new Mock<IPersonSortService>();

   _loggerMock = new Mock<ILogger<PeopleController>>();

   _countriesService = _countriesServiceMock.Object;
   _peopleGetterService = _peopleGetterServiceMock.Object;
   _peopleAdderService = _peopleAdderServiceMock.Object;
   _peopleUpdaterService = _peopleUpdaterServiceMock.Object;
   _peopleDeleterService = _peopleDeleterServiceMock.Object;
   _peopleSorterService = _peopleSorterServiceMock.Object;

   _logger = _loggerMock.Object;
  }

  #region Index

  [Fact]
  public async Task Index_ShouldReturnIndexViewWithPeopleList()
  {
   //Arrange
   List<PersonResponse> people_response_list = _fixture.Create<List<PersonResponse>>();


  PeopleController peopleController = new PeopleController(_countriesService, _logger, _peopleGetterService, _peopleAdderService, _peopleDeleterService, _peopleUpdaterService, _peopleSorterService);

   _peopleGetterServiceMock
    .Setup(temp => temp.GetFilteredPeople(It.IsAny<string>(), It.IsAny<string>()))
    .ReturnsAsync(people_response_list);

   _peopleSorterServiceMock
    .Setup(temp => temp.GetSortedPeople(It.IsAny<List<PersonResponse>>(), It.IsAny<string>(), It.IsAny<SortOrderEnum>()))
    .ReturnsAsync(people_response_list);

   //Act
   IActionResult result = await peopleController.Index(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<SortOrderEnum>());

   //Assert
   ViewResult viewResult = Assert.IsType<ViewResult>(result);

   viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<PersonResponse>>();
   viewResult.ViewData.Model.Should().Be(people_response_list);
  }
  #endregion


  #region Create

  [Fact]
  public async void Create_IfNoModelErrors_ToReturnRedirectToIndex()
  {
   //Arrange
   PersonAddRequest person_add_request = _fixture.Create<PersonAddRequest>();

   PersonResponse person_response = _fixture.Create<PersonResponse>();

   List<CountryAddResponse> countries = _fixture.Create<List<CountryAddResponse>>();

   _countriesServiceMock
    .Setup(temp => temp.GetAllCountries())
    .ReturnsAsync(countries);

   _peopleAdderServiceMock
    .Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>()))
    .ReturnsAsync(person_response);

   PeopleController poepleController = new PeopleController(_countriesService, _logger, _peopleGetterService, _peopleAdderService, _peopleDeleterService, _peopleUpdaterService, _peopleSorterService);


   //Act
   IActionResult result = await poepleController.Create(person_add_request);

   //Assert
   RedirectToActionResult redirectResult = Assert.IsType<RedirectToActionResult>(result);

   redirectResult.ActionName.Should().Be("Index");
  }

  #endregion
 }
}
