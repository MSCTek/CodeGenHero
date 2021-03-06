// <auto-generated>
// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable EmptyNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantOverridenMember
// ReSharper disable UseNameofExpression
// TargetFrameworkVersion = 4.6
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning


namespace CodeGenHero.BingoBuzz.Repository.Entities.BB
{

    // BingoInstanceEventType
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class BingoInstanceEventTypeConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<BingoInstanceEventType>
    {
        public BingoInstanceEventTypeConfiguration()
            : this("dbo")
        {
        }

        public BingoInstanceEventTypeConfiguration(string schema)
        {
            ToTable("BingoInstanceEventType", schema);
            HasKey(x => x.BingoInstanceEventTypeId);

            Property(x => x.BingoInstanceEventTypeId).HasColumnName(@"BingoInstanceEventTypeId").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.Name).HasColumnName(@"Name").HasColumnType("nvarchar").IsRequired().HasMaxLength(250);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
