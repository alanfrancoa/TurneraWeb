using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNLZ.Turnera.Manager.ModelFactories
{
    public class TurnoCompleto
    {
        public int IdTurno { get; set; }
        public DateTime FechaCreacion { set; get; }
        public TimeSpan HoraCreacion { get; set; }
        public required String NombreProfesional { set; get; }
        public String DniPaciente { get; set; }
        public required TimeSpan HoraInicio { get; set; }
        public required TimeSpan HoraFin { get; set; }
        public String EstadoTurno { get; set; }
    }
}
