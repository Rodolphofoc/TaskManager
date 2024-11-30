namespace Applications.Interceptions.Model
{
    public interface IAuditLoggable
    {
        public string TableName { get; }
        public string ActionType { get; }
        public string EntityId { get; }

        public string User { get; set; }
        public string GetChanges(); 
    }
}
