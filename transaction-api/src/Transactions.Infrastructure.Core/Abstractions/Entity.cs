namespace Transactions.Infrastructure.Core.Abstractions;

public abstract class Entity : IEquatable<Entity>
{
    public Guid Id { get; set; }
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public bool IsDeleted { get; set; }

    public bool Equals(Entity? other)
    {
        return Id == other?.Id;
    }
}