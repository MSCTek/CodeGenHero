using CodeGenHero.Core;
using CodeGenHero.Inflector;
using CodeGenHero.Template.Helpers;
using CodeGenHero.Template.Interfaces;
using CodeGenHero.Template.Models.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static CodeGenHero.Core.Enums;
using tConsts = CodeGenHero.Template.Constants.Consts;

namespace CodeGenHero.Template.Models
{
    public abstract class BaseTemplate : BaseMarshalByRefObject, ITemplate
    {
        private readonly TemplateAttribute _templateAttribute;
        private List<PropertyInfo> _templateVariablePropertyInfos;
        private ITemplateVariablesManager _templateVariablesManager;

        public BaseTemplate()
        {
            var templateAttributes = this.GetType().UnderlyingSystemType.GetCustomAttributes(typeof(TemplateAttribute), true);
            _templateAttribute = templateAttributes[0] as TemplateAttribute;
            //Status = TemplateStatus.Idle;
        }

        [TemplateVariable(tConsts.STG_baseNamespace_DEFAULTVALUE, description: "The base part of the namespace to use in the generated file before adding prefix or postfix strings.")]
        public virtual string BaseNamespace { get; set; }

        public virtual string Description
        {
            get
            {
                return _templateAttribute.Description;
            }
        }

        public virtual Guid Id
        {
            get
            {
                return _templateAttribute.Id;
            }
        }

        public virtual string LibraryName { get; set; }

        public virtual string Name
        {
            get
            {
                return _templateAttribute.Name;
            }
        }

        [TemplateVariable(tConsts.STG_namespacePostfix_DEFAULTVALUE)]
        public virtual string NamespacePostfix { get; set; }

        [TemplateVariable(tConsts.STG_prependSchemaNameIndicator_DEFAULTVALUE)]
        public virtual bool PrependSchemaNameIndicator { get; set; }

        public virtual IProcessModel ProcessModel { get; set; }

        [TemplateVariable(tConsts.STG_regexExclude_DEFAULTVALUE)]
        public virtual string RegexExclude { get; set; }

        [TemplateVariable(tConsts.STG_regexInclude_DEFAULTVALUE)]
        public virtual string RegexInclude { get; set; }

        public virtual List<PropertyInfo> TemplateVariablePropertyInfos
        {
            get
            {
                if (_templateVariablePropertyInfos == null)
                {
                    _templateVariablePropertyInfos = new List<PropertyInfo>();
                    var templateProperties = this.GetType().UnderlyingSystemType
                        .GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance);
                    for (int i = 0; i < templateProperties.Length; i++)
                    {
                        var templateProperty = templateProperties[i];
                        var templateVariableAttribute = templateProperty.GetCustomAttribute<TemplateVariableAttribute>();
                        if (templateVariableAttribute != null)
                        {
                            _templateVariablePropertyInfos.Add(templateProperty);
                        }
                    }
                }

                return _templateVariablePropertyInfos;
            }
        }

        //public TemplateStatus Status { get; set; }
        public virtual List<TemplateVariable> TemplateVariables
        {
            get
            {
                List<TemplateVariable> retVal = new List<TemplateVariable>();
                foreach (var tvPropertyInfo in TemplateVariablePropertyInfos)
                {
                    var templateVariableAttribute = tvPropertyInfo.GetCustomAttribute<TemplateVariableAttribute>();
                    var templatePropertyValue = tvPropertyInfo.GetValue(this);
                    var templateVariable = new TemplateVariable()
                    {
                        Name = tvPropertyInfo.Name,
                        Value = templatePropertyValue?.ToString(),
                        PropertyType = tvPropertyInfo.PropertyType,
                        DefaultValue = templateVariableAttribute.DefaultValue,
                        Description = templateVariableAttribute.Description,
                        IsHidden = templateVariableAttribute.IsHidden,
                    };

                    retVal.Add(templateVariable);
                }

                return retVal;
            }
        }

        public virtual string Version
        {
            get
            {
                return _templateAttribute.Version;
            }
        }

        protected virtual ICodeGenHeroInflector Inflector { get; set; } = new CodeGenHeroInflector();

        protected virtual ITemplateVariablesManager TemplateVariablesManager
        {
            get
            {
                _templateVariablesManager = new TemplateVariablesHelper(ProcessModel.TemplateVariables);

                return _templateVariablesManager;
            }
        }

