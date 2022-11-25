using BBQN.AutoEnrolled.API.DataAccess;
using BBQN.AutoEnrolled.API.Integrations;
using BBQN.AutoEnrolled.API.Services;
using BBQN.DBFactory;
using Microsoft.VisualBasic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IDBFactory, DBFactory>();
builder.Services.AddSingleton<IIntegration, Integration> ();
builder.Services.AddSingleton<IAutoEnrollService, AutoEnrollService>();
builder.Services.AddSingleton<IAutoEnrollDbAdaptor, AutoEnrollDBAdaptor>();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
