using CoreMinimalAPI.Todo;
using Microsoft.EntityFrameworkCore;

namespace CoreMinimalAPI.Employee
{
    public class EmployeeApi
    {
        public static void MapRoutes(WebApplication app)
        {
            var employeeGroup = app.MapGroup("/employees")
                .RequireAuthorization("admin_policy");

            employeeGroup.AddEndpointFilter((context,next) =>
            {
                app.Logger.LogInformation("/employee group filters");
                return next(context);
            });

            employeeGroup.MapGet("/", async (EmployeeDb db) =>
                await db.Employees.ToListAsync());

            employeeGroup.MapGet("/{id}", async (int id, EmployeeDb db) =>
                await db.Employees.FindAsync(id)
                    is Employee employee
                        ? Results.Ok(employee)
                        : Results.NotFound());

            employeeGroup.MapPost("/", async (Employee employee, EmployeeDb db) =>
            {
                db.Employees.Add(employee);
                await db.SaveChangesAsync();

                return Results.Created($"/employees/{employee.Id}", employee);
            });

            employeeGroup.MapPut("/{id}", async (int id, Employee inputeEmployee, EmployeeDb db) =>
            {
                var employee = await db.Employees.FindAsync(id);

                if (employee is null) return Results.NotFound();

                employee.Name = inputeEmployee.Name;
                employee.Email= inputeEmployee.Email;
                employee.Age = inputeEmployee.Age;
                employee.DepartmentId   = inputeEmployee.DepartmentId;


                await db.SaveChangesAsync();

                return Results.NoContent();
            });

            employeeGroup.MapDelete("/{id}", async (int id, EmployeeDb db) =>
            {
                if (await db.Employees.FindAsync(id) is Employee employee)
                {
                    db.Employees.Remove(employee);
                    await db.SaveChangesAsync();
                    return Results.NoContent();
                }

                return Results.NotFound();
            });
        }
    }
}
