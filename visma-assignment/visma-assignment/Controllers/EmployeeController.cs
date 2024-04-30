using Microsoft.AspNetCore.Mvc;
using visma_assignment.Contracts;
using visma_assignment.Model;
using visma_assignment.Model.Validations;

namespace visma_assignment.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeRepository _employeeRepo;

        public EmployeeController(ILogger<EmployeeController> logger,
                                    IEmployeeRepository employeeRepo)
        {
            _logger = logger;
            _employeeRepo = employeeRepo;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Employee>> Add(Employee employee)
        {
            try
            {
                var errors = new List<string>();
                var validator = new EmployeeValidator(_employeeRepo);
                var results = await validator.ValidateAsync(employee);

                if (!results.IsValid)
                {
                    foreach (var failure in results.Errors)
                    {
                        errors.Add(failure.ErrorMessage);
                        _logger.LogInformation($"Error: {failure.ErrorMessage}");
                    }
                    return NotFound(errors);
                }
                else
                {
                    var employeeAdded = await _employeeRepo.Add(employee);
                    return employeeAdded == null ? NotFound() : Ok(employeeAdded);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"There is a problem with the transaction: {ex.Message}");
                return NotFound();
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Employee>> Update(Employee employee)
        {
            try
            {
                var errors = new List<string>();
                var validator = new EmployeeValidator(_employeeRepo);
                var results = await validator.ValidateAsync(employee);

                if (!results.IsValid)
                {
                    foreach (var failure in results.Errors)
                    {
                        errors.Add(failure.ErrorMessage);
                        _logger.LogInformation($"Error: {failure.ErrorMessage}");
                    }
                    return NotFound(errors);
                }
                else
                {
                    var employeeUpdated = await _employeeRepo.Update(employee);
                    return employeeUpdated == null ? NotFound() : Ok(employeeUpdated);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("There is a problem with the transaction");
                return NotFound();
            }
        }

        [HttpPut("{employeeId}/{salary}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Employee>> UpdateSalary(string employeeId, decimal salary)
        {
            try
            {
                var employeeUpdated = await _employeeRepo.Update(employeeId, salary);
                return employeeUpdated == null ? NotFound() : Ok(employeeUpdated);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("There is a problem with the transaction");
                return NotFound();
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Employee>> Delete(string employeeId)
        {
            try
            {
                await _employeeRepo.Delete(employeeId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation("There is a problem with the transaction");
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Employee>> GetById(string id)
        {
            try
            {
                var employee = await _employeeRepo.GetById(id);
                return employee == null ? NotFound() : Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("There is a problem with the transaction");
                return NotFound();
            }
        }

        [HttpGet("{name}/{startDate}/{endDate}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Employee>> GetByNameAndBirthdate(string name,
                                                                DateTime startDate,
                                                                DateTime endDate)
        {
            try
            {
                if (startDate > endDate) return NotFound("The end date must be greater than start date");

                var employee = await _employeeRepo.GetByNameAndBirthdate(name, startDate, endDate);
                return employee == null ? NotFound() : Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("There is a problem with the transaction");
                return NotFound();
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Employee>> GetAll()
        {
            try
            {
                var employees = await _employeeRepo.GetAll();
                return employees == null ? NotFound() : Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("There is a problem with the transaction");
                return NotFound();
            }
        }

        [HttpGet("bosses/{bossId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Employee>> GetAllByBossId(string bossId)
        {
            try
            {
                var employee = await _employeeRepo.GetAllByBossId(bossId);
                return employee == null ? NotFound() : Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("There is a problem with the transaction");
                return NotFound();
            }
        }

        [HttpGet("roles/{role}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Employee>> GetCountAndAvgSalary(string role)
        {
            try
            {
                var employee = await _employeeRepo.GetCountAndAvgSalary(role);
                return employee == null ? NotFound() : Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("There is a problem with the transaction");
                return NotFound();
            }
        }
    }
}
