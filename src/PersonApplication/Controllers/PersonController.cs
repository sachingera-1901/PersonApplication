using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersonApplication.DataAccess;
using PersonApplication.Models;

namespace PersonApplication.Controllers
{
    public class PersonController : Controller
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IPersonRespository _personRepository;

        public PersonController(ILogger<PersonController> logger, IPersonRespository _repository)
        {
            _logger = logger;
            _personRepository = _repository;
        }

        // GET: Person
        public async Task<IActionResult> Index()
        {
            try
            {
                var persons = await _personRepository.GetAllPersons();

                var model = persons.Select(x => new Person
                {
                    Id = x.Id,
                    Name = x.Name,
                    Group = new Group
                    {
                        Id = x.Group.Id,
                        Name = x.Group.Name
                    },
                    DateAdded = x.DateAdded

                });

                return View(model);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error whilst displaying all persons on home page");
                throw;
            }
        }


        // GET: Person/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var viewModel = new CreateViewModel();
                var groups = await _personRepository.GetAllGroups();

                viewModel.Group = groups.Select(
                    g => new SelectListItem
                    {
                        Value = g.Id.ToString(),
                        Text = g.Name
                    }).ToList();

                return View(viewModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error whilst display of Add new person page");
                throw;
            }
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel createPerson)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int intGroupId;
                    if (!int.TryParse(createPerson.SelectedGroupId, out intGroupId))
                        return View(createPerson);

                    //create person from person view model
                    var person = new Entities.Models.Person
                    {
                        Name = createPerson.PersonName,
                        DateAdded = createPerson.DateAdded,
                        Group = new Entities.Models.Group
                        {
                            Id = intGroupId
                        }
                    };
                    await _personRepository.AddPerson(person);
                    return RedirectToAction(nameof(Index));
                }
                return View(createPerson);
            }
            catch(Exception e)
            {
                _logger.LogError(e, $"Error whilst creating new person - {createPerson.PersonName}");
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Search(string SearchName, string SearchGroup)
        {
            try
            {
                if (String.IsNullOrEmpty(SearchName) && String.IsNullOrEmpty(SearchGroup))
                    return Content("Please enter person name or group");

                var persons = await _personRepository.Search(SearchName, SearchGroup);

                var model = persons.Select(x => new Person
                {
                    Id = x.Id,
                    Name = x.Name,
                    Group = new Group
                    {
                        Id = x.Group.Id,
                        Name = x.Group.Name
                    },
                    DateAdded = x.DateAdded

                });

                return PartialView("Index", model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error whilst searching for search params - search name {SearchName}, search group {SearchGroup}");
                throw;
            }
        }
    }
}
