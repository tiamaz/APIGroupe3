using System.ComponentModel.DataAnnotations;

namespace Projet.API.Model
{
    public class EtudiantClasse
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ClasseId { get; set; }
    }
}