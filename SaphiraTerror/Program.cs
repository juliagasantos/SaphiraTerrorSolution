using Microsoft.EntityFrameworkCore;
using SaphiraTerror.Data;
using SaphiraTerror.Interfaces;
using SaphiraTerror.Repositories;

var builder = WebApplication.CreateBuilder(args);

//base de dados
builder.Services.AddDbContext<SaphiraTerrorDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ======================================================
// Registro dos REPOSIT�RIOS (inje��o de depend�ncia)
// ======================================================
// Cada interface � ligada � sua implementa��o concreta.
// AddScoped ? ciclo de vida: uma inst�ncia por requisi��o HTTP.
builder.Services.AddScoped<IFilmeRepository, FilmeRepository>();
builder.Services.AddScoped<IGeneroRepository, GeneroRepository>();
builder.Services.AddScoped<IClassificacaoRepository, ClassificacaoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ITipoUsuarioRepository, TipoUsuarioRepository>();

// ======================================================
// Autentica��o com COOKIES
// ======================================================
// Define "SaphiraAuth" como esquema de autentica��o padr�o.
// - LoginPath: rota para login quando usu�rio n�o autenticado.
// - AccessDeniedPath: rota de acesso negado para roles.
// - ExpireTimeSpan: dura��o do cookie de autentica��o.
// - SlidingExpiration: renova o cookie se o usu�rio continuar ativo.
builder.Services.AddAuthentication("SaphiraAuth")
    .AddCookie("SaphiraAuth", options =>
    {
        options.LoginPath = "/Usuario/Login";             // P�gina de login
        options.AccessDeniedPath = "/Usuario/AcessoNegado"; // P�gina de acesso negado
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Expira em 30 min
        options.SlidingExpiration = true;                  // Renova��o autom�tica
    });



// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

//autentica��o

//repositories

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();