
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
using MyApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configure MongoDB settings
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));

// Register services
// builder.Services.AddSingleton<CategoryService>();
// builder.Services.AddSingleton<SubCategoryService>();
builder.Services.AddSingleton<SubUserServices>();
builder.Services.AddSingleton<UserService>();


// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()    
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//jwt middleware
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

// Enable CORS
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
