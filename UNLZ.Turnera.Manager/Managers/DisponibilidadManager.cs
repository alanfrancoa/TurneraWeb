using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNLZ.Turnera.Manager.Entidades;
using UNLZ.Turnera.Manager.Repositorios;

namespace UNLZ.Turnera.Manager.Managers
{
    public interface IDisponibilidadManager
    {
        //Metodo para obtener una disponibilidad por ID.
        Disponibilidad GetDisponibilidad(int IdDisponibilidad);

        //Metodo para obtener datos de la lista, sin alteracion.
        IEnumerable<Disponibilidad> GetDisponibilidades();

        //Creacion de disponibilidad, retorna id de disponibilidad, si esta es insertada exitosamente.
        int CrearDisponibilidad(Disponibilidad disponibilidad);

        bool ModificarDisponibilidad(int ModificacionId, Disponibilidad disponibilidad);

        //Eliminacion de disponibilidad, retorna un booleano, con el resultado de la edicion.
        bool DeleteDisponibilidad(int idDisponibilidad);
    }
    public class DisponibilidadManager : IDisponibilidadManager
    {
        private IDisponibilidadRepository _repository;
        public DisponibilidadManager(IDisponibilidadRepository repository)
        {
            this._repository = repository;
        }

        public int CrearDisponibilidad(Disponibilidad disponibilidad)
        {
            //crea disponibilidad en el repo, nos retorna un mensaje si la insercion es exitosa.
            var cont = _repository.CrearDisponibilidad(disponibilidad);
            return cont;
        }

        public bool ModificarDisponibilidad(int ModificacionId, Disponibilidad disponibilidad)
        {

            //Buscamos el turno
            var disponibilidadEnDB = _repository.GetDisponibilidad(ModificacionId);

            if(disponibilidadEnDB == null)
            {
                return false;
            }

            //Establecemos los campos en base a lo obtenido por parametro
            disponibilidadEnDB.IdDisponibilidad = disponibilidad.IdDisponibilidad;
            disponibilidadEnDB.HoraInicio = disponibilidad.HoraInicio;
            disponibilidadEnDB.HoraFin = disponibilidad.HoraFin;
            disponibilidadEnDB.IdProfesional = disponibilidad.IdProfesional;
            disponibilidadEnDB.Dia = disponibilidad.Dia;
            disponibilidadEnDB.Estado = disponibilidad.Estado;               

                //lo retornamos
                return _repository.ModificarDisponibilidad(ModificacionId, disponibilidadEnDB);
        }

        public bool DeleteDisponibilidad(int idDisponibilidad)
        {
            //retornamos el boolean de la funcion DeleteDisponibilidad del repo.
            return _repository.DeleteDisponibilidad(idDisponibilidad);
        }

        public Disponibilidad GetDisponibilidad(int idDisponibilidad)
        {
            //Nos trae la disponibilidad por id del repositorio
            var disponibilidad = _repository.GetDisponibilidad(idDisponibilidad);
            //retornamos la disponibilidad obtenida
            return disponibilidad;
        }

        public IEnumerable<Disponibilidad> GetDisponibilidades()
        {
            //Retornamos el listado del repositorio.
            return _repository.GetDisponibilidades();
        }

    }
}
