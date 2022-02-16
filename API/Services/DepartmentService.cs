using API.Models;
using API.Models.DbSettings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace API.Services;

public class DepartmentService
{
    private readonly DbSettings _dbSettings;
    private readonly IMongoCollection<Department> _deptCollection;
    public DepartmentService(IOptions<DbSettings> settings)
    {
        _dbSettings = settings.Value;
        _deptCollection = new MongoClient(_dbSettings.ConnectionString)
            .GetDatabase(_dbSettings.DbName)
            .GetCollection<Department>(_dbSettings.Collections.Department);
    }

    public async Task<IEnumerable<Department>> GetAsync()
    {
        return await _deptCollection.Find(d => true).ToListAsync();
    }
    public async Task<Department> GetAsync(string id)
    {
        return await _deptCollection.Find(d => d.Id == id).FirstOrDefaultAsync();
    }
    public async Task CreateAsync(Department department)
    {
        await _deptCollection.InsertOneAsync(department);
    }
    public async Task UpdateAsync(Department department)
    {
        await _deptCollection.ReplaceOneAsync(d => d.Id == department.Id, department);
    }
    public async Task DeleteAsync(string id)
    {
        await _deptCollection.DeleteOneAsync(d => d.Id == id);
    }
}
