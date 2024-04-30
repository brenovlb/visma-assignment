namespace visma_assignment.Repository
{
    public class EmployeeStoreDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string EmployessCollectionName { get; set; } = null!;
    }
}
