using Microsoft.AspNetCore.Mvc.Rendering;
using UNLZ.Turnera.Manager.Entidades;

namespace UNLZ.Turnera.Web.Models
{
    public class TurnoModel
    {
        public Turno model { get; set; }
        
        // Lista de profesionales para el Dropdown
        public List<SelectListItem> ListaProfesionales { get; set; }
        public List<SelectListItem> ListaPacientes { get; set; }
        public List<SelectListItem> ListaDisponibilidades{ get; set; }

       
    }
}
