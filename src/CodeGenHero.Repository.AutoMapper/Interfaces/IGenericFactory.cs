using System.Collections.Generic;

namespace CodeGenHero.Repository.AutoMapper
{
	public interface IGenericFactory<TEntity, TDto>
	{
		TEntity Create(TDto item);
		TDto Create(TEntity item);
		object CreateDataShapedObject(object item, List<string> fieldList, bool childrenRequested);
		object CreateDataShapedObject(TEntity item, List<string> lstOfFields, bool childrenRequested);
	}
}