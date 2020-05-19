using Microsoft.AspNetCore.Mvc;
using Projet.API.Data;

namespace Projet.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ClasseController: ControllerBase
    { 
        /// Repository pattern
        private readonly ApplicationDbContext _db;
        public ClasseController(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public IActionResult Ajouter()
        {
            _db.classe()
        }
    }
}