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
    public partial class BingoInstanceEventType
    {
        public int BingoInstanceEventTypeId { get; set; } // BingoInstanceEventTypeId (Primary key)
        public string Name { get; set; } // Name (length: 250)

        // Reverse navigation

        /// <summary>
        /// Child BingoInstanceEvents where [BingoInstanceEvent].[BingoInstanceEventTypeId] point to this entity (FK_BingoInstanceEvent_BingoInstanceEventType)
        /// </summary>
        public System.Collections.Generic.ICollection<BingoInstanceEvent> BingoInstanceEvents { get; set; } // BingoInstanceEvent.FK_BingoInstanceEvent_BingoInstanceEventType

        public BingoInstanceEventType()
        {
            BingoInstanceEvents = new System.Collections.Generic.List<BingoInstanceEvent>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
