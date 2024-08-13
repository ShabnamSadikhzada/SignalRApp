using Humanizer;
using Microsoft.AspNetCore.SignalR;
using SignalRApplication.Models;
using SignalRApplication.Repositories;
using System.Data;
using System.Data.SqlClient;

namespace SignalRApplication.Services;

public class BaseService<T> : IRepository<T> where T : BaseEntity
{
    private readonly IConfiguration _configuration;
    private readonly IHubContext<SignalRServer> _hubContext;

    public BaseService(IConfiguration configuration, IHubContext<SignalRServer> hubContext)
    {
        _configuration = configuration;
        _hubContext = hubContext;
    }

    public IEnumerable<T> GetAll()
    {
        string? connectionString = _configuration.GetConnectionString("default");
        string tableName = typeof(T).Name.Pluralize();

        var items = new List<T>();

        using SqlConnection connection = new(connectionString);
        
        connection.Open();
        SqlDependency.Start(connectionString);

        using SqlCommand cmd = new();
        cmd.Connection = connection;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = $"select * from {tableName}" ;

        SqlDependency dependency = new(cmd);
        dependency.OnChange += Dependency_OnChange;

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var item = Activator.CreateInstance<T>();
            foreach ( var property in item.GetType().GetProperties())
            {
                property.SetValue(item, reader[property.Name]);
            }
            items.Add(item);
        }


        return items;
    }

    private void Dependency_OnChange(object sender, SqlNotificationEventArgs e)
    {
        _hubContext.Clients.All.SendAsync("refreshTypes");
    }
}

public class ProductService : BaseService<Product>, IProductRepository
{
    public ProductService(
        IConfiguration configuration, 
        IHubContext<SignalRServer> hubContext) 
        : base(configuration, hubContext)
    {
    }
}

public class CategoryService : BaseService<Category>, ICategoryRepository
{
    public CategoryService(
        IConfiguration configuration, 
        IHubContext<SignalRServer> hubContext) 
        : base(configuration, hubContext)
    {
    }
}
