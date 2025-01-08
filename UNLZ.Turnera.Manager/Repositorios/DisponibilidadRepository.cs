using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UNLZ.Turnera.Manager.Entidades;

namespace UNLZ.Turnera.Manager.Repositorios
{
    public interface IDisponibilidadRepository
    {
        //Metodo para obtener una Disponibilidad por ID.
        Disponibilidad GetDisponibilidad(int IdDisponibilidad);

        //Metodo para obtener datos de la lista, sin alteracion.
        IEnumerable<Disponibilidad> GetDisponibilidades();

        //Creacion de Disponibilidad, retorna id de Disponibilidad, si esta es insertada exitosamente.
        int CrearDisponibilidad(Disponibilidad disponibilidad);

        //Modificar disponibilidad. devuelve resultado de la edicionDeleteDisponibilidad
        bool ModificarDisponibilidad(int ModificacionId, Disponibilidad disponibilidad);

        //Eliminacion de Disponibilidad, retorna un booleano, con el resultado de la edicion.
        bool DeleteDisponibilidad(int idDisponibilidad);

    }

    public class DisponibilidadRepository : IDisponibilidadRepository
    {
        //Dependencia del repositorio
        private String StringConnection;

        public DisponibilidadRepository(string stringConnection)
        {
            StringConnection = stringConnection;
        }

        public Disponibilidad GetDisponibilidad(int idDisponibilidad)
        {
       
            using (IDbConnection conn = new SqlConnection(StringConnection))
            {
                // Traer todo con el id ingresado.
                string query = @"SELECT * FROM disponibilidad WHERE idDisponibilidad = @IdDisponibilidad";

                // Usar QuerySingleOrDefault para manejar el caso en que no se encuentre ningún registro.
                Disponibilidad result = conn.QuerySingleOrDefault<Disponibilidad>(query, new { IdDisponibilidad = idDisponibilidad });
                return result;
            }
        }

        public int CrearDisponibilidad(Disponibilidad disponibilidad)
        {
            using (IDbConnection conn = new SqlConnection(StringConnection))
            {
                string query = @"INSERT INTO DISPONIBILIDAD(horaInicio, horaFin, idProfesional, dia, estado)
                                VALUES(@horaInicio, @horaFin, @idProfesional, @dia, 'activo');
                                SELECT CAST(SCOPE_IDENTITY() AS INT); ";

                disponibilidad.IdDisponibilidad = conn.QuerySingle<int>(query, disponibilidad);

                return disponibilidad.IdDisponibilidad;
            }
        }

        public bool ModificarDisponibilidad(int ModificacionId, Disponibilidad disponibilidad)
        {
            using (IDbConnection conn = new SqlConnection(StringConnection))
            {
                // Consulta de actualización en todos los campos del disponibilidad//
                string query = @" UPDATE disponibilidad 
                                SET horaInicio = @horaInicio,
                                    horaFin = @horaFin,
                                    dia  =  @dia,
                                    estado = @estadoDisponibilidad
                                WHERE idDisponibilidad = @IdDisponibilidad";

                // Ejecuta la consulta y verifica si se actualizó una fila
                // Asignamos el ID al objeto para usarlo en la query
                disponibilidad.IdDisponibilidad = ModificacionId;

                // Ejecuta la consulta y verifica si se actualizó una fila
                return conn.Execute(query, disponibilidad) == 1;
            }
        }

        public bool DeleteDisponibilidad(int IdDisponibilidad)
        {
            using (IDbConnection conn = new SqlConnection(StringConnection))
            {
          
                string query = @"UPDATE 
                                DISPONIBILIDAD 
                                SET estado = 'Inactivo' 
                                WHERE idDisponibilidad = " + IdDisponibilidad;
               

               
                return conn.Execute(query) == 1;
            }

        }

        public IEnumerable<Disponibilidad> GetDisponibilidades()
        {
            using (IDbConnection conn = new SqlConnection(StringConnection))
            {
                //Trae los "disponibles"
                string query = "SELECT * FROM DISPONIBILIDAD";

                //almacenamos el retorno en un IEnumerable
                IEnumerable<Disponibilidad> result = conn.Query<Disponibilidad>(query);


                return result;
            }
        }

    }
}

