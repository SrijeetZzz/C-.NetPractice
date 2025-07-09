
// using MyApi.Models;
// using MyApi.Services;

// var builder = WebApplication.CreateBuilder(args);

// builder.Services.Configure<MongoDBSettings>(
//     builder.Configuration.GetSection("MongoDBSettings"));

// // builder.Services.AddSingleton<ProductService>();
// builder.Services.AddSingleton<CategoryService>();


// builder.Services.AddControllers();
// builder.Services.AddEndpointsApiExplorer();

// var app = builder.Build();

// app.UseHttpsRedirection();
// app.UseAuthorization();
// app.MapControllers();

// app.Run();

using MyApi.Models;
using MyApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure MongoDB settings
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));

// Register services
builder.Services.AddSingleton<CategoryService>();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()    // You can specify origins here
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Enable CORS
app.UseCors();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
