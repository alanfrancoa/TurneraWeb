using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNLZ.Turnera.Manager.Entidades
{
    public class Paciente
    {
        public required String DniPaciente { get; set; } // Clave Primaria
        public required String NombrePaciente { get; set; }
        public required String EmailPaciente { get; set; }
        public required String TelefonoPaciente { get; set; }
        public String Estado {  get; set; }
    }
}
