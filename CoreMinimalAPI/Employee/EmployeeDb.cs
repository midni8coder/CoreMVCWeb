using Microsoft.EntityFrameworkCore;

namespace CoreMinimalAPI.Todo
{
    class EmployeeDb : DbContext
    {
        public EmployeeDb(DbContextOptions<EmployeeDb> options)
            : base(options) { }
        public DbSet<Employee.Employee> Employees => Set<Employee.Employee>();
    }
}
