using EntraTicket.Data;
using EntraTicket.Repositories; // Aseg�rate de que este using est� presente
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Agrega servicios al contenedor.
builder.Services.AddControllers();

// Configura la cadena de conexi�n
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registra los repositorios en el contenedor de servicios
builder.Services.AddScoped<EventRepository>(sp => new EventRepository(connectionString));
builder.Services.AddScoped<Metodos>(); // Registrar la clase Metodos

// Configuraci�n para JWT
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

// Configuraci�n para Swagger (Documentaci�n de API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuraci�n de middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configura el middleware para archivos est�ticos
app.UseStaticFiles(); // Permite servir archivos est�ticos desde wwwroot

app.UseHttpsRedirection();

// Configuraci�n de autenticaci�n y autorizaci�n
app.UseAuthentication(); // Aseg�rate de que esto est� antes de UseAuthorization()
app.UseAuthorization();

// Configura la p�gina de inicio
app.MapGet("/", () => Results.File("wwwroot/index.html"));

app.MapControllers();

app.Run();
