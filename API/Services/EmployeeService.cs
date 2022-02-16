using API.Models;
using API.Models.DbSettings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API.Services;

public class EmployeeService
{
    private readonly DbSettings _dbSettings;
    private readonly IMongoCollection<Employee> _empCollection;
    public EmployeeService(IOptions<DbSettings> settings)
    {
        _dbSettings = settings.Value;
        _empCollection = new MongoClient(_dbSettings.ConnectionString)
            .GetDatabase(_dbSettings.DbName)
            .GetCollection<Employee>(_dbSettings.Collections.Employee);
    }

    public async Task<IEnumerable<Employee>> GetAsync()
    {
        return await _empCollection.Find(e => true).ToListAsync();
    }
    public async Task<Employee> GetAsync(string id)
    {
        return await _empCollection.Find(e => e.Id == id).FirstOrDefaultAsync();
    }
    public async Task CreateAsync(Employee employee)
    {
        if (employee.DateOfJoining == default)
            employee.DateOfJoining = DateTime.UtcNow;

        await _empCollection.InsertOneAsync(employee);
    }
    public async Task UpdateAsync(Employee employee)
    {
        var filter = Builders<Employee>.Filter.Eq("Id", employee.Id);
        Employee oldEmpData = await _empCollection.Find(e => e.Id == employee.Id).FirstOrDefaultAsync();

        if (string.IsNullOrWhiteSpace(employee.Name))
            employee.Name = oldEmpData.Name;

        if (string.IsNullOrWhiteSpace(employee.DepartmentId))
            employee.DepartmentId = oldEmpData.DepartmentId;

        if (employee.DateOfJoining == default)
            employee.DateOfJoining = oldEmpData.DateOfJoining;

        if (employee.ImageName == "anonymous.png")
            employee.ImageName = oldEmpData.ImageName;

        var update = Builders<Employee>.Update.Set("Name", employee.Name)
                                              .Set("DepartmentId", employee.DepartmentId)
                                              .Set("ImageName", employee.ImageName)
                                              .Set("DateOfJoining", employee.DateOfJoining);

        await _empCollection.UpdateOneAsync(filter, update);
    }
    public async Task DeleteAsync(string id)
    {
        await _empCollection.DeleteOneAsync(e => e.Id == id);
    }
}
