using Microsoft.EntityFrameworkCore;

namespace CoreMinimalAPI.Todo
{
    class TodoDb : DbContext
    {
        public TodoDb(DbContextOptions<TodoDb> options)
            : base(options) { }
        public DbSet<Todo> Todos => Set<Todo>();
        //public DbSet<Employee.Employee> Employees => Set<Employee.Employee>();
    }
}
