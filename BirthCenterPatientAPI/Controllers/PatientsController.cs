using BirthCenterPatientAPI.Models;
using BirthCenterPatientAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BirthCenterPatientAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : Controller
    {
        private readonly PatientContext _context;
        private readonly ILogger<PatientsController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;

        /// <summary>
        /// Constructor for the patient controller.
        /// </summary>
        /// <param name="context">The patient database context.</param>
        /// <param name="logger">The logger for logging during the execution of the controller.</param>
        /// /// <param name="hostingEnvironment">The hosting environment, providing information about the hosting environment.</param>
        public PatientsController(PatientContext context, ILogger<PatientsController> logger, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Retrieves the Postman collection containing saved requests from a JSON file.
        /// </summary>
        /// <remarks>
        /// This endpoint allows retrieving a Postman collection containing saved requests from a JSON file.
        /// The JSON file should be stored in the "PostmanCollection" folder within the application root directory.
        /// The name of the JSON file is expected to be "Patient API Demo.postman_collection.json".
        /// </remarks>
        /// <returns>
        /// If the JSON file exists, returns the content of the Postman collection as JSON.
        /// If the JSON file does not exist, returns a 404 Not Found response.
        /// </returns>
        [HttpGet("postman-collection")]
        public IActionResult GetPostmanCollection()
        {
            var postmanCollectionFileName = "Patient API Demo.postman_collection.json";
            var postmanCollectionFilePath = Path.Combine(_hostingEnvironment.ContentRootPath, "PostmanCollection", postmanCollectionFileName);

            if (!System.IO.File.Exists(postmanCollectionFilePath))
            {
                return NotFound();
            }

            var fileContent = System.IO.File.ReadAllText(postmanCollectionFilePath);

            return Content(fileContent, "application/json");
        }

        /// <summary>
        /// Retrieves information about a specific patient by their ID.
        /// </summary>
        /// <param name="id">The unique identifier of the patient.</param>
        /// <returns>
        /// If the patient exists, returns the information about the patient.
        /// If the patient does not exist, returns a 404 Not Found response.
        /// </returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(Guid id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            return patient;
        }

        /// <summary>
        /// Search for patients by birth date.
        /// </summary>
        /// <param name="birthDate">The birth date to search for patients.</param>
        /// <returns>A list of patients matching the specified birth date.</returns>
        [HttpGet("searchByBirthDate")]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatientsByBirthDate([FromQuery] DateTime birthDate)
        {
            var patients = await _context.Patients
                .Where(p => p.BirthDate.Date == birthDate.Date)
                .ToListAsync();

            if (patients == null || patients.Count == 0)
            {
                return NotFound();
            }

            return patients;
        }

        /// <summary>
        /// Search for patients within a specified date range.
        /// </summary>
        /// <param name="dateFrom">The start date of the date range.</param>
        /// <param name="dateTo">The end date of the date range.</param>
        /// <returns>A list of patients whose birth date falls within the specified date range.</returns>
        [HttpGet("searchByDateRange")]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatientsByDateRange([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
        {
            var patients = await _context.Patients
                .Where(p => p.BirthDate >= dateFrom && p.BirthDate <= dateTo)
                .ToListAsync();

            if (patients == null || patients.Count == 0)
            {
                return NotFound();
            }

            return patients;
        }

        /// <summary>
        /// Search for patients by last name.
        /// </summary>
        /// <param name="lastName">The last name to search for patients.</param>
        /// <returns>A list of patients with the specified last name.</returns>
        [HttpGet("searchByLastName")]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatientsByLastName([FromQuery] string lastName)
        {
            var patients = await _context.Patients
                .Where(p => p.LastName == lastName)
                .ToListAsync();

            if (patients.Count == 0)
            {
                return NotFound();
            }

            return patients;
        }


        /// <summary>
        /// Updates an existing patient record.
        /// </summary>
        /// <param name="id">The unique identifier of the patient.</param>
        /// <param name="patient">The updated patient object.</param>
        /// <returns>
        /// NoContent if the patient was successfully updated.
        /// BadRequest if the provided patient ID does not match the ID in the request body.
        /// NotFound if the patient with the specified ID does not exist.
        /// </returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient(Guid id, Patient patient)
        {
            if (id != patient.Id)
            {
                _logger.LogError($"Mismatch between patient ID in the URL and patient object: URL ID - {id}, Patient ID - {patient.Id}");
                return BadRequest("Mismatch between patient ID in the URL and patient object");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Entry(patient).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Patient record updated successfully: ID - {patient.Id}");

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
                {
                    _logger.LogError($"Patient with ID {id} not found");

                    return NotFound("Patient not found");
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating patient record: ID - {patient.Id}"); ;

                return StatusCode(500, "An error occurred while processing your request.");
            }
        }



        /// <summary>
        /// Creates a new patient record.
        /// </summary>
        /// <param name="patient">The patient object to be created.</param>
        /// <returns>
        /// CreatedAtAction with the newly created patient object if successful.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<Patient>> PostPatient(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"New patient record created: {patient.Id}");

                return CreatedAtAction("GetPatient", new { id = patient.Id }, patient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new patient record");

                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// Deletes a patient record by ID.
        /// </summary>
        /// <param name="id">The ID of the patient to be deleted.</param>
        /// <returns>
        /// NoContent if the deletion is successful, NotFound if the patient does not exist.
        /// </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(Guid id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                _logger.LogInformation($"Patient with ID {id} not found.");

                return NotFound();
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Patient with ID {id} deleted successfully.");

            return NoContent();
        }


        private bool PatientExists(Guid id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}
