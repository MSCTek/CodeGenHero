using AutoMapper;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace CodeGenHero.Repository.AutoMapper
{
    public class GenericFactory<TEntity, TDto> : IGenericFactory<TEntity, TDto>
    {
        private IMapper _mapper;

        public GenericFactory(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDto Create(TEntity item)
        {
            return _mapper.Map<TDto>(item);
        }

        public TEntity Create(TDto item)
        {
            return _mapper.Map<TEntity>(item);
        }

        public object CreateDataShapedObject(TEntity item, List<string> lstOfFields, bool childrenRequested)
        {
            return CreateDataShapedObject(Create(item), lstOfFields, childrenRequested);
        }

        public virtual object CreateDataShapedObject(object item, List<string> fieldList, bool childrenRequested)
        {
            if (!fieldList.Any() || childrenRequested)
            {
                return item;
            }
            else
            {
                // create a new ExpandoObject & dynamically create the properties for this object
                ExpandoObject objectToReturn = new ExpandoObject();
                var itemType = item.GetType();
                foreach (var field in fieldList)
                {
                    //if (field.ToLowerInvariant() == "children")
                    //    continue;  // Special case 'children' not allowed due to its reserved user for triggering the loading of related child objects in the repository.

                    // need to include public and instance, b/c specifying a binding flag overwrites the
                    // already-existing binding flags.
                    var fieldValue = itemType
                        .GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                        .GetValue(item, null);

                    // add the field to the ExpandoObject
                    ((IDictionary<String, Object>)objectToReturn).Add(field, fieldValue);
                }

                return objectToReturn;
            }
        }
    }
}