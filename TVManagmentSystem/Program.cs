using Microsoft.EntityFrameworkCore;
using TVManagmentSystem.DbContexti;
using TVManagmentSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<GlobalDbContext>(io =>
{
    io.UseSqlServer(builder.Configuration.GetConnectionString("GlobalString"));
});
builder.Services.AddScoped<IUserService,ChanellService>();


builder.Services.AddControllers();
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
