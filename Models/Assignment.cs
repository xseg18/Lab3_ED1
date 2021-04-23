using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Lab3_ED1.Models
{
    public class Assignment
    {
        [Display(Name = "Título")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "Proyecto")]
        [Required]
        public string Project { get; set; }
        [Display(Name = "Descripción")]
        [Required]
        public string Description { get; set; }
        [Display(Name = "Prioridad")]
        [Required]
        public int Priority { get; set; }
        [Display(Name = "Fecha de entrega")]
        [Required]
        public DateTime Date { get; set; }
    }
}
