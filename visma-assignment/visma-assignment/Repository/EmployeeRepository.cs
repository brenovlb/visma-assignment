using Microsoft.Extensions.Options;
using MongoDB.Driver;
using visma_assignment.Contracts;
using visma_assignment.Model;

namespace visma_assignment.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IMongoCollection<Employee> _employeeCollection;

        public EmployeeRepository(IOptions<EmployeeStoreDatabaseSettings> employeeStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                                    employeeStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                                    employeeStoreDatabaseSettings.Value.DatabaseName);

            _employeeCollection = mongoDatabase.GetCollection<Employee>(
                                    employeeStoreDatabaseSettings.Value.EmployessCollectionName);
        }

        public async Task<Employee> Add(Employee employee)
        {
            await _employeeCollection.InsertOneAsync(employee);
            return employee;
        }

        public async Task Delete(string id)
        {
            await _employeeCollection.DeleteOneAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            return await _employeeCollection.Find(_ => true).ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetAllByBossId(string bossId)
        {
            return await _employeeCollection.Find(e => e.BossId == bossId).ToListAsync();
        }

        public async Task<Employee> GetBossById(string boosId)
        {
            return await _employeeCollection.Find(e => e.BossId == boosId).FirstOrDefaultAsync();
        }

        public async Task<Employee> GetById(string id)
        {
            return await _employeeCollection.Find(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Employee>> GetByNameAndBirthdate(string name, DateTime startDate, DateTime endDate)
        {
            return await _employeeCollection.Find(e => e.FirstName.ToLower().Contains(name.ToLower()) &&
                                                       e.Birthdate >= startDate &&
                                                       e.Birthdate <= endDate).ToListAsync();
        }

        public async Task<Employee> GetByRole(string role)
        {
            return await _employeeCollection.Find(e => e.Role.ToLower().Equals(role.ToLower())).FirstOrDefaultAsync();
        }

        public async Task<object> GetCountAndAvgSalary(string role)
        {
            var employees = await _employeeCollection.Find(e => e.Role.ToLower().Equals(role.ToLower())).ToListAsync();
            return new
            {
                AvarageSalary = employees.Average(e => e.CurrentSalary),
                EmployeeCount = employees.Count()
            };
        }

        public async Task<Employee> Update(string employeeId, decimal employeeSalary)
        {
            var employee = await _employeeCollection.Find(e => e.Id == employeeId).FirstOrDefaultAsync();
            employee.CurrentSalary = employeeSalary;
            await _employeeCollection.ReplaceOneAsync(e => e.Id == employeeId, employee);
            return employee;
        }

        public async Task<Employee> Update(Employee employee)
        {
            await _employeeCollection.ReplaceOneAsync(e => e.Id == employee.Id, employee);
            return employee;
        }
    }
}
