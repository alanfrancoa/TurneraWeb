using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

using System.Text;
using System.Threading.Tasks;
using UNLZ.Turnera.Manager.Entidades.Auditoria;

namespace UNLZ.Turnera.Manager.Entidades
{
    public class Usuario : Audit
    {

        // Atributos de la entidad Usuaurio
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string ApellidoUsuario { get; set; }
        public string EmailUsuario { get; set; }
        public string? PasswordUsuario { get; set; }
        public string? GoogleIdentificador { get; set; }
    }
}
