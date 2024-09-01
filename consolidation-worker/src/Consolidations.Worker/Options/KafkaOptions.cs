namespace Consolidations.Worker.Options;

public class KafkaOptions
{
    public string BootstrapServers { get; set; }
    public string GroupId { get; set; }
}