using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UNLZ.Turnera.Manager.Repositorios;

namespace UNLZ.Turnera.Web.Controllers
{
    public class LoginController : Controller
    {

        private readonly IUsuarioRepository _usuarioRepository;

        public LoginController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public IActionResult Index()
        {
            // Verificamos si el usuario ya ha iniciado sesion
            if (HttpContext.User.Identities.First().IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // LOGIN: Logueo e inicio del proceso de autenticación con Google
        public async Task Login()
        {
            // Realizamos este proceso mediante los esquemas de autenticación de Google
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
               new AuthenticationProperties
               {
                   RedirectUri = Url.Action("GoogleResponse")
               });
        }

        // RESPONSE: Manejo de la respuesta de autenticación de Google
        [HttpGet("/signin-google")]
        public async Task<IActionResult> GoogleResponse()
        {
            // Autenticamos mediante el equema de CookieAuthentication para obtener el accesso del usuario
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;

            // Asignar rol de Administrador si el email coincide
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, result.Principal.FindFirst(ClaimTypes.Name)?.Value ?? ""),
                new Claim(ClaimTypes.Email, email)
            };

            if (email == "admin@example.com") // reemplaza con el email del administrador con google, alguno de nuestros mails
            {
                claims.Add(new Claim(ClaimTypes.Role, "Administrador"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "Usuario"));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            // Redirijimos al usuario a la acción Index del controlador Home
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        // Login con base de datos
        [HttpPost]
        public async Task<IActionResult> LoginWithCredentials(string email, string password)
        {
            var usuario = _usuarioRepository.GetUsuarioPorEmailYContraseña(email, password);
            if (usuario != null)
            {
                // Crear claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                    new Claim(ClaimTypes.Email, usuario.EmailUsuario),
                    new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()) //para el boton alquilar
                };

                // Asignar rol en función del correo electrónico
                if (usuario.EmailUsuario == "admin@email.com") // reemplaza con el email del administrador
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Administrador"));
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Usuario"));
                }

                // Crear la identidad y firmar la cookie de autenticación
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Si el usuario no es encontrado, muestra el mensaje de error en la vista
                ViewBag.ErrorMessage = "Email o contraseña incorrectos";
                return View("Index");
            }

            // Manejar login fallido
            //ModelState.AddModelError("", "Email o contraseña incorrectos");
            //return View("Index");
        }

        // LOGOUT: Cierre de sesión
        public async Task<IActionResult> Logout()
        {
            // Cerramos la sesión del usuario
            await HttpContext.SignOutAsync();

            // Rederijimos al Index
            return RedirectToAction("Index");
        }
    }
}
