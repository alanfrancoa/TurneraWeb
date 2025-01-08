using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UNLZ.Turnera.Manager.Entidades;
using UNLZ.Turnera.Manager.Repositorios;

namespace UNLZ.Turnera.Web.Controllers
{
    public class UsuarioController : Controller
    {

        private readonly ITurnoRepository _turnoRepository;

        public UsuarioController(ITurnoRepository turnoRepository)
        {
            _turnoRepository = turnoRepository;
        }

        // GET
        public ActionResult Index()
        {
            var turnosJson = TempData["Turnos"] as string;

            if (!string.IsNullOrEmpty(turnosJson))
            {
                var turnos = JsonConvert.DeserializeObject<IEnumerable<Turno>>(turnosJson);
                return View(turnos);
            }

            return View(Enumerable.Empty<Turno>());
        }

        // GET: UsuarioController
        public ActionResult Buscador()
        {
            return View();
        }

        // POST: Buscar turnos por dni Paciente
        [HttpPost]
        public ActionResult BuscarTurnosPorDni(string dni)
        {
            try
            {
                // Validación de entrada
                if (string.IsNullOrWhiteSpace(dni))
                {
                    TempData["Mensaje"] = "Debe ingresar un DNI.";
                    return RedirectToAction("Buscador");
                }

                var turnosPaciente = _turnoRepository.GetTurnosPorPaciente(dni);

                if (turnosPaciente == null || !turnosPaciente.Any())
                {
                    TempData["Mensaje"] = "No se encontraron turnos para el DNI ingresado.";
                    return RedirectToAction("Buscador");
                }

                // Serializar la lista de turnos a JSON
                TempData["Turnos"] = JsonConvert.SerializeObject(turnosPaciente);
                TempData.Keep("Turnos");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Registro del error y mensaje genérico para el usuario
                Console.Error.WriteLine($"Error al buscar turnos: {ex.Message}");
                TempData["Error"] = "Ocurrió un problema al buscar los turnos. Intente nuevamente.";
                return RedirectToAction("Buscador");
            }
        }

        // GET: UsuarioController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UsuarioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsuarioController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsuarioController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
