using System;
using CodeGenHero.Core;
using CodeGenHero.Template.Models;

namespace CodeGenHero.Template.CSLA
{
    public abstract class BaseCSLATemplate : BaseTemplate
    {
        public BaseCSLATemplate()
        {
        }

        internal void AddError(ref TemplateOutput templateOutput, Exception ex, Enums.LogLevel logLevel)
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

        protected void AddTemplateVariablesManagerErrorsToRetVal(ref TemplateOutput retVal, Enums.LogLevel logLevel)
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

        //protected T DeserializeJsonObject<T>(string json)
        //{
        //    if (string.IsNullOrWhiteSpace(json))
        //    {
        //        return default(T);
        //    }

        //    try
        //    {
        //        return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        //    }
        //    catch (Exception)
        //    {
        //        return default(T);
        //    }
        //}
    }
}