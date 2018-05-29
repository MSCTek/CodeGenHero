using System.Linq;
using System.Threading.Tasks;

namespace CodeGenHero.Repository
{
	public interface ICRUDOperation<EntityType> where EntityType : class
	{
		Task<IRepositoryActionResult<EntityType>> DeleteAsync(EntityType item);

		Task<EntityType> GetFirstOrDefaultAsync(EntityType item);

		IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class;

		Task<IRepositoryActionResult<EntityType>> InsertAsync(EntityType item);

		Task<IRepositoryActionResult<EntityType>> UpdateAsync(EntityType item);
	}
}