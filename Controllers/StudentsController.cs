using Microsoft.AspNetCore.Mvc;
using newRelicTestingApplication.Models;
using newRelicTestingApplication.Utils;
using System.Runtime.CompilerServices;

namespace newRelicTestingApplication.Controllers
{
    public class StudentsController : Controller
    {
        private readonly testingContext _dbContext;
        private readonly IConfiguration _configuration;
        private static readonly NewRelicEventApiClientUtils _newRelicEventApiClientUtils = new NewRelicEventApiClientUtils();

        public StudentsController(testingContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var env = _configuration.GetValue<String>("Environment");
            //var Students = _dbContext.Students.ToList();
            List<Student> Students = new List<Student>();
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
            _newRelicEventApiClientUtils.CreateCustomEvent("endpointTest", eventAttributes);
            return View(Students);
        }

        [HttpGet]
        public IActionResult Create() {
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
            _newRelicEventApiClientUtils.CreateCustomEvent("endpointTest", eventAtrributs);
            //_dbContext.Students.Add(Student);
            //_dbContext.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
