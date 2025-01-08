using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNLZ.Turnera.Manager.Entidades;
using Dapper;

namespace UNLZ.Turnera.Manager.Repositorios
{   //Interfaz para metodos CRUD en BBDD de tabla Profesional
    public interface _ProfesionalRepository
    {
        //Metodo para obtener un profesional por ID.
        Profesional GetProfesional(int idProfesional);
        //Metodo para obtener datos de la lista, sin alteracion.
        IEnumerable<Profesional> GetProfesionales();


        //Metodo para obtener unicamente profesionales activos.
        IEnumerable<Profesional> GetProfesionalesActivos();

        //Metodo para obtener profesionales por especialidad.
        IEnumerable<Profesional> GetProfesionalesPorEspecialidadActivos(int idEspecialidad);

        //Creacion de profesional, retorna id de profesional, si esta es insertado exitosamente.
        int CrearProfesional(Profesional profesional);

        //Modificacion de profesional, retorna un booleano, con el resultado de la edicion.
        bool ModificarProfesional(int ModificacionId, Profesional profesional);

        //Eliminacion de ´profesional, retorna un booleano, con el resultado de la edicion(SOFT DELETE).
        bool DeleteProfesional(int Profesional);
    
    }

    public class ProfesionalRepository : _ProfesionalRepository
    {
        //Dependencia del repositorio
        private String StringConnection;

        public ProfesionalRepository(string stringConnection)
        {
            this.StringConnection = stringConnection;
        }

        //
        public Profesional GetProfesional(int IdProfesional)
        {
            //El uso de Using, permite abrir y cerrar automaticamente la conexion a la BBDD.
            using (IDbConnection conn = new SqlConnection(StringConnection))
            {
                //Traer todo con el id ingresado.
                string query = "SELECT * FROM PROFESIONAL WHERE idProfesional = " + IdProfesional;

                //Retorno de nuestra query
                Profesional result = conn.QuerySingle<Profesional>(query);
                return result;
            }
        }
        public IEnumerable<Profesional> GetProfesionales()
        {
            using (IDbConnection conn = new SqlConnection(StringConnection))
            {
                //Traemos todos los profesionales
                string query = "SELECT * FROM PROFESIONAL";

                //almacenamos el retorno en un IEnumerable
                IEnumerable<Profesional> result = conn.Query<Profesional>(query);

                //retornamos el listado
                return result;
            }
        }
        public int CrearProfesional(Profesional profesional)
        {
            //El uso de Using, permite abrir y cerrar automaticamente la conexion a la BBDD.
            using (IDbConnection conn = new SqlConnection(StringConnection))
            {
                // Query de inserción adaptada con los valores necesarios
                string query = @"
            INSERT INTO PROFESIONAL (nombreProfesional, emailProfesional, telefonoProfesional, idEspecialidad)
            VALUES (@NombreProfesional, @EmailProfesional, @TelefonoProfesional, @IdEspecialidad);
            SELECT CAST(SCOPE_IDENTITY() AS INT)";

                // Ejecuta la query y asigna el ID generado al objeto profesional
                profesional.IdProfesional = conn.QuerySingle<int>(query, profesional);

                // Retorna el ID del profesional insertado
                return profesional.IdProfesional;
            }
        }

        public bool ModificarProfesional(int ModificacionId, Profesional profesional)
        {
            using (IDbConnection conn = new SqlConnection(StringConnection))
            {
                // Consulta de actualización con todos los campos
                string query = @"
            UPDATE PROFESIONAL 
            SET 
                nombreProfesional = @NombreProfesional, 
                emailProfesional = @EmailProfesional, 
                telefonoProfesional = @TelefonoProfesional, 
                idEspecialidad = @IdEspecialidad 
            WHERE idProfesional = @IdProfesional";

                // Asignamos el ID al objeto para usarlo en la query
                profesional.IdProfesional = ModificacionId;

                // Ejecuta la consulta y verifica si se actualizó una fila
                return conn.Execute(query, profesional) == 1;
            }
        }


        public bool DeleteProfesional(int IDProfesional)
        {
            using (IDbConnection conn = new SqlConnection(StringConnection))
            {
                //"Eliminacion", se modifica el estado a inactivo.
                string query = @"UPDATE 
                    PROFESIONAL 
                SET 
                    estado = 'Inactivo' 
                WHERE idProfesional = " + IDProfesional;

                //retorna True, si se realiza la query
                return conn.Execute(query) == 1;
            }
        }

        public IEnumerable<Profesional> GetProfesionalesActivos()
        {
            using (IDbConnection conn = new SqlConnection(StringConnection))
            {
                //Traemos todos los profesionales
                string query = "SELECT * FROM PROFESIONAL WHERE estado = 'Activo'";

                //almacenamos el retorno en un IEnumerable
                IEnumerable<Profesional> result = conn.Query<Profesional>(query);

                //retornamos el listado
                return result;
            }
        }

        public IEnumerable<Profesional> GetProfesionalesPorEspecialidadActivos(int idEspecialidad)
        {
            using (IDbConnection conn = new SqlConnection(StringConnection))
            {
                // Consulta que combina filtro por especialidad y estado activo
                string query = @"
                    SELECT * 
                    FROM PROFESIONAL 
                    WHERE idEspecialidad = @idEspecialidad 
                    AND estado = 'Activo'";

                return conn.Query<Profesional>(query, new { idEspecialidad });
            }
        }

        public IEnumerable<dynamic> GetProfesionalesConEspecialidad()
        {
            using (IDbConnection conn = new SqlConnection(StringConnection))
            {
                // Consulta para traer el nombre del profesional y el nombre de la especialidad
                string query = @"
            SELECT 
                p.nombreProfesional AS NombreProfesional,
                e.nombreEspecialidad AS NombreEspecialidad
            FROM 
                PROFESIONAL p
            INNER JOIN 
                ESPECIALIDAD e 
            ON 
                p.idEspecialidad = e.idEspecialidad
            WHERE 
                p.estado = 'Activo' AND e.estado = 'Activo'";

                // Ejecuta la consulta y devuelve los resultados
                return conn.Query(query);
            }
        }

    }

}
