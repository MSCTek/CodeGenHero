using System.Linq;
using System.Threading.Tasks;

namespace CodeGenHero.Repository
{
	public interface ICRUDOperation<EntityType> where EntityType : class
	{
		Task<RepositoryActionResult<EntityType>> DeleteAsync(EntityType item);

		Task<EntityType> GetFirstOrDefaultAsync(EntityType item);

		IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class;

		Task<RepositoryActionResult<EntityType>> InsertAsync(EntityType item);

		Task<RepositoryActionResult<EntityType>> UpdateAsync(EntityType item);
	}
}