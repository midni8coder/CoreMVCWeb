using Microsoft.EntityFrameworkCore;

namespace CoreMinimalAPI
{
    class TodoDb : DbContext
    {
        public TodoDb(DbContextOptions<TodoDb> options)
            : base(options) { }
        public DbSet<Todo> Todos => Set<Todo>();
    }
}
