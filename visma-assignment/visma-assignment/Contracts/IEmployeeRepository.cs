using visma_assignment.Model;

namespace visma_assignment.Contracts
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetById(string id);
        Task<IEnumerable<Employee>> GetByNameAndBirthdate(string name, DateTime startDate, DateTime endDate);
        Task<IEnumerable<Employee>> GetAll();
        Task<IEnumerable<Employee>> GetAllByBossId(string bossId);
        Task<object> GetCountAndAvgSalary(string role);
        Task<Employee> Add(Employee employee);
        Task<Employee> Update(Employee employee);
        Task<Employee> Update(string employeeId, decimal employeeSalary);
        Task Delete(string id);
        Task<Employee> GetByRole(string role);
    }
}