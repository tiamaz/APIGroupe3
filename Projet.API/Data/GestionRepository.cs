using System.Threading.Tasks;

namespace Projet.API.Data
{
    public class GestionRepository : IGestionRepository
    {
        private readonly ApplicationDbContext _db;

        public GestionRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Add<T>(T entity) where T : class
        {
            _db.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _db.Remove(entity);
        }

        public async Task<bool> SaveChanges()
        {
            return await _db.SaveChangesAsync() > 0; 
        }
    }
}