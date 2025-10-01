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

//repositories
builder.Services.AddScoped<IFilmeRepository, FilmeRepository>();
builder.Services.AddScoped<IGeneroRepository, GeneroRepository>();

//services
builder.Services.AddScoped<IEmailService, EmailService>();

//configura��o do CORS
//n�o esquecer de colocar enbableCors as controllers
builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
}));

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

//ativar cors
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();