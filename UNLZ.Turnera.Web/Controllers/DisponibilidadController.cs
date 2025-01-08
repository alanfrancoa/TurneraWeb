using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;
using UNLZ.Turnera.Manager.Entidades;
using UNLZ.Turnera.Manager.Managers;
using UNLZ.Turnera.Manager.Repositorios;
using UNLZ.Turnera.Web.Models;

namespace UNLZ.Turnera.Web.Controllers
{
    public class DisponibilidadController : Controller
    {
        //Creacion- dependencias manager//
        private IDisponibilidadManager _disponibilidadManager;
        private _ProfesionalRepository _profesionalRepository;

        //constructor//
        public DisponibilidadController(IDisponibilidadManager disponibilidadManager, _ProfesionalRepository profesionalRepo)
        {
            _disponibilidadManager = disponibilidadManager;
            _profesionalRepository = profesionalRepo;
        }
        // GET: DisponibilidadController
        public ActionResult Index()
        {
            var disponibilidades = _disponibilidadManager.GetDisponibilidades();
            return View(disponibilidades);
        }

        // GET: DisponibilidadController/Details/5
        public ActionResult Details(int id)
        {
            var disponibilidad = _disponibilidadManager.GetDisponibilidad(id);

            DisponibilidadModel disponibilidadModel = new DisponibilidadModel();
            disponibilidadModel.model = disponibilidad;
            disponibilidadModel.ListaProfesionalesItem = new List<SelectListItem>();
            var profesionales = _profesionalRepository.GetProfesionales();
            foreach (var profesional in profesionales)
            {
                disponibilidadModel.ListaProfesionalesItem.Add(new SelectListItem { Value = profesional.IdProfesional.ToString(), Text = profesional.NombreProfesional });
            }

            return View(disponibilidadModel);
        }


        // GET: DisponibilidadController/Create
        public ActionResult Create()
        {
            DisponibilidadModel disponibilidadModel = new DisponibilidadModel();
            disponibilidadModel.model = null;

            disponibilidadModel.ListaProfesionalesItem = new List<SelectListItem>();
            var profesionales = _profesionalRepository.GetProfesionalesActivos();
            foreach (var profesional in profesionales)
            {
                disponibilidadModel.ListaProfesionalesItem.Add(new SelectListItem { Value = profesional.IdProfesional.ToString(), Text = profesional.NombreProfesional });
            }


            return View(disponibilidadModel);
        }

        // POST: DisponibilidadController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DisponibilidadModel disponibilidadModel)
        {
            try
            {
                Disponibilidad disponibilidad = new Disponibilidad
                {
                    HoraInicio = disponibilidadModel.model.HoraInicio,
                    HoraFin = disponibilidadModel.model.HoraFin,
                    IdProfesional = disponibilidadModel.model.IdProfesional,
                    Dia = disponibilidadModel.model.Dia,
                    Estado = "Activo"

                };

                _disponibilidadManager.CrearDisponibilidad(disponibilidad);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DisponibilidadController/Edit/5
        public ActionResult Edit(int id)
        {

            //Obtenemos el paciente
            var disponibilidad = _disponibilidadManager.GetDisponibilidad(id);

            //Crea y asigna un modelo de paciente, y retorna la vista
            DisponibilidadModel disponibilidadModel = new DisponibilidadModel();
            disponibilidadModel.model = disponibilidad;
                return View(disponibilidadModel);

        }

        // POST: DisponibilidadController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int idDisponibilidad, DisponibilidadModel disponibilidadModel)
        {
            try
            {
                
                Disponibilidad disponibilidad = new Disponibilidad
                {
                    IdDisponibilidad = disponibilidadModel.model.IdDisponibilidad,
                    HoraInicio = disponibilidadModel.model.HoraInicio,
                    HoraFin = disponibilidadModel.model.HoraFin,
                    IdProfesional = disponibilidadModel.model.IdProfesional,
                    Dia = disponibilidadModel.model.Dia,
                    Estado = disponibilidadModel.model.Estado,
                };

                _disponibilidadManager.ModificarDisponibilidad(idDisponibilidad, disponibilidad);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
         

        // GET: DisponibilidadController/Delete/5
        public ActionResult Delete(int id)
        {
            var disponibilidad = _disponibilidadManager.GetDisponibilidad(id);
            var profesionales = _profesionalRepository.GetProfesionales();

            DisponibilidadModel disponibilidadModel = new DisponibilidadModel();
            disponibilidadModel.model = disponibilidad;

            disponibilidadModel.ListaProfesionalesItem = new List<SelectListItem>();
            foreach (var profesional in profesionales)
            {
                disponibilidadModel.ListaProfesionalesItem.Add(new SelectListItem { Value = profesional.IdProfesional.ToString(), Text = profesional.NombreProfesional });
            }

            return View(disponibilidadModel);
        }

        // POST: DisponibilidadController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int idDisponibilidad, IFormCollection collection)
        {
            try
            {

                // Llama al método para eliminar
                var result = _disponibilidadManager.DeleteDisponibilidad(idDisponibilidad);
                if (result)
                {
                    // Si la eliminación es exitosa, redirige al índice
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Si falla, retorna a la vista con un mensaje de error
                    ModelState.AddModelError("", "No se pudo eliminar esta disponibilidad.");
                    var disponibilidad = _disponibilidadManager.GetDisponibilidad(idDisponibilidad);
                    return View(new DisponibilidadModel { model = disponibilidad });
                }
            }
            catch
            {
                return View();
            }
        }
    }
}
