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
    public class TurnoController : Controller
    {
        //Creacion -dependencias//

        private ITurnoManager _turnoManager;
        private IEspecialidadRepository _especialidadRepository;
        private _ProfesionalRepository _profesionalRepository;
        private IPacienteRepository _pacienteRepository;
        private IDisponibilidadRepository _disponibilidadRepository;



        //Creacion- constructor//
        public TurnoController(ITurnoManager turnoManager, IEspecialidadRepository especialidadRepository, _ProfesionalRepository profesionalRepository, IPacienteRepository pacienteRepository, IDisponibilidadRepository disponibilidadRepository)
        {

            _turnoManager = turnoManager;
            _especialidadRepository = especialidadRepository;
            _profesionalRepository = profesionalRepository;
            _pacienteRepository = pacienteRepository;
            _disponibilidadRepository = disponibilidadRepository;
        }

        // GET: TurnoController
        public ActionResult Index()
        {
            var turno = _turnoManager.GetTurnosCompleto();
            return View(turno);
        }

        // GET: TurnoController/Details/5
        public ActionResult Details(int id)
        {
            var turno = _turnoManager.GetTurno(id);

            var profesional = _profesionalRepository.GetProfesional(turno.IdProfesional);
            var disponibilidad = _disponibilidadRepository.GetDisponibilidad(turno.IdDisponibilidad);
            var paciente = _pacienteRepository.GetPaciente(turno.DniPaciente);
            // Obtener el nombre de la especialidad usando el repositorio de especialidades
            var especialidad = _especialidadRepository.GetEspecialidadNombre(profesional.IdEspecialidad);

            ViewData["FechaCreacion"] = turno.FechaCreacion.ToShortDateString(); // Solo la fecha
            ViewData["NombreProfesionalEspecialidad"] = $"{profesional.NombreProfesional} ({especialidad})"; // Nombre y especialidad
            ViewData["DisponibilidadHorario"] = $"{disponibilidad.HoraInicio} - {disponibilidad.HoraFin}"; // Horarios de disponibilidad
            ViewData["NombrePacienteDni"] = $"{paciente.NombrePaciente} (DNI: {paciente.DniPaciente})"; // Nombre y DNI del paciente
            ViewData["ProfesionalEspecialidad"] = $"{profesional.NombreProfesional} ({especialidad})"; // Nombre y especialidad del profesional


            //crea modelo//
            TurnoModel turnoModel = new TurnoModel();
            turnoModel.model= turno;
                     
            return View(turnoModel);
        }

        // GET: TurnoController/Create
        public ActionResult Create()
        {
            TurnoModel turnoModel = new TurnoModel
            {
                model = new Turno(),
                ListaProfesionales = new List<SelectListItem>(), // Inicializa la lista vacía
                ListaPacientes = new List<SelectListItem>(),
                ListaDisponibilidades = new List<SelectListItem>(),

            };

            // Llena la lista de profesionales
            var profesionales = _profesionalRepository.GetProfesionalesActivos();
            var pacientes = _pacienteRepository.GetPacientesActivos();
            var disponibilidades= _disponibilidadRepository.GetDisponibilidades();



            foreach (var profesional in profesionales)
            {
                turnoModel.ListaProfesionales.Add(new SelectListItem
                {
                    Value = profesional.IdProfesional.ToString(),
                    Text = " Dr: " + profesional.NombreProfesional + " | Especialidad: " +
                           _especialidadRepository.GetEspecialidadNombre(profesional.IdEspecialidad)
                });
            }

            foreach(var paciente in pacientes)
            {
                turnoModel.ListaPacientes.Add(new SelectListItem
                {
                    Value= paciente.DniPaciente.ToString(),
                    Text = " DNI: " + paciente.DniPaciente + " | Nombre: " +
                        _pacienteRepository.GetPacienteNombre(paciente.DniPaciente)
                });

            }


            foreach (var disponibilidad in disponibilidades)
            {
                turnoModel.ListaDisponibilidades.Add(new SelectListItem
                {
                    Value = disponibilidad.IdDisponibilidad.ToString(),
                    Text = " Hora inicio: " + disponibilidad.HoraInicio.ToString() + " | Hora fin: " +
                    disponibilidad.HoraFin.ToString()
                        
                });

            }


            return View(turnoModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TurnoModel turnoModel)
        {
            try
            {
                Turno turno = new Turno
                {
                    FechaCreacion = turnoModel.model.FechaCreacion.Date, // Usa la fecha ingresada por el usuario
                    HoraCreacion = turnoModel.model.HoraCreacion,   // Hora ingresada
                    IdProfesional = turnoModel.model.IdProfesional, // Id del profesional
                    DniPaciente = turnoModel.model.DniPaciente,
                    IdDisponibilidad = turnoModel.model.IdDisponibilidad,
                    EstadoTurno= turnoModel.model.EstadoTurno,
                   
                };

                _turnoManager.CrearTurno(turno);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                // Si ocurre un error, recarga la lista de profesionales para la vista
                turnoModel.ListaProfesionales = _profesionalRepository.GetProfesionalesActivos()
                    .Select(profesional => new SelectListItem
                    {
                        Value = profesional.IdProfesional.ToString(),
                        Text = " Dr: " + profesional.NombreProfesional + " | Especialidad: " +
                               _especialidadRepository.GetEspecialidadNombre(profesional.IdEspecialidad)
                    }).ToList();

                return View();
            }
        }


        // GET: TurnoController/Edit/5
        public ActionResult Edit(int id)
        {
            var turno = _turnoManager.GetTurno(id);

            // Obtener datos para los Dropdown
            var pacientes = _pacienteRepository.GetPacientesActivos(); // Reemplaza con tu lógica
            var profesionales = _profesionalRepository.GetProfesionalesActivos(); // Reemplaza con tu lógica
            var disponibilidades = _disponibilidadRepository.GetDisponibilidades(); // Reemplaza con tu lógica
            
            
            // Construye el modelo para la vista
             TurnoModel turnoModel = new TurnoModel
           {
                model = turno, 
                ListaPacientes = pacientes.Select(p => new SelectListItem
                {
                    Value = p.DniPaciente.ToString(),
                    Text = $"{p.NombrePaciente} (DNI: {p.DniPaciente})",
                    Selected = p.DniPaciente.ToString() == turno.DniPaciente.ToString() // Marca el paciente seleccionado
                }).ToList(),
                ListaProfesionales = profesionales.Select(p => new SelectListItem
                {
                    Value = p.IdProfesional.ToString(),
                    Text = $"{p.NombreProfesional} ({_especialidadRepository.GetEspecialidadNombre(p.IdEspecialidad)})",
                    Selected = p.IdProfesional == turno.IdProfesional // Marca el profesional seleccionado
                }).ToList(),
                ListaDisponibilidades = disponibilidades.Select(d => new SelectListItem
                {
                    Value = d.IdDisponibilidad.ToString(),
                    Text = $"{d.HoraInicio} - {d.HoraFin}",
                    Selected = d.IdDisponibilidad == turno.IdDisponibilidad // Marca la disponibilidad seleccionada
                }).ToList(),
                                
             };

            return View(turnoModel); // Pasa el modelo a la vista
        }

        // POST: TurnoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TurnoModel turnoModel)
        {
            try
            {
                Turno turnoActualizado = new Turno
                {
                    FechaCreacion = turnoModel.model.FechaCreacion, // Fecha ingresada
                    HoraCreacion = turnoModel.model.HoraCreacion,   // Hora ingresada
                    IdProfesional = turnoModel.model.IdProfesional, // Profesional seleccionado
                    DniPaciente = turnoModel.model.DniPaciente,     // Paciente seleccionado
                    IdDisponibilidad = turnoModel.model.IdDisponibilidad, // Disponibilidad seleccionada
                    EstadoTurno = turnoModel.model.EstadoTurno // Estado del turno
                };

                // Llama al manager para actualizar el turno
                _turnoManager.ModificarTurno(id, turnoActualizado);

                return RedirectToAction(nameof(Index)); // Redirige a la lista de turnos
            }
            catch
            {
                // Si ocurre un error, recarga las listas necesarias para los Dropdowns
                turnoModel.ListaProfesionales = _profesionalRepository.GetProfesionalesActivos()
                    .Select(p => new SelectListItem
                    {
                        Value = p.IdProfesional.ToString(),
                        Text = $"{p.NombreProfesional} ({_especialidadRepository.GetEspecialidadNombre(p.IdEspecialidad)})"
                    }).ToList();

                turnoModel.ListaPacientes = _pacienteRepository.GetPacientesActivos()
                    .Select(p => new SelectListItem
                    {
                        Value = p.DniPaciente.ToString(),
                        Text = $"{p.NombrePaciente} (DNI: {p.DniPaciente})"
                    }).ToList();

                turnoModel.ListaDisponibilidades = _disponibilidadRepository.GetDisponibilidades()
                    .Select(d => new SelectListItem
                    {
                        Value = d.IdDisponibilidad.ToString(),
                        Text = $"{d.HoraInicio} - {d.HoraFin}"
                    }).ToList();

                return View(turnoModel); // Retorna la vista con los datos corregidos
            }
        }

        // GET: TurnoController/Delete/5
        public ActionResult Delete(int id)
        {
            //busca turno por id, crea el modelo y retorna a la vista//
            var turno = _turnoManager.GetTurno(id);

            // Obtiene el paciente asociado con su nombre
            var paciente = _pacienteRepository.GetPaciente(turno.DniPaciente);
            var profesional = _profesionalRepository.GetProfesional(turno.IdProfesional);
            var disponibilidad = _disponibilidadRepository.GetDisponibilidad(turno.IdDisponibilidad);
            var especialidad = _especialidadRepository.GetEspecialidadNombre(profesional.IdEspecialidad);

            ViewData["NombrePacienteDni"] = $"{paciente.NombrePaciente} (DNI: {paciente.DniPaciente})"; // Nombre y DNI del paciente
            ViewData["FechaCreacion"] = turno.FechaCreacion.ToShortDateString(); // Solo la fecha
            ViewData["HoraCreacion"] = turno.HoraCreacion.ToString(); // Solo la fecha
            ViewData["NombreProfesionalEspecialidad"] = $"{profesional.NombreProfesional} ({especialidad})"; // Nombre y especialidad
            ViewData["DisponibilidadHorario"] = $"{disponibilidad.HoraInicio} - {disponibilidad.HoraFin}"; // Horarios de disponibilidad
           ViewData["ProfesionalEspecialidad"] = $"{profesional.NombreProfesional} ({especialidad})"; // Nombre y especialidad del profesional



            TurnoModel turnoModel = new TurnoModel();
            turnoModel.model = turno;
            return View(turnoModel);
        }

        // POST: TurnoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int idTurno, TurnoModel turnoModel)
        {
            try
            {
               var result = _turnoManager.DeleteTurno(idTurno);
                
                if (result)
                {//si elimina correctamente, redirige al index//
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "no se elimino turno.");
                    var turno = _turnoManager.GetTurno(idTurno);
                    return View( new TurnoModel { model = turno});
                }
            }
            catch
            {
                
                return View(); 
            }
        }
    }
}
