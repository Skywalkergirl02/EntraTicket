using EntraTicket.Data;
using EntraTicket.Repositories; // Asegúrate de que este using esté presente
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Agrega servicios al contenedor.
builder.Services.AddControllers();

// Configura la cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registra los repositorios en el contenedor de servicios
builder.Services.AddScoped<EventRepository>(sp => new EventRepository(connectionString));
builder.Services.AddScoped<Metodos>(); // Registrar la clase Metodos

// Configuración para JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Configuración para Swagger (Documentación de API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuración de middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configura el middleware para archivos estáticos
app.UseStaticFiles(); // Permite servir archivos estáticos desde wwwroot

app.UseHttpsRedirection();

// Configuración de autenticación y autorización
app.UseAuthentication(); // Asegúrate de que esto esté antes de UseAuthorization()
app.UseAuthorization();

// Configura la página de inicio
app.MapGet("/", () => Results.File("wwwroot/index.html"));

app.MapControllers();

app.Run();
