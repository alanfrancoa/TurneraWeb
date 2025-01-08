using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNLZ.Turnera.Manager.Entidades
{
    public class Disponibilidad
    {
        public int IdDisponibilidad { get; set; }
        public required TimeSpan HoraInicio { get; set; }
        public required TimeSpan HoraFin { get; set; }
        public int IdProfesional { set; get; }
        public required DateTime Dia { get; set; }
        public String Estado { get; set; }

    }
}

