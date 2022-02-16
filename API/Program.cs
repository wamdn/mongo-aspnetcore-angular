using API.Models;
using API.Models.DbSettings;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<DbSettings>(builder.Configuration.GetSection("Database"));
builder.Services.AddSingleton<DepartmentService>();
builder.Services.AddSingleton<EmployeeService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowOrigins", builder =>
    {
        builder.WithOrigins("http://localhost:4200", "http://127.0.0.1:4200")
                .WithMethods("GET", "POST", "PUT", "DELETE")
                .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(builder.Environment.WebRootPath),
    RequestPath = "/Public"
});

app.UseCors("AllowOrigins");

/**
 * Departments Routes
 */

// GET /api/departments
app.MapGet("/api/departments", async (DepartmentService deptService) => 
    await deptService.GetAsync());

// GET /api/departments/{id}
app.MapGet("/api/departments/{id:regex(^[0-9a-f]{{24}}$)}", async (string id, DepartmentService deptService) =>
    await deptService.GetAsync(id));

// POST /api/departments
app.MapPost("/api/departments", async (Department dept, DepartmentService deptService) =>
    await deptService.CreateAsync(dept));

// PUT /api/departments
app.MapPut("/api/departments", async (Department dept, DepartmentService deptService) =>
    await deptService.UpdateAsync(dept));

// DELETE /api/employees/{id}
app.MapDelete("/api/departments/{id:regex(^[0-9a-f]{{24}}$)}", async (string id, DepartmentService deptService) =>
    await deptService.DeleteAsync(id));

/**
 * Employees Routes
 */

// GET /api/employees
app.MapGet("/api/employees", async (EmployeeService empService) =>
    await empService.GetAsync());

// GET /api/employees/{id}
app.MapGet("/api/employees/{id:regex(^[0-9a-f]{{24}}$)}", async (string id, EmployeeService empService) =>
    await empService.GetAsync(id));

// POST /api/employees
app.MapPost("/api/employees", async (Employee emp, EmployeeService empService) =>
    await empService.CreateAsync(emp));

// PUT /api/employees
app.MapPut("/api/employees", async (Employee emp, EmployeeService empService) =>
    await empService.UpdateAsync(emp));

// DELETE /api/employees/{id}
app.MapDelete("/api/employees/{id:regex(^[0-9a-f]{{24}}$)}", async (string id, EmployeeService empService) =>
    await empService.DeleteAsync(id));

// POST /api/employees/savefile
app.MapPost("/api/employees/savefile", async (HttpContext ctx, IWebHostEnvironment env) =>
{
    try
    {
        if (!ctx.Request.Form.Files.Any()) 
            throw new InvalidOperationException();

        IFormFile photo = ctx.Request.Form.Files[0];
        string ext = Path.GetExtension(photo.FileName);
        string photoFileName = Guid.NewGuid().ToString() + ext;
        string photoPath = Path.Combine(env.WebRootPath, "Photos", photoFileName);

        using FileStream fs = new (photoPath, 
                                   FileMode.CreateNew, 
                                   FileAccess.Write, 
                                   FileShare.Write, 
                                   4096, 
                                   useAsync: true);

        await photo.CopyToAsync(fs);

        await ctx.Response.WriteAsJsonAsync(photoFileName);
    }
    catch
    {
        await ctx.Response.WriteAsJsonAsync("anounymous.png");
    }
});

// Testing if api works
// GET /test
app.MapGet("/test", () => "Hello, world!");

app.Run();