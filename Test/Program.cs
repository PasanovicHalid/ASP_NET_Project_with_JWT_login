using TestAPI.Extensions;
using TestLibrary.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add auto mapper
builder.Services.AddAutoMapper(typeof(Program));

// Add services to the container.
builder.Services.SetupDependencies();
builder.Services.SetupDBs(builder.Configuration);
builder.Services.SetupAuthentication(builder.Configuration);
builder.Services.SetupSettings(builder.Configuration);
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.SetupSwagger();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/swagger/V1/swagger.json", "Test Application");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
