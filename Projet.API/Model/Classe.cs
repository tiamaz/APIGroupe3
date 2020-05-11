using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Projet.API.Model
{
    public class Classe
    {
        public int ClasseId { get; set; }
        [Required]
        public string Nom { get; set; }
        [Required]
        public string Filiere { get; set; }
        [Required]
        public int Niveau { get; set; }
        [Required]
        public int NombreEtudiant  { get; set; }

        [Required]
        public int UserId { get; set; }

        public ICollection<Publication> publication { get; set; }
        public ICollection<EtudiantClasse> etudiantClasse { get; set; }

    }
}