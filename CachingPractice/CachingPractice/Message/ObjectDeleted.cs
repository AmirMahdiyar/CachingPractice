namespace Message
{
    public class ObjectDeleted
    {
        public string Key { get; set; }
        public Guid InstanceId { get; set; }

        public ObjectDeleted(string key, Guid instanceId)
        {
            Key = key;
            InstanceId = instanceId;
        }
    }
}
