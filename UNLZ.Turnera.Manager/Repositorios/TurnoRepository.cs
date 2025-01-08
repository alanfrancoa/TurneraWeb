using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNLZ.Turnera.Manager.Entidades;
using Dapper;
using UNLZ.Turnera.Manager.ModelFactories;

namespace UNLZ.Turnera.Manager.Repositorios
{
    //Interfaz- metodos CRUD en DB- tabla Turno//
    public interface ITurnoRepository
    {
        //Obtener un turno por ID
        Turno GetTurno(int idTurno);

        //Obtener una lista de turnos
        IEnumerable<Turno> GetTurno();

        //Obtener una lista de turnos con datos completos 
        IEnumerable<TurnoCompleto> GetTurnosCompeto();

        //Crear turno- devuelve id del turno
        int CrearTurno(Turno turno);

        //Modificar turno- devuelve resultado de la edicion
        bool ModificarTurno(int ModificacionId, Turno turno);

        //Eliminacion(soft-delete) de turno- devuelve resultado de operacion
        bool DeleteTurno(int idTurno);

        // Obtener turnos por DNI 
        IEnumerable<Turno> GetTurnosPorPaciente(string dniPaciente);
    }

    public class TurnoRepository : ITurnoRepository
    {
        //Dependencia repository//
        private string StringConnection;

        public TurnoRepository(string stringConnection)
        {
            this.StringConnection = stringConnection;
        }

        //Metodos

        public Turno GetTurno(int idTurno)
        {
            //El uso de Using, permite abrir y cerrar automaticamente la conexion a la BBDD.
            using (IDbConnection conn = new SqlConnection(StringConnection))
            {
                // Traer todo con el id ingresado.
                string query = "SELECT *" +
                    " FROM TURNO " +
                    "WHERE idTurno = @IdTurno;";

                // Usar QuerySingleOrDefault para manejar el caso en que no se encuentre ningún registro.
                Turno result = conn.QuerySingleOrDefault<Turno>(query, new { IdTurno = idTurno });
                return result;
            }

        }
        public IEnumerable<Turno> GetTurno()
        {
            using (IDbConnection conn = new SqlConnection(StringConnection))
            {
                //Traer todos los turnos//
                string query = "SELECT * FROM TURNO";

                //almacenamos el retorno en un IEnumerable
                IEnumerable<Turno> result = conn.Query<Turno>(query);

                //retornamos- listado de todos los turnos//
                return result;
            }
        }

        public int CrearTurno(Turno turno)
        {
            //El uso de Using, permite abrir y cerrar automaticamente la conexion a la BBDD.
            using (IDbConnection conn = new SqlConnection(StringConnection))
            {
                // Query de inserción adaptada con los valores necesarios del turno//
                string query = @"
            INSERT INTO TURNO ( fechaCreacion, horaCreacion, idProfesional, dniPaciente, idDisponibilidad)
            VALUES ( @FechaCreacion, @HoraCreacion, @IdProfesional, @DniPaciente, @IdDisponibilidad);
            SELECT CAST(SCOPE_IDENTITY() AS INT)";

                // Ejecuta la query y asigna el ID generado al objeto turno//
                turno.IdTurno = conn.QuerySingle<int>(query, turno);

                // Retorna el ID turno nuevo//
                return turno.IdTurno;
            }
        }

        public bool ModificarTurno(int ModificacionId, Turno turno)
        {
            using (IDbConnection conn = new SqlConnection(StringConnection))
            {
                // Consulta de actualización en todos los campos del turno//
                string query = @"
            UPDATE TURNO 
            SET 
                fechaCreacion = @FechaCreacion, 
                horaCreacion  = @HoraCreacion, 
                idProfesional =  @IdProfesional,
                dniPaciente  =  @DniPaciente,
                idDisponibilidad = @IdDisponibilidad,
                estadoTurno = @EstadoTurno
            WHERE idTurno = @IdTurno";
                                                
                // Asignamos el ID al objeto para usarlo en la consulta
                turno.IdTurno = ModificacionId;

                // Ejecuta la consulta y verifica si se actualizó una fila
                return conn.Execute(query, turno) == 1;
            }
        }
        public bool DeleteTurno(int idTurno)
        {
            using (IDbConnection conn = new SqlConnection(StringConnection))
            {
                //"falsa-Eliminacion", cambia el estado a cancelado.
                string query = @"
                       UPDATE TURNO 
                       SET estadoTurno = 'Cancelado' 
                       WHERE idTurno = @IdTurno";

                //retorna True, si se realiza la query
                return conn.Execute(query, new { IdTurno= idTurno}) == 1;
            }
        }

        public IEnumerable<Turno> GetTurnosPorPaciente(string dniPaciente)
        {
            using (IDbConnection conn = new SqlConnection(StringConnection))
            {
                //Traer todos los turnos//
                string query = "SELECT * FROM Turno WHERE dniPaciente = @DniPaciente";

                //almacenamos el retorno en un IEnumerable
                IEnumerable<Turno> result = conn.Query<Turno>(query, new { DniPaciente = dniPaciente });

                //retornamos- listado de todos los turnos//
                return result;
            }
        }

        public IEnumerable<TurnoCompleto> GetTurnosCompeto()
        {
            using (IDbConnection conn = new SqlConnection(StringConnection))
            {
                //Traer todos los turnos//
                string query = @"
            SELECT 
                t.idTurno,
                t.fechaCreacion,
                t.horaCreacion,
                p.nombreProfesional,
                t.dniPaciente,
                d.horaInicio,
                d.horaFin,
                t.estadoTurno
            FROM 
                TURNO t
            JOIN 
                PROFESIONAL p ON t.idProfesional = p.idProfesional
            JOIN 
                DISPONIBILIDAD d ON t.idDisponibilidad = d.idDisponibilidad
            ORDER BY 
                t.fechaCreacion, t.horaCreacion;";

                //almacenamos el retorno en un IEnumerable
                IEnumerable<TurnoCompleto> result = conn.Query<TurnoCompleto>(query);

                //retornamos- listado de todos los turnos//
                return result;
            }
        }
    }

    

}
