using System.Linq;

namespace CodeGenHero.EAMVCXamPOCO.Interface
{
	public interface ICRUDOperation<EntityType> where EntityType : class
	{
		RepositoryActionResult<EntityType> Delete(EntityType item);

		EntityType GetFirstOrDefault(EntityType item);

		IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class;

		RepositoryActionResult<EntityType> Insert(EntityType item);

		RepositoryActionResult<EntityType> Update(EntityType item);
	}
}