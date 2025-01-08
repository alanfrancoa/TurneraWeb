using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UNLZ.Turnera.Manager.Entidades;
using UNLZ.Turnera.Manager.Managers;
using UNLZ.Turnera.Web.Models;

namespace UNLZ.Turnera.Web.Controllers
{
    public class EspecialidadController : Controller
    {
        //Creacion- dependencias manager//
        private IEspecialidadManager _especialidadManager;
       
        //constructor//
        public EspecialidadController(IEspecialidadManager especialidadManager)
        {
            _especialidadManager = especialidadManager;
        }

        // GET: EspecialidadController
        public ActionResult Index()
        {
            var especialidades= _especialidadManager.GetEspecialidades();
            return View(especialidades);
        }

        // GET: EspecialidadController/Details/5
        public ActionResult Details(int id)
        {
            var especialidad = _especialidadManager.GetEspecialidad(id);

            EspecialidadModel especialidadModel = new EspecialidadModel();
            especialidadModel.model = especialidad;
            return View(especialidadModel);
        }

        // GET: EspecialidadController/Create
        public ActionResult Create()
        {
            EspecialidadModel especialidadModel = new EspecialidadModel();
            especialidadModel.model = null;
            return View(especialidadModel);
        }

        // POST: EspecialidadController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Especialidad especialidad = new Especialidad {
                    NombreEspecialidad = collection["model.NombreEspecialidad"]
                };

                _especialidadManager.CrearEspecialidad(especialidad);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EspecialidadController/Edit/5
        public ActionResult Edit(int id)
        {
            var especialidad = _especialidadManager.GetEspecialidad(id);

            EspecialidadModel especialidadModel = new EspecialidadModel();
            especialidadModel.model = especialidad;

            return View(especialidadModel);
        }

        // POST: EspecialidadController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                Especialidad especialidad = new Especialidad { NombreEspecialidad = collection["model.NombreEspecialidad"] };
                _especialidadManager.ModificarEspecialidad(id, especialidad);
                
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EspecialidadController/Delete/5
        public ActionResult Delete(int id)
        {
            var especialidad = _especialidadManager.GetEspecialidad(id);
            EspecialidadModel especialidadModel=new EspecialidadModel();
            especialidadModel.model=especialidad;
            return View(especialidadModel);
        }

        // POST: EspecialidadController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                _especialidadManager.DeleteEspecialidad(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
