using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UNLZ.Turnera.Manager.Entidades;
using UNLZ.Turnera.Manager.Managers;
using UNLZ.Turnera.Manager.Repositorios;
using UNLZ.Turnera.Web.Models;

namespace UNLZ.Turnera.Web.Controllers
{
    public class PacienteController : Controller

    {
        // Dependencia del manager de pacientes
        private IPacienteManager _pacienteManager;

        // Constructor que recibe el manager como dependencia
        public PacienteController(IPacienteManager pacienteManager)
        {
            _pacienteManager = pacienteManager;
        }

        // GET: PacienteController
        public ActionResult Index()
        {
            var pacientes = _pacienteManager.GetPacientes();
            return View(pacientes);
        }

        // GET: PacienteController/Details/5
        public ActionResult Details(string dniPaciente)
        {
            var paciente = _pacienteManager.GetPaciente(dniPaciente);
            PacienteModel pacienteModel = new PacienteModel();
            pacienteModel.model = paciente;
            return View(pacienteModel);
        }



        // GET: PacienteController/Create
        public ActionResult Create()
        {
            PacienteModel pacienteModel = new PacienteModel();
            pacienteModel.model = null;

            return View(pacienteModel);

        }

        // POST: PacienteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // Crea una nueva instancia de Paciente 
                Paciente paciente = new Paciente
                {
                    DniPaciente = collection["model.DniPaciente"],          
                    NombrePaciente = collection["model.NombrePaciente"],  
                    EmailPaciente = collection["model.EmailPaciente"],      
                    TelefonoPaciente = collection["model.TelefonoPaciente"]

                };
                // Llamamos al método CrearPaciente del manager para guardar al paciente en la base de datos y retornamos al Index
                _pacienteManager.CrearPaciente(paciente);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PacienteController/Edit/5
        public ActionResult Edit(string dniPaciente)
        {
            //Obtenemos el paciente
            var paciente = _pacienteManager.GetPaciente(dniPaciente);

            //Crea y asigna un modelo de paciente, y retorna la vista
            PacienteModel pacienteModel = new PacienteModel();
            pacienteModel.model = paciente;
            return View(pacienteModel);

        }

        // POST: PacienteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string dniPaciente, IFormCollection collection)
        {
            try
            {
                // Creamos una nueva instancia de Paciente con los valores enviados
                Paciente paciente = new Paciente
                {   
                    DniPaciente = dniPaciente.ToString(),
                    NombrePaciente = collection["model.NombrePaciente"],
                    EmailPaciente = collection["model.EmailPaciente"],
                    TelefonoPaciente = collection["model.TelefonoPaciente"]
                };

                // Llamamos al manager para realizar la modificación
                var result = _pacienteManager.ModificarPaciente(dniPaciente, paciente);

                if (result)
                {
                    // Redirigimos al índice si la modificación es exitosa
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Retornamos a la vista con un mensaje de error si falla
                    ModelState.AddModelError("", "No se pudo modificar al paciente.");
                    return View(new PacienteModel { model = paciente });
                }
            }
            catch
            {
                // En caso de excepción, retornamos la misma vista
                return View();
            }
        }


        // GET: PacienteController/Delete/5
        public ActionResult Delete(string dniPaciente)
        {
            //Busca el paciente por DNI, crea el modelo y lo retorna a la vista
            var paciente = _pacienteManager.GetPaciente(dniPaciente);
            PacienteModel pacienteModel= new PacienteModel();
            pacienteModel.model= paciente;
            return View(pacienteModel);
        }

        // POST: PacienteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string dniPaciente, IFormCollection collection)
        {
            try
            {
                // Llama al método para eliminar lógicamente al paciente
                var result = _pacienteManager.DeletePaciente(dniPaciente);
                if (result)
                {
                    // Si la eliminación es exitosa, redirige al índice
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Si falla, retorna a la vista con un mensaje de error
                    ModelState.AddModelError("", "No se pudo eliminar al paciente.");
                    var paciente = _pacienteManager.GetPaciente(dniPaciente);
                    return View(new PacienteModel { model = paciente });
                }
            }
            catch
            {
                // Muestra un mensaje de error genérico en caso de excepciones
                return View();
            }
        }

    }
}
