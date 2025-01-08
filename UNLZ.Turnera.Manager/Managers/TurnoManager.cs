using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNLZ.Turnera.Manager.Entidades;
using UNLZ.Turnera.Manager.ModelFactories;
using UNLZ.Turnera.Manager.Repositorios;
//MANAGER-TURNO//
namespace UNLZ.Turnera.Manager.Managers
{
    //Interfaz con metodos para abm turno//
    public interface ITurnoManager
    {
        //Metodo- Obtener un turno por ID
        Turno GetTurno(int IdTurno);

        //Metodo- Obtener datos de la lista
        IEnumerable<Turno> GetTurno();

        // Metodo - Obtener la lista de turno compelta
        IEnumerable<TurnoCompleto> GetTurnosCompleto();

        //Creacion de turno- devuelve id del turno
        int CrearTurno(Turno turno);

        //Modificacion del turno- devuelve un booleano, con el resultado de la edicion
        bool ModificarTurno(int ModificacionId, Turno turno);

        //Eliminacion(soft-delete) de turno- devuelve un booleano, con el resultado de lo nuevo
        bool DeleteTurno(int IdTurno);

    }
    public class TurnoManager : ITurnoManager
        
    {
        private ITurnoRepository _repository;

        //Constructor- inicia el repositorio de turno
        public TurnoManager(ITurnoRepository repository)
        {
            this._repository = repository;
        }

        //llamando logica de consultas CRUD//
        public int CrearTurno(Turno turno)
        {
            //Crear turno en repo- retornando el ID 
            var cont = _repository.CrearTurno(turno);
            return cont;
        }

        public bool DeleteTurno(int idTurno)
        {
            //Devolvemos boolean de "Delete"
            return _repository.DeleteTurno(idTurno);
        }

        public Turno GetTurno(int IdTurno)
        {
            //Trae turno por ID-repo
            var turno = _repository.GetTurno(IdTurno);

            //retornamos el turno obtenido
            return turno;


        }

        public IEnumerable<Turno> GetTurno()
        {
            //traemos el listado de turno del repositorio.
            return _repository.GetTurno();
        }

        public IEnumerable<TurnoCompleto> GetTurnosCompleto()
        {
            return _repository.GetTurnosCompeto();
        }

        public bool ModificarTurno(int ModificacionId, Turno turno)
        {

            //Buscamos el turno
            var turnoEnDB = _repository.GetTurno(ModificacionId);

            //Establecemos los campos en base a lo obtenido por parametro
            turnoEnDB.FechaCreacion = turno.FechaCreacion;
            turnoEnDB.HoraCreacion = turno.HoraCreacion;
            turnoEnDB.IdProfesional = turno.IdProfesional;
            turnoEnDB.DniPaciente = turno.DniPaciente.ToString();
            turnoEnDB.IdDisponibilidad = turno.IdDisponibilidad;
            turnoEnDB.EstadoTurno = turno.EstadoTurno.ToString();

            //la funcion del repo, nos va a retornar un boolean
            var cont = _repository.ModificarTurno(ModificacionId, turno);

            //lo retornamos
            return cont;
        }
    }

}
