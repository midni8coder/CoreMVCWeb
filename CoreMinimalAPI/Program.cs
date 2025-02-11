using CoreMinimalAPI.Employee;
using CoreMinimalAPI.Todo;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Adding DB context and developer exception filter int Dependency Injection(DI)
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDbContext<EmployeeDb>(opt => opt.UseInMemoryDatabase("EmployeeList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddCors();
builder.Services.AddAuthentication().AddJwtBearer();//AddJwtBearer("LocalAuthIssuer"); ;
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("admin_policy", policy =>
    {
        policy.RequireRole("admin");
        policy.RequireClaim("scope", "read_employee");
    });
// Adding Swagger API testing
builder.Services.AddEndpointsApiExplorer();
// We should not add a swagger document multiple times
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "TodoAPI";
    config.Title = "TodoAPI v1";
    config.Version = "v1";
});
var app = builder.Build();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "TodoAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

app.MapGet("/", () =>
    "Hello world..!!")
    .AddEndpointFilter((context, next) =>
    {
        app.Logger.LogInformation("Default filter");
        app.Logger.LogInformation($"Bearer token : {Convert.ToString(context.HttpContext.Request.Headers["Authorization"])}");
        return next(context);
    })
    .RequireAuthorization("admin_policy");

TodoApi.MapRoutes(app);
EmployeeApi.MapRoutes(app);


app.Run();