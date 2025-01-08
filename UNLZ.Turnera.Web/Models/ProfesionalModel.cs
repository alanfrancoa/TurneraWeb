using Microsoft.AspNetCore.Mvc.Rendering;
using UNLZ.Turnera.Manager.Entidades;

namespace UNLZ.Turnera.Web.Models
{
    public class ProfesionalModel
    {
        public Profesional model { get; set; }

        public List<SelectListItem> ListaEspecialidadesItem { get; set; }
    }
}
