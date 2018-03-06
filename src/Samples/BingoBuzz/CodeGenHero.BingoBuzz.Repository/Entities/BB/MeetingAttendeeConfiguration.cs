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

    // MeetingAttendee
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class MeetingAttendeeConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<MeetingAttendee>
    {
        public MeetingAttendeeConfiguration()
            : this("dbo")
        {
        }

        public MeetingAttendeeConfiguration(string schema)
        {
            ToTable("MeetingAttendee", schema);
            HasKey(x => x.MeetingAttendeeId);

            Property(x => x.MeetingAttendeeId).HasColumnName(@"MeetingAttendeeId").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.MeetingId).HasColumnName(@"MeetingId").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.UserId).HasColumnName(@"UserId").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.NotificationRuleId).HasColumnName(@"NotificationRuleId").HasColumnType("uniqueidentifier").IsOptional();
            Property(x => x.CreatedDate).HasColumnName(@"CreatedDate").HasColumnType("datetime2").IsRequired();
            Property(x => x.CreatedUserId).HasColumnName(@"CreatedUserId").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.UpdatedDate).HasColumnName(@"UpdatedDate").HasColumnType("datetime2").IsRequired();
            Property(x => x.UpdatedUserId).HasColumnName(@"UpdatedUserId").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.IsDeleted).HasColumnName(@"IsDeleted").HasColumnType("bit").IsRequired();

            // Foreign keys
            HasOptional(a => a.NotificationRule).WithMany(b => b.MeetingAttendees).HasForeignKey(c => c.NotificationRuleId).WillCascadeOnDelete(false); // FK_MeetingAttendee_NotificationRule
            HasRequired(a => a.CreatedUser).WithMany(b => b.MeetingAttendees_CreatedUserId).HasForeignKey(c => c.CreatedUserId).WillCascadeOnDelete(false); // FK_MeetingAttendee_User_Created
            HasRequired(a => a.Meeting).WithMany(b => b.MeetingAttendees).HasForeignKey(c => c.MeetingId).WillCascadeOnDelete(false); // FK_MeetingAttendee_Meeting
            HasRequired(a => a.UpdatedUser).WithMany(b => b.MeetingAttendees_UpdatedUserId).HasForeignKey(c => c.UpdatedUserId).WillCascadeOnDelete(false); // FK_MeetingAttendee_User_Updated
            HasRequired(a => a.User_UserId).WithMany(b => b.MeetingAttendees_UserId).HasForeignKey(c => c.UserId).WillCascadeOnDelete(false); // FK_MeetingAttendee_User
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>