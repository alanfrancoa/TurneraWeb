using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNLZ.Turnera.Manager.Entidades;
using UNLZ.Turnera.Manager.Repositorios;

namespace UNLZ.Turnera.Manager.Managers
{
    public interface IProfesionalManager
    {
        //Metodo para obtener una profesional por ID.
        Profesional GetProfesional(int IdProfesional);

        //Metodo para obtener datos de la lista, sin alteracion.
        IEnumerable<Profesional> GetProfesionales();

        //Creacion de profesional, retorna id del profesional, si esta es insertado exitosamente.
        int CrearProfesional(Profesional profesional);

        //Modificacion de profesional, retorna un booleano, con el resultado de la edicion.
        bool ModificarProfesional(int ModificacionId, Profesional profesional);

        //Eliminacion de profesional, retorna un booleano, con el resultado de la edicion. SOFTDELETE
        bool DeleteProfesional(int Profesional);

    }
    public class ProfesionalManager : IProfesionalManager
    {   
        private _ProfesionalRepository _repository;

        //Constructor que inicializa el repositorio
        public ProfesionalManager(_ProfesionalRepository repository) 
        {
            this._repository = repository;       
        }


        public int CrearProfesional(Profesional profesional)
        {
            //la creacion de profesional en el repo, nos retorna el ID en caso de que la insercion sea exitosa
            var cont = _repository.CrearProfesional(profesional);
            return cont;
        }

        public bool DeleteProfesional(int Profesional)
        {
            //retornamos el boolean de la funcion DeleteProfesional del repo.
            return _repository.DeleteProfesional(Profesional);
        }

        public Profesional GetProfesional(int IdProfesional)
        {
            //Nos trae el profesional por id del repositorio
            var profesional = _repository.GetProfesional(IdProfesional);

            //retornamos el profesional obtenido
            return profesional;
        }

        public IEnumerable<Profesional> GetProfesionales()
        {
            //traemos el listado del repositorio.
            return _repository.GetProfesionales();
        }

        public bool ModificarProfesional(int ModificacionId, Profesional profesional)
        {
            //primero buscamos el profesional
            var profesionalEnDB = _repository.GetProfesional(ModificacionId);

            //Establecemos los campos en base a lo obtenido por parametro
            profesionalEnDB.NombreProfesional = profesional.NombreProfesional;
            profesionalEnDB.EmailProfesional = profesional.EmailProfesional.ToString();
            profesionalEnDB.TelefonoProfesional = profesional.TelefonoProfesional.ToString();

            //la funcion del repo, nos va a retornar un boolean
            var cont = _repository.ModificarProfesional(ModificacionId, profesional);

            //lo retornamos
            return cont;
        }
    }
}
