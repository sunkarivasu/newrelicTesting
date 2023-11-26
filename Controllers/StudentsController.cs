using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using newRelicTestingApplication.Models;
using newRelicTestingApplication.Utils;
using System.Runtime.CompilerServices;

namespace newRelicTestingApplication.Controllers
{
    public class StudentsController : Controller
    {
        //private readonly testingContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly ILogger<StudentsController> _logger;
        private static readonly NewRelicEventApiClientUtils _newRelicEventApiClientUtils = new NewRelicEventApiClientUtils();

        //public StudentsController(testingContext dbContext, IConfiguration configuration)
        //{
        //    _dbContext = dbContext;
        //    _configuration = configuration;
        //}


        public StudentsController(IConfiguration configuration, ILogger<StudentsController> logger)
        {
            _configuration = configuration;
            _logger = logger;
            
        }

        public IActionResult Index()
        {
            var env = _configuration.GetValue<String>("Environment");
            //var Students = _dbContext.Students.ToList();
            List<Student> Students = new List<Student>();
            _logger.LogInformation("Fetching all students");
            Students.Add(new Student()
            {
                Id = 1,
                Name = "test1",
                Gender = "Male"
            });
            Students.Add(new Student()
            {
                Id = 2,
                Name = "test2",
                Gender = "Female"
            });
            var eventAttributes = new Dictionary<String, Object>()
            {
                { "endpoint","/students"},
                { "requestTime", DateTime.UtcNow},
                { "responseCount", Students.Count}
            };
            _logger.LogInformation($"First Student details::{Students[0].Name}");
            _newRelicEventApiClientUtils.CreateCustomEvent("endpointTest", eventAttributes);
            return View(Students);
        }

        [HttpGet]
        public IActionResult Create() {
            _logger.LogInformation("Fetching Student create form");
            return View();
        }

        [HttpPost]
        public IActionResult Create(String Name, String Gender)
        {
            //var Student = new Student()
            //{
            //    Name = Name,
            //    Gender = Gender == "None" ? null : Gender
            //};
            var eventAtrributs = new Dictionary<String, Object>()
            {
                {"endpoint","/students/create" },
                {"requestTime", DateTime.UtcNow},
                {"name",Name},
                {"gender", Gender}
            };
            _logger.LogInformation($"New Student with name {Name} ({Gender}) is Created");
            _newRelicEventApiClientUtils.CreateCustomEvent("endpointTest", eventAtrributs);
            //_dbContext.Students.Add(Student);
            //_dbContext.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
