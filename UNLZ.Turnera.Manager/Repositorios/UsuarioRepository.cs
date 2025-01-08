using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNLZ.Turnera.Manager.Entidades;
using UNLZ.Turnera.Manager.Repositorios;


namespace UNLZ.Turnera.Manager.Repositorios
{
    // Interfaz para implementar las acciones de Login de usuario
    public interface IUsuarioRepository
    {
        int CrearUsuario(Usuario usuario);
        Usuario? GetUsuarioPorGoogleSubject(string googleSubject);
        Usuario? GetUsuarioPorId(int IdUsuario);
        IEnumerable<Usuario> GetUsuarios();
        Usuario? GetUsuarioPorEmailYContraseña(string email, string contraseña);
    }


}
     public class UsuarioRepository : IUsuarioRepository
    {
        // String de conexión como dependencia
        private string _connectionString;

        //CONSTRUCTOR//
        public UsuarioRepository(string connectionStrings)
        {
            _connectionString= connectionStrings;
        }

        //METODOS INTERFAZ//

        //Obtenemos la lista de todos los usuarios//
        public IEnumerable<Usuario> GetUsuarios()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
            List<Usuario> usuarios = db.Query<Usuario>("SELECT * FROM Usuario").ToList();
                return usuarios;
            }

        }

        //traemos al user por su ID//
        public Usuario? GetUsuarioPorId(int IdUsuario)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Usuario usuario = db.Query<Usuario>("SELECT * FROM Usuario WHERE IdUsuario = " + IdUsuario.ToString()).FirstOrDefault();

                return usuario;
            }
        }


        //Traemos un usuario de la DB usando el identificador de google//
        public Usuario? GetUsuarioPorGoogleSubject(string googleSubject)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Usuario usuario = db.Query<Usuario>("SELECT * FROM Usuario WHERE GoogleIdentificador = '" + googleSubject.ToString() + "'").FirstOrDefault();
                return usuario;
            }
                
        }

        //Creamos un nuevo usuario en la DB y nos devuelve ese id recien creado//
        public int CrearUsuario(Usuario usuario)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Usuario (nombreUsuario, apellidoUsuario, emailUsuario, passwordUsuario, GoogleIdentificador)  
                         VALUES (@NombreUsuario, @ApellidoUsuario, @EmailUsuario, @PasswordUsuario, @GoogleIdentificador  );                    
                         SELECT CAST(SCOPE_IDENTITY() AS INT)";

                usuario.IdUsuario = db.QuerySingle<int>(query, usuario);


                return usuario.IdUsuario;
            }
                
        }

    public Usuario? GetUsuarioPorEmailYContraseña(string email, string contraseña)
    {
        using (IDbConnection db = new SqlConnection(_connectionString))
        {
            // verifica si el correo y la contraseña coinciden
            string query = @"
            SELECT * 
            FROM Usuario
            WHERE emailUsuario = @EmailUsuario 
              AND passwordUsuario = @PasswordUsuario";
            Usuario usuario = db.Query<Usuario>(query, new { EmailUsuario = email, PasswordUsuario = contraseña }).FirstOrDefault();

            return usuario;
        }
    }
}

