using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> logger;
        private readonly IPersonService personService;

        public PersonController(IPersonService personService, ILogger<PersonController> logger)
        {
            this.logger = logger;
            this.personService = personService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Person person)
        {
            if (person is null)
            {
                string error = "Person payload is not valid";
                logger.LogError(error);
                return new BadRequestObjectResult(error);
            }

            string response = await personService.SavePerson(person);
            if (string.IsNullOrEmpty(response) || response.StartsWith("Error:"))
            {
                logger.LogError(response);
                return new BadRequestObjectResult(response);
            }

            return new OkObjectResult(new { response });
        }
    }
}
