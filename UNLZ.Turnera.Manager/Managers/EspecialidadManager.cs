using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNLZ.Turnera.Manager.Entidades;
using UNLZ.Turnera.Manager.Repositorios;

namespace UNLZ.Turnera.Manager.Managers
{
    public interface IEspecialidadManager 
    {
        //Metodo para obtener una especialidad por ID.
        Especialidad GetEspecialidad(int IdEspecialidad);

        //Metodo para obtener datos de la lista, sin alteracion.
        IEnumerable<Especialidad> GetEspecialidades();

        //Creacion de especialidad, retorna id de especialidad, si esta es insertada exitosamente.
        int CrearEspecialidad(Especialidad especialidad);

        //Modificacion de especialidad, retorna un booleano, con el resultado de la edicion.
        bool ModificarEspecialidad(int ModificacionId, Especialidad especialidad);

        //Eliminacion de especialidad, retorna un booleano, con el resultado de la edicion.
        bool DeleteEspecialidad(int Especialidad);
    }
    public class EspecialidadManager : IEspecialidadManager
    {   
        private IEspecialidadRepository _repository;

        //Constructor que inicializa el repositorio.
        public EspecialidadManager(IEspecialidadRepository repository)
        {
            _repository = repository;
        }


        public int CrearEspecialidad(Especialidad especialidad)
        {   
            //la creacion de especialidad en el repo, nos retorna el ID en caso de que la insercion sea exitosa.
            var cont = _repository.CrearEspecialidad(especialidad);
            return cont;
        }

        public bool DeleteEspecialidad(int Especialidad)
        {   
            //retornamos el boolean de la funcion DeleteEspecialidad del repo.
            return _repository.DeleteEspecialidad(Especialidad);
        }

        public Especialidad GetEspecialidad(int IdEspecialidad)
        {   
            //Nos trae la especialidad por id del repositorio
            var especialidad = _repository.GetEspecialidad(IdEspecialidad);
             //retornamos la especialidad obtenida
            return especialidad;
        }

        public IEnumerable<Especialidad> GetEspecialidades()
        {   
            //Retornamos el listado del repositorio.
            return _repository.GetEspecialidades();
        }

        public bool ModificarEspecialidad(int ModificacionId, Especialidad especialidad)
        {   
            //primero buscamos la especialidad
            var especialidadEnDB = _repository.GetEspecialidad(ModificacionId);

            //Establecemos el nombre en base al obtenido por parametro
            especialidadEnDB.NombreEspecialidad = especialidad.NombreEspecialidad;

            //la funcion del repo, nos va a retornar un boolean
            var cont = _repository.ModificarEspecialidad(ModificacionId, especialidad);

            //lo retornamos
            return cont;
        }
    }
}
