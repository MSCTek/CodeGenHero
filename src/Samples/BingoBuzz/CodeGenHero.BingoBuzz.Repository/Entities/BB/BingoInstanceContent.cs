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

    // BingoInstanceContent
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class BingoInstanceContent
    {
        public System.Guid BingoInstanceContentId { get; set; } // BingoInstanceContentId (Primary key)
        public System.Guid BingoContentId { get; set; } // BingoContentId
        public System.Guid BingoInstanceId { get; set; } // BingoInstanceId
        public System.Guid UserId { get; set; } // UserId
        public int Col { get; set; } // Col
        public int Row { get; set; } // Row
        public bool FreeSquareIndicator { get; set; } // FreeSquareIndicator
        public int BingoInstanceContentStatusTypeId { get; set; } // BingoInstanceContentStatusTypeId
        public System.DateTime CreatedDate { get; set; } // CreatedDate
        public System.Guid CreatedUserId { get; set; } // CreatedUserId
        public System.DateTime UpdatedDate { get; set; } // UpdatedDate
        public System.Guid UpdatedUserId { get; set; } // UpdatedUserId
        public bool IsDeleted { get; set; } // IsDeleted

        // Reverse navigation

        /// <summary>
        /// Child BingoInstanceEvents where [BingoInstanceEvent].[BingoInstanceContentId] point to this entity (FK_BingoInstanceEvent_BingoInstanceContent)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<BingoInstanceEvent> BingoInstanceEvents { get; set; } // BingoInstanceEvent.FK_BingoInstanceEvent_BingoInstanceContent

        // Foreign keys

        /// <summary>
        /// Parent BingoContent pointed by [BingoInstanceContent].([BingoContentId]) (FK_BingoInstanceContent_BingoContent)
        /// </summary>
        public virtual BingoContent BingoContent { get; set; } // FK_BingoInstanceContent_BingoContent

        /// <summary>
        /// Parent BingoInstance pointed by [BingoInstanceContent].([BingoInstanceId]) (FK_BingoInstanceContent_BingoInstance)
        /// </summary>
        public virtual BingoInstance BingoInstance { get; set; } // FK_BingoInstanceContent_BingoInstance

        /// <summary>
        /// Parent BingoInstanceContentStatusType pointed by [BingoInstanceContent].([BingoInstanceContentStatusTypeId]) (FK_BingoInstanceContent_BingoInstanceContentStatusType)
        /// </summary>
        public virtual BingoInstanceContentStatusType BingoInstanceContentStatusType { get; set; } // FK_BingoInstanceContent_BingoInstanceContentStatusType

        /// <summary>
        /// Parent User pointed by [BingoInstanceContent].([CreatedUserId]) (FK_BingoInstanceContent_User_Created)
        /// </summary>
        public virtual User CreatedUser { get; set; } // FK_BingoInstanceContent_User_Created

        /// <summary>
        /// Parent User pointed by [BingoInstanceContent].([UpdatedUserId]) (FK_BingoInstanceContent_User_Updated)
        /// </summary>
        public virtual User UpdatedUser { get; set; } // FK_BingoInstanceContent_User_Updated

        /// <summary>
        /// Parent User pointed by [BingoInstanceContent].([UserId]) (FK_BingoInstanceContent_User)
        /// </summary>
        public virtual User User_UserId { get; set; } // FK_BingoInstanceContent_User

        public BingoInstanceContent()
        {
            BingoInstanceContentId = System.Guid.NewGuid();
            FreeSquareIndicator = true;
            BingoInstanceContentStatusTypeId = 1;
            IsDeleted = false;
            BingoInstanceEvents = new System.Collections.Generic.List<BingoInstanceEvent>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>