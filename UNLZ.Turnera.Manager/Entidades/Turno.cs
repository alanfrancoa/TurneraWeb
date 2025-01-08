using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNLZ.Turnera.Manager.Entidades
{
    public class Turno
    {
        public int IdTurno { get; set; }
        public DateTime FechaCreacion { set; get; }
        public TimeSpan HoraCreacion { get; set; }

        public int IdProfesional { set; get; }
        public String DniPaciente { get; set; }
        public int IdDisponibilidad { get; set; }
        public String EstadoTurno  { get; set; }

    }
}
