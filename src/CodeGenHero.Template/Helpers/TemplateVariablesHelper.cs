using System.Collections.Generic;
using System.Linq;
using System;
using CodeGenHero.Core;
using CodeGenHero.Template.Interfaces;
using CodeGenHero.Template.Models.Interfaces;
using CodeGenHero.Template.Models;

namespace CodeGenHero.Template.Helpers
{
    public class TemplateVariablesHelper : BaseMarshalByRefObject, ITemplateVariablesManager
    {
        public TemplateVariablesHelper()
        {
            Errors = new List<string>();
            TemplateVariables = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public TemplateVariablesHelper(List<TemplateVariable> templateVariables) : this()
        {
            templateVariables.ForEach(x =>
            {
                TemplateVariables.Add(x.Name, x.Value);
            });
        }

        public TemplateVariablesHelper(IDictionary<string, string> dict) : this()
        {
            TemplateVariables = new Dictionary<string, string>(dict, StringComparer.OrdinalIgnoreCase);
        }

        public List<string> Errors { get; set; }

        public Dictionary<string, string> TemplateVariables { get; set; }

        //public static string GetValue(string name, IDictionary<string, string> settingsToSearch)
        //{
        //    TemplateVariablesHelper tsm = new TemplateVariablesHelper(settingsToSearch);
        //    return tsm.GetValue(name);
        //}

        public bool GetDictBool(string name)
        {
            return (GetValue(name) == "true") ? true : false;
        }

        public string GetOutputFile(ITemplateIdentity templateIdentity, string fileName)
        {
            string retVal = GetValue(fileName);
            retVal = System.IO.Path.GetFileName(retVal);
            retVal = $"{templateIdentity.TemplateName}\\{templateIdentity.TemplateVersion}\\{retVal}";
            return retVal;
        }

        /// <summary>
        /// Builds a return string from the name.  If the value references other keys, those keys will be replaced with their values
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetValue(string name)
        {
            if (TemplateVariables.Count() == 0)
            {
                return null;
            }
            if (name == null)
            {
                return null;
            }
            string retVal = null;
            if (!TemplateVariables.ContainsKey(name))
            {
                Errors.Add($"attribute not found for {name}");
                return retVal;
            }

            retVal = TemplateVariables[name];
            if (string.IsNullOrEmpty(retVal))
                return retVal;

            //if the value contains a quote, it's probably a json string.
            //if we ever have to include quotes in values that also store variables, we'll need to adjust this
            if (retVal.Contains("\""))
            {
                return retVal;
            }
            int numLoops = 0;
            while (retVal.Contains("{"))
            {
                string[] values = retVal.Split(new char[] { '{', '}' });
                //we want each second item here.  //"this is {to} be only converting {the} words in brackets
                for (int i = 1; i < values.Length; i += 2)
                {
                    if (TemplateVariables.ContainsKey(values[i]))
                    {
                        retVal = retVal.Replace("{" + values[i] + "}", TemplateVariables[values[i]]);
                    }
                    else
                    {
                        Errors.Add($"attribute not found for {values[i]}");
                        return retVal;
                    }
                }

                numLoops++;
                if (numLoops > 10)
                {
                    Errors.Add($"Max number of loop iterations reached while substituting dictionary values in Model.Dict.GetValue for Name: '{name}' and value: {retVal}");

                    break;
                }
            }

            return retVal;
        }

        /// <summary>
        /// returns all dictionary items that starts with name as a string list of the values.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<string> GetValueList(string name)
        {
            List<string> retVal = new List<string>();
            var load = TemplateVariables.Where(x => x.Key.StartsWith(name)).OrderBy(x => x.Key);
            //int i = 0;
            //we want to keep everything in order by the key name (usinglist1, usinglist2, etc)
            foreach (var item in load)
            {
                string key = item.Key;
                retVal.Add(this.GetValue(key)); //we do it this way so that we're passing the value through the processing code,
            }

            return retVal;
        }

        //public void Update(List<TemplateSetting> settings)
        //{
        //	foreach (TemplateSetting item in settings)
        //	{
        //		this[item.Name] = item.Value;
        //	}
        //}

        //private string[] GetJSONPair(JToken jtoken)
        //{
        //    var prop = jtoken.Value<JProperty>();
        //    string name = prop.Name;
        //    JToken jvalue = prop.Value;
        //    string value = jvalue.Value<string>();
        //    if (name.StartsWith(Consts.STG_usingList))
        //    {
        //        //we want to be able to have multiple "using" so append a number to it
        //        name = NewKey(Consts.STG_usingList);
        //    }
        //    return new string[2] { name, value };
        //}

        /// <summary>
        /// assign a new key that doesn't exist in the dictionary, based on the name, because sometimes we want many of an attrib and sometimes we don't
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string NewKey(string name)
        {
            int i = 1;
            while (TemplateVariables.ContainsKey(name + i.ToString("D3")))
            {
                i++;
            }
            return name + i.ToString("D3");
        }
    }
}