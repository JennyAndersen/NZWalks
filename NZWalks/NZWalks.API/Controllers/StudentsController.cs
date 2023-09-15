using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{
    // https://localhost:portnumber/api/students
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        //GET: 
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            string[] StudentNames = new string[] { "John", "Jane", "Mark" };
            return Ok(StudentNames);
        }

        
    }
}
