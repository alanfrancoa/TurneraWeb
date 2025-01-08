using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNLZ.Turnera.Manager.Entidades;
using UNLZ.Turnera.Manager.Repositorios;

namespace UNLZ.Turnera.Manager.Managers
{
    public interface IPacienteManager
    {
        // Método para obtener un paciente por DNI.
        Paciente GetPaciente(string dniPaciente);

        // Método para obtener una lista de pacientes.
        IEnumerable<Paciente> GetPacientes();

        // Método para obtener una lista de pacientes activos.
        IEnumerable<Paciente> GetPacientesActivos();

        // Creación de paciente, retorna el DNI del paciente si se inserta exitosamente.
        string CrearPaciente(Paciente paciente);

        // Modificación de paciente, retorna un booleano con el resultado de la edición.
        bool ModificarPaciente(string ModificacionDni, Paciente paciente);

        // Eliminación de paciente (soft delete), retorna un booleano con el resultado.
        bool DeletePaciente(string dniPaciente);
    }

    public class PacienteManager : IPacienteManager
    {
        private IPacienteRepository _repository;

        // Constructor que inicializa el repositorio.
        public PacienteManager(IPacienteRepository repository)
        {
            this._repository = repository;
        }
        public string CrearPaciente(Paciente paciente)
        {
            // Crea el paciente en el repositorio y retorna el DNI si la inserción es exitosa.
            var dni = _repository.CrearPaciente(paciente);
            return dni;
        }

        public bool DeletePaciente(string dniPaciente)
        {
            // Retorna el booleano de la función DeletePaciente del repositorio.
            return _repository.DeletePaciente(dniPaciente);
        }

        public Paciente GetPaciente(string dniPaciente)
        {
            // Obtiene el paciente por DNI del repositorio.
            var paciente = _repository.GetPaciente(dniPaciente);
            return paciente;
        }

        public IEnumerable<Paciente> GetPacientes()
        {
            // Retorna el listado de pacientes activos del repositorio.
            return _repository.GetPacientes();
        }

        public IEnumerable<Paciente> GetPacientesActivos()
        {
            // Retorna el listado de pacientes activos del repositorio.
            return _repository.GetPacientesActivos();
        }

        public bool ModificarPaciente(string ModificacionDni, Paciente paciente)
        {
            // Validamos si existe un paciente con el DNI proporcionado
            var pacienteEnDB = _repository.GetPaciente(ModificacionDni);

            if (pacienteEnDB == null)
            {
                // Si el paciente no existe, devolvemos false
                return false;
            }

            // Actualizamos los campos del paciente encontrado
            pacienteEnDB.NombrePaciente = paciente.NombrePaciente;
            pacienteEnDB.EmailPaciente = paciente.EmailPaciente;
            pacienteEnDB.TelefonoPaciente = paciente.TelefonoPaciente;

            // Llamamos al repositorio para actualizar
            return _repository.ModificarPaciente(ModificacionDni, pacienteEnDB);
        }

    }
}
