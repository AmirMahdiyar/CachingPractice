namespace Message
{
    public class ObjectKey
    {
        public string Key { get; set; }
        public Guid InstanceId { get; set; }

        public ObjectKey(string key, Guid instanceId)
        {
            Key = key;
            InstanceId = instanceId;
        }
    }
}
