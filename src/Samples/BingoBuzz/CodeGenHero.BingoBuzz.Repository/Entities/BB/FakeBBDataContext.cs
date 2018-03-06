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

    using System.Linq;

    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class FakeBBDataContext : IBBDataContext
    {
        public System.Data.Entity.DbSet<BingoContent> BingoContents { get; set; }
        public System.Data.Entity.DbSet<BingoInstance> BingoInstances { get; set; }
        public System.Data.Entity.DbSet<BingoInstanceContent> BingoInstanceContents { get; set; }
        public System.Data.Entity.DbSet<BingoInstanceContentStatusType> BingoInstanceContentStatusTypes { get; set; }
        public System.Data.Entity.DbSet<BingoInstanceEvent> BingoInstanceEvents { get; set; }
        public System.Data.Entity.DbSet<BingoInstanceEventType> BingoInstanceEventTypes { get; set; }
        public System.Data.Entity.DbSet<BingoInstanceStatusType> BingoInstanceStatusTypes { get; set; }
        public System.Data.Entity.DbSet<Company> Companies { get; set; }
        public System.Data.Entity.DbSet<FrequencyType> FrequencyTypes { get; set; }
        public System.Data.Entity.DbSet<Meeting> Meetings { get; set; }
        public System.Data.Entity.DbSet<MeetingAttendee> MeetingAttendees { get; set; }
        public System.Data.Entity.DbSet<MeetingSchedule> MeetingSchedules { get; set; }
        public System.Data.Entity.DbSet<NotificationMethodType> NotificationMethodTypes { get; set; }
        public System.Data.Entity.DbSet<NotificationRule> NotificationRules { get; set; }
        public System.Data.Entity.DbSet<RecurrenceRule> RecurrenceRules { get; set; }
        public System.Data.Entity.DbSet<User> Users { get; set; }

        public FakeBBDataContext()
        {
            BingoContents = new FakeDbSet<BingoContent>("BingoContentId");
            BingoInstances = new FakeDbSet<BingoInstance>("BingoInstanceId");
            BingoInstanceContents = new FakeDbSet<BingoInstanceContent>("BingoInstanceContentId");
            BingoInstanceContentStatusTypes = new FakeDbSet<BingoInstanceContentStatusType>("BingoInstanceContentStatusTypeId");
            BingoInstanceEvents = new FakeDbSet<BingoInstanceEvent>("BingoInstanceEventId");
            BingoInstanceEventTypes = new FakeDbSet<BingoInstanceEventType>("BingoInstanceEventTypeId");
            BingoInstanceStatusTypes = new FakeDbSet<BingoInstanceStatusType>("BingoInstanceStatusTypeId");
            Companies = new FakeDbSet<Company>("CompanyId");
            FrequencyTypes = new FakeDbSet<FrequencyType>("FrequencyTypeId");
            Meetings = new FakeDbSet<Meeting>("MeetingId");
            MeetingAttendees = new FakeDbSet<MeetingAttendee>("MeetingAttendeeId");
            MeetingSchedules = new FakeDbSet<MeetingSchedule>("MeetingScheduleId");
            NotificationMethodTypes = new FakeDbSet<NotificationMethodType>("NotificationMethodTypeId");
            NotificationRules = new FakeDbSet<NotificationRule>("NotificationRuleId");
            RecurrenceRules = new FakeDbSet<RecurrenceRule>("RecurrenceRuleId");
            Users = new FakeDbSet<User>("UserId");

            InitializePartial();
        }

        public int SaveChangesCount { get; private set; }
        public int SaveChanges()
        {
            ++SaveChangesCount;
            return 1;
        }

        public System.Threading.Tasks.Task<int> SaveChangesAsync()
        {
            ++SaveChangesCount;
            return System.Threading.Tasks.Task<int>.Factory.StartNew(() => 1);
        }

        public System.Threading.Tasks.Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken)
        {
            ++SaveChangesCount;
            return System.Threading.Tasks.Task<int>.Factory.StartNew(() => 1, cancellationToken);
        }

        partial void InitializePartial();

        protected virtual void Dispose(bool disposing)
        {
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private System.Data.Entity.Infrastructure.DbChangeTracker _changeTracker;
        public System.Data.Entity.Infrastructure.DbChangeTracker ChangeTracker { get { return _changeTracker; } }
        private System.Data.Entity.Infrastructure.DbContextConfiguration _configuration;
        public System.Data.Entity.Infrastructure.DbContextConfiguration Configuration { get { return _configuration; } }
        private System.Data.Entity.Database _database;
        public System.Data.Entity.Database Database { get { return _database; } }
        public System.Data.Entity.Infrastructure.DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        {
            throw new System.NotImplementedException();
        }
        public System.Data.Entity.Infrastructure.DbEntityEntry Entry(object entity)
        {
            throw new System.NotImplementedException();
        }
        public System.Collections.Generic.IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> GetValidationErrors()
        {
            throw new System.NotImplementedException();
        }
        public System.Data.Entity.DbSet Set(System.Type entityType)
        {
            throw new System.NotImplementedException();
        }
        public System.Data.Entity.DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            throw new System.NotImplementedException();
        }
        public override string ToString()
        {
            throw new System.NotImplementedException();
        }

    }
}
// </auto-generated>