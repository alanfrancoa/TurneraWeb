using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UNLZ.Turnera.Manager.Entidades;
using UNLZ.Turnera.Manager.Managers;
using UNLZ.Turnera.Manager.Repositorios;
using UNLZ.Turnera.Web.Models;

namespace UNLZ.Turnera.Web.Controllers
{
    public class ProfesionalController : Controller
    {   
        //Creacion de dependencias
        private IProfesionalManager _profesionalManager;
        private IEspecialidadRepository _especialidadRepository;

        //constructor
        public ProfesionalController(IProfesionalManager profesionalManager, IEspecialidadRepository especialidadRepo) 
        {
            _profesionalManager = profesionalManager;
            _especialidadRepository = especialidadRepo;
        }

        // GET: ProfesionalController
        public ActionResult Index()
        {   
            var profesionales = _profesionalManager.GetProfesionales();
            return View(profesionales);
        }

        // GET: ProfesionalController/Details/5
        public ActionResult Details(int id)
        {
            var profesional = _profesionalManager.GetProfesional(id);

            ProfesionalModel profesionalModel = new ProfesionalModel();
            profesionalModel.model = profesional;
            profesionalModel.ListaEspecialidadesItem = new List<SelectListItem>();
            var especialidades = _especialidadRepository.GetEspecialidades();
            foreach(var especialidad in especialidades)
            {
                profesionalModel.ListaEspecialidadesItem.Add(new SelectListItem { Value = especialidad.IdEspecialidad.ToString(), Text = especialidad.NombreEspecialidad });
            }

            return View(profesionalModel);
        }

        // GET: ProfesionalController/Create
        public ActionResult Create()
        {
            ProfesionalModel profesionalModel = new ProfesionalModel();
            profesionalModel.model = null;

            profesionalModel.ListaEspecialidadesItem = new List<SelectListItem>();
            var especialidades = _especialidadRepository.GetEspecialidadesActivas();
            foreach (var especialidad in especialidades)
            {
                profesionalModel.ListaEspecialidadesItem.Add(new SelectListItem { Value = especialidad.IdEspecialidad.ToString(), Text = especialidad.NombreEspecialidad });
            }


            return View(profesionalModel);
        }

        // POST: ProfesionalController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Profesional profesional = new Profesional
                {
                    NombreProfesional = collection["model.NombreProfesional"],
                    EmailProfesional = collection["model.EmailProfesional"],
                    TelefonoProfesional = collection["model.TelefonoProfesional"],
                    IdEspecialidad = int.Parse(collection["model.IdEspecialidad"])

                };
                _profesionalManager.CrearProfesional(profesional);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProfesionalController/Edit/5
        public ActionResult Edit(int id)
        {
            var profesional = _profesionalManager.GetProfesional(id);
            var especialidades = _especialidadRepository.GetEspecialidadesActivas();

            ProfesionalModel profesionalModel = new ProfesionalModel();
            profesionalModel.model = profesional;

            profesionalModel.ListaEspecialidadesItem = new List<SelectListItem>();
            foreach (var especialidad in especialidades)
            {
                profesionalModel.ListaEspecialidadesItem.Add(new SelectListItem { Value = especialidad.IdEspecialidad.ToString(), Text = especialidad.NombreEspecialidad });
            }
            return View(profesionalModel);
        }

        // POST: ProfesionalController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                Profesional profesional = new Profesional
                {
                    NombreProfesional = collection["model.NombreProfesional"],
                    EmailProfesional = collection["model.EmailProfesional"],
                    TelefonoProfesional = collection["model.TelefonoProfesional"],
                    IdEspecialidad = int.Parse(collection["model.IdEspecialidad"]),
                };
                _profesionalManager.ModificarProfesional(id, profesional);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProfesionalController/Delete/5
        public ActionResult Delete(int id)
        {
            var profesional = _profesionalManager.GetProfesional(id);
            var especialidades = _especialidadRepository.GetEspecialidades();

            ProfesionalModel profesionalModel = new ProfesionalModel();
            profesionalModel.model = profesional;

            profesionalModel.ListaEspecialidadesItem = new List<SelectListItem>();
            foreach (var especialidad in especialidades)
            {
                profesionalModel.ListaEspecialidadesItem.Add(new SelectListItem { Value = especialidad.IdEspecialidad.ToString(), Text = especialidad.NombreEspecialidad });
            }
            return View(profesionalModel);

        }

        // POST: ProfesionalController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {   _profesionalManager.DeleteProfesional(id) ;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
