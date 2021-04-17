using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Lab3_ED1.Models
{
    public class Assignment
    {
        [Display(Name = "Nombre")]
        [Required]
        public int Name { get; set; }
        [Display(Name = "Proyecto")]
        [Required]
        public string Project { get; set; }
        [Display(Name = "Precio")]
        [Required]
        public decimal Price { get; set; }
        [Display(Name = "Descripción")]
        [Required]
        public string Description { get; set; }
        [Display(Name = "Prioridad")]
        [Required]
        public string Priority { get; set; }
        [Display(Name = "Fecha de entrega")]
        [Required]
        public int Date { get; set; }
    }
}
