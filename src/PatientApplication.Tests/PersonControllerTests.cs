using Microsoft.Extensions.Logging;
using Moq;
using PersonApplication.Controllers;
using PersonApplication.DataAccess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PersonEntity = PersonApplication.Entities.Models.Person;
using GroupEntity = PersonApplication.Entities.Models.Group;
using Microsoft.AspNetCore.Mvc;
using PersonApplication.Models;
using System.Linq;
using NUnit.Framework;

namespace PatientApplication.Tests
{
    [TestFixture]
    [Category("Unit")]
    public class PersonControllerTests
    {
        private Mock<IPersonRespository> _personRepository;
        private Mock<ILogger<PersonController>> _logger;
        
        [SetUp]
        public void Setup()
        {
            _personRepository = new Mock<IPersonRespository>();
            _logger = new Mock<ILogger<PersonController>>();
        }

        [Test]
        public async Task GivenPersonsExistInDB_WhenIndexActionIsCalled_ThenDisplayAllPersonsInIndexView()
        {
            //Arrange
            _personRepository.Setup(x => x.GetAllPersons()).ReturnsAsync(new List<PersonEntity>
            {
                new PersonEntity
                {
                    Id = 1,
                    Name = "John Smith",
                    DateAdded = DateTime.Now.AddDays(-20),
                    Group = new GroupEntity()
                },
                new PersonEntity
                {
                    Id = 2,
                    Name = "Jane Smith",
                    DateAdded = DateTime.Now.AddDays(-20),
                    Group = new GroupEntity()
                }
            });
            var controller = new PersonController(_logger.Object, _personRepository.Object);

            //Act
            var result = await controller.Index();

            //Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = (ViewResult)result;
            var model = (IEnumerable<Person>)viewResult.ViewData.Model;
            Assert.AreEqual(2, model.Count());
        }

        [Test]
        public async Task GivenGroupsExistInDB_WhenCreateActionIsCalled_ThenViewModelHasGroupsInCreateView()
        {
            //Arrange
            _personRepository.Setup(x => x.GetAllGroups()).ReturnsAsync(new List<GroupEntity>
            {
                new GroupEntity
                {
                    Id = 1,
                    Name = "Group A"
                },
                new GroupEntity
                {
                    Id = 2,
                    Name = "Group B"
                }
            });
            var controller = new PersonController(_logger.Object, _personRepository.Object);

            //Act
            var result = await controller.Create();

            //Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = (ViewResult)result;
            var model = (CreateViewModel)viewResult.ViewData.Model;
            Assert.AreEqual(2, model.Group.Count());
        }

        [Test]
        public async Task GivenNewPersonIsTobeAdded_WhenModelStateIsValidAndAllInoutsAreOk_ThenReirectToIndexPage()
        {
            //Arrange
            var createModel = new CreateViewModel
            {
                PersonName = "PName",
                DateAdded = DateTime.Today.AddDays(-20),
                SelectedGroupId = "1"
            };
            var controller = new PersonController(_logger.Object, _personRepository.Object);

            //Act
            var result = await controller.Create(createModel);

            //Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("Index", ((RedirectToActionResult)result).ActionName);
        }

        [Test]
        public async Task GivenNewPersonIsTobeAdded_WhenModelStateIsValidAndGroupIdIsInvalid_ThenStayOnCreatePage()
        {
            //Arrange
            var createModel = new CreateViewModel
            {
                PersonName = "PName",
                DateAdded = DateTime.Today.AddDays(-20),
                SelectedGroupId = "r"
            };
            var controller = new PersonController(_logger.Object, _personRepository.Object);

            //Act
            var result = await controller.Create(createModel);

            //Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = (ViewResult)result;
            var model = (CreateViewModel)viewResult.ViewData.Model;
            Assert.AreEqual(2, model.Group.Count());
        }

        [Test]
        public async Task GivenNewPersonIsTobeAdded_WhenModelStateIsInValid_ThenStayOnCreatePage()
        {
            //Arrange
            var createModel = new CreateViewModel
            {
                PersonName = "PName",
                DateAdded = DateTime.Today.AddDays(-20),
                SelectedGroupId = "1"
            };
            var controller = new PersonController(_logger.Object, _personRepository.Object);
            controller.ModelState.AddModelError("some", "error");
            //Act
            var result = await controller.Create(createModel);

            //Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = (ViewResult)result;
            var model = (CreateViewModel)viewResult.ViewData.Model;
            Assert.AreEqual(2, model.Group.Count());
        }

        [Test]
        public async Task GivenSearchIsPerformed_WhenPersonNameOrGroupNameIsNonEmptyString_ThenSearhIsSuccessfulAndPartialIndexViewIsLoaded()
        {
            //Arrange
            _personRepository.Setup(x => x.Search(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<PersonEntity>
            {
                new PersonEntity
                {
                    Id = 1,
                    Name = "John Smith",
                    DateAdded = DateTime.Now.AddDays(-20),
                    Group = new GroupEntity()
                },
                new PersonEntity
                {
                    Id = 2,
                    Name = "Jane Smith",
                    DateAdded = DateTime.Now.AddDays(-20),
                    Group = new GroupEntity()
                }
            });
            var controller = new PersonController(_logger.Object, _personRepository.Object);

            //Act
            var result = await controller.Search("name", "group");

            //Assert
            Assert.IsInstanceOf<PartialViewResult>(result);
            var viewResult = (PartialViewResult)result;
            var model = (IEnumerable<Person>)viewResult.ViewData.Model;
            Assert.AreEqual(2, model.Count());
        }

        [Test]
        public async Task GivenSearchIsPerformed_WhenPersonNameAndGroupNameAreEmptyString_ThenSearchIsNotSuccessfulAndContentViewIsResturned()
        {
            //Arrange
            _personRepository.Setup(x => x.Search(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<PersonEntity>
            {
                new PersonEntity
                {
                    Id = 1,
                    Name = "John Smith",
                    DateAdded = DateTime.Now.AddDays(-20),
                    Group = new GroupEntity()
                },
                new PersonEntity
                {
                    Id = 2,
                    Name = "Jane Smith",
                    DateAdded = DateTime.Now.AddDays(-20),
                    Group = new GroupEntity()
                }
            });
            var controller = new PersonController(_logger.Object, _personRepository.Object);

            //Act
            var result = await controller.Search("", "");

            //Assert
            Assert.IsInstanceOf<ContentResult>(result);
            var contentResult = (ContentResult)result;
            Assert.IsNotNull(contentResult.Content);
        }
    }
}
