using UNLZ.Turnera.Manager.Managers;
using UNLZ.Turnera.Manager.Repositorios;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using UNLZ.Turnera.Manager.Entidades;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//IMPORTACIONES- traemos nuestro manager de entidad//
builder.Services.AddScoped<IEspecialidadManager, EspecialidadManager>();
builder.Services.AddScoped<IProfesionalManager, ProfesionalManager>();
builder.Services.AddScoped<ITurnoManager, TurnoManager>();
builder.Services.AddScoped<IPacienteManager, PacienteManager>();
builder.Services.AddScoped<IDisponibilidadManager, DisponibilidadManager>();

//IMPORTACIONES - Importacion de los repositorios//


builder.Services.AddScoped<IEspecialidadRepository>(_ => new EspecialidadRepository(builder.Configuration["Db:ConnectionString"]));
builder.Services.AddScoped<_ProfesionalRepository>(_ => new ProfesionalRepository(builder.Configuration["Db:ConnectionString"]));
builder.Services.AddScoped<ITurnoRepository>(_ => new TurnoRepository(builder.Configuration["Db:ConnectionString"]));
builder.Services.AddScoped<IDisponibilidadRepository>(_ => new DisponibilidadRepository(builder.Configuration["Db:ConnectionString"]));
builder.Services.AddScoped<IPacienteRepository>(_ => new PacienteRepository(builder.Configuration["Db:ConnectionString"]));
builder.Services.AddScoped<IUsuarioRepository>(_ => new UsuarioRepository(builder.Configuration["Db:ConnectionString"]));


// AUTENTICACION - Seteamos la autenticación de Google OAuth //

builder.Services.AddAuthentication(opciones =>
{

    // Establece el esquema de autenticación predeterminado como Cookie.
    opciones.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;

    // Establece el esquema de inicio de sesión predeterminado como Cookie.
    opciones.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

    // Establece el esquema de desafío predeterminado como Google (para redirigir a Google en caso de no autenticación).
    opciones.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
    // Añade la autenticación basada en cookies.
    .AddCookie(opciones =>
    {
        opciones.LoginPath = "/Login/Index";
    }
    )
    // Añade la autenticación con Google.
    .AddGoogle(GoogleDefaults.AuthenticationScheme, opciones =>
    {
        // Configura el ClientId utilizando una clave de configuración.
        opciones.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value;

        // Configura el ClientSecret utilizando una clave de configuración.
        opciones.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;

        // Evento que se activa cuando se crea un ticket de autenticación (al iniciar sesión).
        opciones.Events.OnCreatingTicket = ctx =>
        {
            // Obtiene el servicio `IUsuarioRepository` desde el contexto HTTP.
            var usuarioServicio = ctx.HttpContext.RequestServices.GetRequiredService<IUsuarioRepository>();

            // Obtiene el email del usuario actual del contexto 
            var email = ctx.Identity.FindFirst(ClaimTypes.Email)?.Value;

            // Obtiene el identificador único del usuario en Google (Google Name Identifier).
            var googleNameIdentifier = ctx.Identity.Claims
                .FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value.ToString();

           if (string.IsNullOrEmpty(googleNameIdentifier))
                throw new InvalidOperationException("El Google Name Identifier no está presente en los claims.");

            // Busca un usuario existente en la base de datos usando el identificador de Google.
            var usuario = usuarioServicio.GetUsuarioPorGoogleSubject(googleNameIdentifier);
            int idUsuario = 0;

            // Si el usuario no existe en la base de datos:
            if (usuario == null)
            {
                // Crea una nueva instancia del usuario con los datos del ticket de Google.
                Usuario usuarioNuevo = new Usuario
                {
                    NombreUsuario = ctx.Identity.FindFirst(ClaimTypes.GivenName)?.Value,
                    ApellidoUsuario = ctx.Identity.FindFirst(ClaimTypes.Surname)?.Value,
                    GoogleIdentificador = googleNameIdentifier,
                    EmailUsuario = email,
                };

                // Guarda el usuario nuevo en la base de datos y obtiene su ID.
                idUsuario = usuarioServicio.CrearUsuario(usuarioNuevo);
            }
            else
            {
                // Si el usuario existe, asigna su ID.
                idUsuario = usuario.IdUsuario;
            }

            // Asignar rol basado en el email
            var role = email == "admin@example.com" ? "Administrador" : "Usuario";

            // Agregamos Claims personalizadas
            ctx.Identity.AddClaim(new Claim(ClaimTypes.Role, role));
            ctx.Identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, idUsuario.ToString()));

            // Obtiene el token de acceso de Google y lo agrega como reclamación.
            var accessToken = ctx.AccessToken;
            ctx.Identity.AddClaim(new System.Security.Claims.Claim("accessToken", accessToken));

            // Completa la tarea de creación de ticket.
            return Task.CompletedTask;
        };
        opciones.Events.OnRemoteFailure = ctx =>
        {
            Console.WriteLine($"Error de autenticación: {ctx.Failure.Message}");
            ctx.Response.Redirect("/Login/Index"); // Redirigir al login o a una página de error personalizada
            ctx.HandleResponse(); // Detiene el procesamiento adicional
            return Task.CompletedTask;
        };
    });

var app = builder.Build();

// Configura el HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");


app.Run();
