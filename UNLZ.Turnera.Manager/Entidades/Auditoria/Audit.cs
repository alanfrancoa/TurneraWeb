using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNLZ.Turnera.Manager.Entidades.Auditoria
{
    public class Audit
    {
        public DateTime FechaAlta { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public DateTime? FechaBaja { get; set; }
        
    }
}
