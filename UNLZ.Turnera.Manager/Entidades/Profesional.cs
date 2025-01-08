using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNLZ.Turnera.Manager.Entidades
{
    public class Profesional
    {
        public int IdProfesional { get; set; }
        public required String NombreProfesional { set; get; }
        public required String EmailProfesional { get; set; }

        public required String TelefonoProfesional { set; get; }

        public int IdEspecialidad { get; set; }

        public String Estado { get; set; }
    }
}
