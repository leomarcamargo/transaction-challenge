namespace Consolidations.Infrastructure.CrossCutting.Redis;

public class RedisOptions
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string Password { get; set; }
}