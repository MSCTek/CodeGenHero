using System.Collections.Generic;
using CodeGenHero.Core;
using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Template.Models.Interfaces;
using cghm = CodeGenHero.Core.Metadata;

namespace MSC.CodeGenHero.Library
{
    public class ProcessModel : BaseMarshalByRefObject, IProcessModel
    {
        public ProcessModel(ITemplateIdentity templateIdentity,
            IDictionary<string, string> templateVariables, string runName, IMetadataSourceProperties metadataSourceProperties,
            IList<IEntityNavigation> excludedNavigationProperties,
            string baseWritePath, IModel metadataSourceModel) : this()
        {
            Errors = new List<string>();
            //TemplateName = templateName;
            //TemplateId = templateId;
            //TemplateVersion = templateVersion;
            TemplateIdentity = templateIdentity;
            RunName = runName;
            //DataSource = dataSource;
            //SchemaText = schemaText;
            BaseWritePath = baseWritePath;
            MetadataSourceModel = metadataSourceModel;
            MetadataSourceProperties = metadataSourceProperties;
            ExcludedNavigationProperties = excludedNavigationProperties;
            //ConnectionStringName = connectionStringName;
            TemplateVariables = templateVariables;
            FileCount = 0;
        }

        private ProcessModel()
        {
        }

        public string BaseWritePath { get; set; }

        //public string ConnectionStringName { get; set; }
        //public string DataSource { get; set; }

        //public IList<IEntityType> EntityTypes { get; set; }
        public IList<string> Errors { get; set; }

        public IList<IEntityNavigation> ExcludedNavigationProperties { get; set; }

        // Not sure why this was using IForeignKey.  It was using INavigationProperty on the 2017 version.
        //public IList<IForeignKey> ExcludeCircularReferenceNavigationProperties { get; set; }
        public int FileCount { get; set; }

        public IModel MetadataSourceModel { get; set; }

        public IMetadataSourceProperties MetadataSourceProperties { get; set; }

        public string RunName { get; set; }

        public string SchemaText { get; set; }

        public ITemplateIdentity TemplateIdentity { get; set; }

        public IDictionary<string, string> TemplateVariables { get; set; }
    }
}