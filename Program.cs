using Pt_For_Me;
using Pt_For_Me.Interfaces;
using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


/*public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    services.AddDbContext<PtForMeContext>(options =>
    {
        options.UseSqlServer(Configuration.GetConnectionString("PtForMeDB"),
          sqlServerOptionsAction: sqlOptions =>
          {
              sqlOptions.EnableRetryOnFailure();
          }
            );
    });
}*/


//cors
builder.Services.AddCors(p => p.AddPolicy("corspolicy", build =>
{
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

//refrences for interface & repository
builder.Services.AddScoped<IPtForMeRepository, PtForMeRepository>();
builder.Services.AddTransient<IPtForMeRepository, PtForMeRepository>();
//for agora
builder.Services.AddHttpClient();
//for cors policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

//for the connection string to link db
builder.Services.AddDbContext<PtForMeContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});



//for the folder where we will be storing the images uploaded
AppDomain.CurrentDomain.SetData("AppDataDirectory", System.IO.Path.Combine(builder.Environment.ContentRootPath, "App_Data"));


//IWebHostEnvironment env;
//var env = builder.Environment.IsDevelopment;
//string baseDir = env.ContentRootPath;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//allow cors
app.UseCors("AllowAll");
app.UseCors("corspolicy");

app.MapControllers();

app.Run();