        public virtual void AddError(ref TemplateOutput templateOutput, Exception ex, Enums.LogLevel logLevel)
        {
            while (ex != null)
            {
                TemplateError te = new TemplateError()
                {
                    ErrorLevel = logLevel,
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    TemplateIdentity = ProcessModel.TemplateIdentity.Copy()
                };

                templateOutput.Errors.Add(te);
                ex = ex.InnerException;
            }
        }

        public virtual void AddTemplateVariablesManagerErrorsToRetVal(ref TemplateOutput retVal, Enums.LogLevel logLevel)
        {
            foreach (var errorItem in TemplateVariablesManager.Errors)
            {
                retVal.Errors.Add(new TemplateError()
                {
                    ErrorLevel = logLevel,
                    Message = errorItem,
                    TemplateIdentity = ProcessModel.TemplateIdentity.Copy()
                });
            }
        }

        public virtual T DeserializeJsonObject<T>(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return default(T);
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public virtual void Dispose()
        {
        }

        public virtual TemplateOutput Generate()
        {
            return new TemplateOutput();
        }

        ///// <summary>
        ///// Appends navigations to the exclusion list when the navigation ClrType is not available as an allowedEntity.
        ///// This may happen when excludeRegEx and includeRegEx patterns are used to filter out some entities that exist in metadata, but will not be generated.
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <param name="allowedEntities"></param>
        ///// <param name="excludedNavigationProperties"></param>
        ///// <returns>A superset of excluded navigation properties</returns>
        //public virtual IList<IEntityNavigation> GetExcludedNavigationProperties(
        //    IEntityType entity,
        //    IList<IEntityType> allowedEntities,
        //    IList<IEntityNavigation> excludedNavigationProperties)
        //{
        //    IList<IEntityNavigation> retVal = new List<IEntityNavigation>();

        //    if (excludedNavigationProperties != null)
        //    {
        //        retVal.AddRange(excludedNavigationProperties);
        //    }

        //    var allowedClrTypes = allowedEntities.Select(x => x.ClrType).ToList();
        //    foreach (var navigationItem in entity.Navigations)
        //    {
        //        var navigationClrType = navigationItem.ClrType;

        //        if (!allowedClrTypes.Any(x => navigationClrType.FullName.Equals(x.FullName, StringComparison.InvariantCultureIgnoreCase)
        //            || navigationClrType.FullName.IndexOf(x.FullName) > 0))
        //        {
        //            retVal.Add(new EntityNavigation()
        //            {
        //                EntityType = entity,
        //                Navigation = navigationItem
        //            });
        //        }
        //    }

        //    return retVal;
        //}

        public virtual List<TemplateError> Initialize(IProcessModel processModel)
        {
            try
            {
                var retVal = new List<TemplateError>();
                ProcessModel = processModel;

                foreach (KeyValuePair<string, string> entry in ProcessModel.TemplateVariables)
                {
                    // do something with entry.Value or entry.Key
                    var x = $"\t{entry.Key} : {entry.Value}";
                }

                foreach (var tvPropertyInfo in TemplateVariablePropertyInfos)
                {
                    string templateSettingValue = null;
                    string propertyInfoName = tvPropertyInfo.Name.ToLowerInvariant();
                    if (!ProcessModel.TemplateVariables.Keys.Contains(propertyInfoName))
                    {
                        retVal.Add(new TemplateError()
                        {
                            EventId = (int)EventId.TemplateInitializeError,
                            Message = $"Missing TemplateSetting {propertyInfoName} in ProcessModel.",
                            TemplateIdentity = ProcessModel.TemplateIdentity.Copy()
                        });
                    }
                    else
                    {
                        if (ProcessModel.TemplateVariables.TryGetValue(propertyInfoName, out templateSettingValue))
                        {
                            // If this point hits, it means the variable was in the dict, even if it's a blank or null value.  If it's null, we want to set it to blank but not show it as an error.
                            // regexexclude and include are often both empty

                            if (string.IsNullOrEmpty(templateSettingValue))
                            {
                                templateSettingValue = string.Empty; // Just in case it's null
                                //retVal.Add(new TemplateError()
                                //{
                                //    EventId = (int)EventId.TemplateSettingError,
                                //    Message = $"Missing value for TemplateSetting {propertyInfoName} in ProcessModel.",
                                //    TemplateIdentity = ProcessModel.TemplateIdentity.Copy()
                                //});
                                //continue;
                            }

                            var firstIndex = templateSettingValue.IndexOf('{');
                            var lastIndex = templateSettingValue.IndexOf('}');
                            while (firstIndex >= 0 && lastIndex >= 0 && firstIndex < lastIndex) // templateSettingValue.Contains("{") && templateSettingValue.Contains("}"))
                            {   // For variables that contain multiple replacement placeholders that require multiple passes.
                                System.Diagnostics.Debug.WriteLine($"Working on variable {templateSettingValue}");

                                var innerSettingToReplace = templateSettingValue.Substring(firstIndex + 1, lastIndex - 1 - firstIndex);
                                // templateSettingValue.Substring(templateSettingValue.IndexOf('{') + 1, templateSettingValue.IndexOf('}') - 1);
                                var innerTemplateSettingKey = ProcessModel.TemplateVariables.Keys.FirstOrDefault(
                                    x => x.ToLowerInvariant() == innerSettingToReplace.ToLowerInvariant());

                                if (innerTemplateSettingKey == null)
                                {
                                    retVal.Add(new TemplateError()
                                    {
                                        EventId = (int)EventId.TemplateSettingError,
                                        Message = $"Missing key in TemplateSetting key of {innerSettingToReplace} in ProcessModel.",
                                        TemplateIdentity = ProcessModel.TemplateIdentity.Copy()
                                    });
                                    break;
                                }
                                if (ProcessModel.TemplateVariables.TryGetValue(innerTemplateSettingKey, out string replacementSettingValue))
                                {
                                    templateSettingValue = templateSettingValue.Replace($"{{{innerSettingToReplace}}}", replacementSettingValue);
                                }
                                else
                                {
                                    break;
                                }

                                firstIndex = templateSettingValue.IndexOf('{');
                                lastIndex = templateSettingValue.IndexOf('}');
                            }
                            System.Diagnostics.Debug.WriteLine($"Done with variable {templateSettingValue}");

                            if (tvPropertyInfo.PropertyType == typeof(bool) || tvPropertyInfo.PropertyType == typeof(Boolean))
                            {
                                bool setvalue = true;
                                if (string.IsNullOrEmpty(templateSettingValue) || !templateSettingValue.Equals("true", StringComparison.OrdinalIgnoreCase))
                                {
                                    setvalue = false;
                                }
                                tvPropertyInfo.SetValue(this, setvalue);
                            }
                            else if (tvPropertyInfo.PropertyType == typeof(int))
                            {
                                tvPropertyInfo.SetValue(this, templateSettingValue);
                            }
                            else if (tvPropertyInfo.PropertyType == typeof(string))
                            {
                                if (!string.IsNullOrWhiteSpace(templateSettingValue) && templateSettingValue.EndsWith("."))
                                {   // Strip off trailing period.  This can happen if the user did not set a variable value
                                    templateSettingValue = templateSettingValue.Substring(0, templateSettingValue.Length - 1);
                                }
                                if (templateSettingValue.Contains(".."))
                                {
                                    //user did not specify value for a variable that is concatenated with other values
                                    templateSettingValue = templateSettingValue.Replace("..", ".");
                                }
                                tvPropertyInfo.SetValue(this, templateSettingValue);
                            }
                        }
                        else
                        {
                            retVal.Add(new TemplateError()
                            {
                                EventId = (int)EventId.TemplateSettingError,
                                Message = $"Missing value for TemplateSetting {propertyInfoName} in ProcessModel.",
                                TemplateIdentity = ProcessModel.TemplateIdentity.Copy()
                            });
                            continue;
                        }
                    }
                }

                return retVal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="templateVariableName"></param>
        /// <param name="value"></param>
        /// <returns>true if value was set successfully, false if variable was not found or value could not be set.</returns>
        public virtual bool SetTemplateVariable(string templateVariableName, object value)
        {
            bool retVal = false;

            var tvPropertyInfo = TemplateVariablePropertyInfos.FirstOrDefault(x => x.Name.ToLowerInvariant() == templateVariableName.ToLowerInvariant());
            if (tvPropertyInfo != null)
            {
                tvPropertyInfo.SetValue(this, value);
            }

            return retVal;
        }

        public virtual void Stop()
        {
        }
    }
}