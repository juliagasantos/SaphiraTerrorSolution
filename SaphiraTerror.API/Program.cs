using EmailAPI.Services;
using Microsoft.EntityFrameworkCore;
using SaphiraTerror.API.Interfaces;
using SaphiraTerror.Data;
using SaphiraTerror.Interfaces;
using SaphiraTerror.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//base de dados
builder.Services.AddDbContext<SaphiraTerrorDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro dos REPOSITÓRIOS (injeção de dependência)
builder.Services.AddScoped<IFilmeRepository, FilmeRepository>();
builder.Services.AddScoped<IGeneroRepository, GeneroRepository>();

//services
builder.Services.AddScoped<IEmailService, EmailService>();

//configuração do cors
//
builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
     builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
}));


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

//ativar cors
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
