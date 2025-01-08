using Microsoft.AspNetCore.Mvc.Rendering;
using UNLZ.Turnera.Manager.Entidades;

namespace UNLZ.Turnera.Web.Models
{
    public class DisponibilidadModel
    {
        public Disponibilidad model { get; set; }
        public List<SelectListItem> ListaProfesionalesItem { get; set; }
    }
}