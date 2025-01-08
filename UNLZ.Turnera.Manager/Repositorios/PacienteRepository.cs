using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNLZ.Turnera.Manager.Entidades;
using Dapper;
using System.Data.SqlClient;
using System.Data;

namespace UNLZ.Turnera.Manager.Repositorios
{
        public interface IPacienteRepository
        {   
            //metodo para obtener un paciente por su DNI
            Paciente GetPaciente(string dniPaciente);
            //Metodo para obtener todos los pacientes
            IEnumerable<Paciente> GetPacientes();
            
            //Metodo para obtener todos los pacientes activos
            IEnumerable<Paciente> GetPacientesActivos();
            
            //Metodo para crear un paciente
           string CrearPaciente(Paciente paciente);
            
            //Metodo para modificar paciente
            bool ModificarPaciente(string ModificacionDni, Paciente paciente);
            //metodo para eliminar paciente por soft, delete
            bool DeletePaciente(string dniPaciente);

            //metodo para obtener nombre de paciente//
            string GetPacienteNombre(string dniPaciente);
        }

        //Definicion de la clse que implementa la interfaz
        public class PacienteRepository : IPacienteRepository
        {
            //Dependencia del repositorio
            private String StringConnection;

            public PacienteRepository(string stringConnection)
            {
                this.StringConnection = stringConnection;
            }

        
        public string CrearPaciente(Paciente paciente)
        {
            using (IDbConnection conn = new SqlConnection(StringConnection))
            {
                // Consulta de inserción de la tabla 'Paciente'
                string query = @"
            INSERT INTO Paciente (dniPaciente, nombrePaciente, emailPaciente, telefonoPaciente)
            VALUES (@DniPaciente, @NombrePaciente, @EmailPaciente, @TelefonoPaciente);
            SELECT CAST(SCOPE_IDENTITY() AS VARCHAR(8));";  // Devuelve el 'dniPaciente' insertado.

                // Ejecutar la query e insertar el paciente
                paciente.DniPaciente = conn.QuerySingle<string>(query, paciente);

                // Retorna el 'dniPaciente' del paciente insertado
                return paciente.DniPaciente;
            }
        }

        public bool DeletePaciente(string dniPaciente)
        {
            using (IDbConnection conn = new SqlConnection(StringConnection))
            {
                //Modificacion a Inactivo 
                string query = @"
                        UPDATE Paciente 
                        SET estado = 'Inactivo'
                        WHERE dniPaciente = @DniPaciente";


                //Ejecuta la consulta
                return conn.Execute(query, new { DniPaciente = dniPaciente }) == 1;

            }
        }

        public Paciente GetPaciente(string dniPaciente)
        {
            using (IDbConnection conn = new SqlConnection(StringConnection))
            {
                string query = @"
                    SELECT * 
                    FROM Paciente 
                    WHERE dniPaciente = @DniPaciente;";

                //Retorno de nuestra query, si no encuentra el paciente devulve null
                Paciente result = conn.QuerySingleOrDefault<Paciente>(query, new { DniPaciente = dniPaciente });
                return result;

            }

        }

        public string GetPacienteNombre(string DniPaciente)
        {
            using (IDbConnection conn = new SqlConnection(StringConnection))
            {
                // Consulta para obtener solo el nombre del paciente
                string query = "SELECT nombrePaciente FROM PACIENTE WHERE dniPaciente = @DniPaciente";

                // Ejecutar la consulta y obtener el resultado
                string result = conn.QuerySingle<string>(query, new { DniPaciente });

                // Retornar el nombre del paciente
                return result;
            }
        }

        public IEnumerable<Paciente> GetPacientes()
        {
            using (IDbConnection conn = new SqlConnection(StringConnection))
            {
                //Treamos todos los pacientes
                string query = "SELECT * FROM Paciente";

                //Guardamos lo que retorno
                IEnumerable<Paciente> result = conn.Query<Paciente>(query);

                return result;
            }

        }




        public IEnumerable<Paciente> GetPacientesActivos()
        {
            using (IDbConnection conn = new SqlConnection(StringConnection))
            {
                //Treamos todos los pacientes
                string query = "SELECT * FROM Paciente WHERE estado = 'Activo'";

                //Guardamos lo que retorno
                IEnumerable<Paciente> result = conn.Query<Paciente>(query);

                return result;
            }
        }

        public bool ModificarPaciente(string ModificacionDni, Paciente paciente)
        {
            using (IDbConnection conn = new SqlConnection(StringConnection))
            {
                string query = @"
            UPDATE Paciente
            SET
                nombrePaciente = @NombrePaciente,
                emailPaciente = @EmailPaciente,
                telefonoPaciente = @TelefonoPaciente
            WHERE dniPaciente = @DniPaciente";

                // Aquí usamos directamente los valores del paciente pasado como parámetro.
                return conn.Execute(query, new
                {
                    DniPaciente = ModificacionDni,
                    paciente.NombrePaciente,
                    paciente.EmailPaciente,
                    paciente.TelefonoPaciente
                }) == 1;
            }
        }
    }
}



    

