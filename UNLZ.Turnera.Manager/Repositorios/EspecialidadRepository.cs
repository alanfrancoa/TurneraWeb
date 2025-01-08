using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNLZ.Turnera.Manager.Entidades;

namespace UNLZ.Turnera.Manager.Repositorios
{   //Interfaz para metodos CRUD en BBDD de tabla Especialidad.
    public interface IEspecialidadRepository 
    {   
        //Metodo para obtener una especialidad por ID.
        Especialidad GetEspecialidad(int IdEspecialidad);

        //Metodo para obtener nombe de  una especialidad por ID.
        string GetEspecialidadNombre(int IdEspecialidad);

        //Metodo para obtener datos de la lista, sin alteracion.
        IEnumerable<Especialidad> GetEspecialidades();

        //Metodo para obtener datos de la lista que su estado sea activo.
        IEnumerable<Especialidad> GetEspecialidadesActivas();

        //Creacion de especialidad, retorna id de especialidad, si esta es insertada exitosamente.
        int CrearEspecialidad(Especialidad especialidad);

        //Modificacion de especialidad, retorna un booleano, con el resultado de la edicion.
        bool ModificarEspecialidad(int ModificacionId, Especialidad especialidad);

        //Eliminacion de especialidad, retorna un booleano, con el resultado de la edicion.
        bool DeleteEspecialidad(int Especialidad);

    }
    public class EspecialidadRepository : IEspecialidadRepository
    {   
        //Dependencia del repositorio
        private String StringConnection;

        //Constructor
        public EspecialidadRepository(string stringConnection)
        {
            StringConnection = stringConnection;
        }

        //
        public Especialidad GetEspecialidad(int IdEspecialidad)
        {   
            //El uso de Using, permite abrir y cerrar automaticamente la conexion a la BBDD.
            using(IDbConnection conn = new SqlConnection(StringConnection)) 
            {   
                //Traer todo con el id ingresado.
                string query = "SELECT * FROM ESPECIALIDAD WHERE idEspecialidad = " + IdEspecialidad;
                
                //Retorno de nuestra query
                Especialidad result = conn.QuerySingle<Especialidad>(query);
                 return result;
            }

        }

        //
        public int CrearEspecialidad(Especialidad especialidad)
        {
            //El uso de Using, permite abrir y cerrar automaticamente la conexion a la BBDD.
            using (IDbConnection conn = new SqlConnection(StringConnection))
            {
                //Traer todo con el id ingresado.
                string query = @"INSERT INTO ESPECIALIDAD (nombreEspecialidad)
                                VALUES ( @nombreEspecialidad);
                                SELECT CAST(SCOPE_IDENTITY() AS INT) ";

                //Se ejecuta la query, retorna el int
                especialidad.IdEspecialidad = conn.QuerySingle<int>(query, especialidad);

                //Retorno de nuestra query con el ID
                return especialidad.IdEspecialidad;
            }
        }

        public bool DeleteEspecialidad(int IDEspecialidad)
        {   
            using (IDbConnection conn = new SqlConnection(StringConnection)) 
            {   
                //"Eliminacion", se modifica el estado a inactivo.
                string query = @"UPDATE 
                    ESPECIALIDAD 
                SET 
                    estado = 'Inactivo' 
                WHERE idEspecialidad = " + IDEspecialidad ;

                //retorna True, si se realiza la query
                return conn.Execute(query) == 1;
            }

        }


        public IEnumerable<Especialidad> GetEspecialidades()
        {
            using (IDbConnection conn = new SqlConnection(StringConnection)) 
            {   
                //Traemos todas las especialidades
                string query = "SELECT * FROM ESPECIALIDAD";

                //almacenamos el retorno en un IEnumerable
                IEnumerable<Especialidad> result = conn.Query<Especialidad>(query); 
                
                //retornamos el listado
                return result;
            }
        }

        public bool ModificarEspecialidad(int ModificacionId, Especialidad especialidad)
        {
            using (IDbConnection conn = new SqlConnection(StringConnection))
            {
                //se modifica la especialidad.
                string query = @"UPDATE 
                             ESPECIALIDAD 
                         SET 
                             NombreEspecialidad = @NombreEspecialidad 
                             WHERE idEspecialidad = " + ModificacionId;

                //retorna True, si se realiza la query
                return conn.Execute(query, especialidad) == 1;
            }
        }

        public IEnumerable<Especialidad> GetEspecialidadesActivas()
        {
            using (IDbConnection conn = new SqlConnection(StringConnection))
            {
                //Traemos todas las especialidades
                string query = "SELECT * FROM ESPECIALIDAD WHERE estado = 'Activo'";

                //almacenamos el retorno en un IEnumerable
                IEnumerable<Especialidad> result = conn.Query<Especialidad>(query);

                //retornamos el listado
                return result;
            }
        }

        public string GetEspecialidadNombre(int IdEspecialidad)
        {
            using (IDbConnection conn = new SqlConnection(StringConnection))
            {
                // Consulta para obtener solo el nombre de la especialidad
                string query = "SELECT nombreEspecialidad FROM ESPECIALIDAD WHERE idEspecialidad = @IdEspecialidad";

                // Ejecutar la consulta y obtener el resultado
                string result = conn.QuerySingle<string>(query, new { IdEspecialidad });

                // Retornar el nombre de la especialidad
                return result;
            }
        }

    }
}
