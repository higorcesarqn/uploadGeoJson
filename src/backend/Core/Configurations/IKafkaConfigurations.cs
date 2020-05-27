namespace Core.Configurations
{
    public interface IKafkaConfigurations
    {
        public string BootstrapServers { get; }
        public string Topic { get; }
    }
}
