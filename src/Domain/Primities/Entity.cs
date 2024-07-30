namespace Ecommerce.Domain.Shared.Primitives;

public abstract class Entity<TKey> : IEquatable<Entity<TKey>>
    where TKey : notnull
{
    protected Entity(TKey id)
    {
        Id = id;
    }

    protected Entity()
    {
    }

    public TKey Id { get; private init; } = default!;

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TKey> other)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (GetType() != other.GetType())
        {
            return false;
        }

        if (Id is null || other.Id is null)
        {
            return false;
        }

        return Id.Equals(other.Id);
    }

    public static bool operator ==(Entity<TKey> a, Entity<TKey> b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(Entity<TKey> a, Entity<TKey> b) => !(a == b);

    public override int GetHashCode() => (GetType().ToString() + Id).GetHashCode();

    public bool Equals(Entity<TKey>? other)
    {
        return Equals((object?)other);
    }
}