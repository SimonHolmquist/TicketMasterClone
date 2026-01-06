namespace Booking.Domain.SeedWork;

public abstract class Entity
{
    int _requestedHashCode;
    int _id;

    // Usamos ID numérico para SQL, pero podrías usar GUID
    public virtual int Id
    {
        get { return _id; }
        protected set { _id = value; }
    }

    public bool IsTransient()
    {
        return this.Id == default;
    }

    // Igualdad basada en ID (clave para DDD)
    public override bool Equals(object? obj)
    {
        if (obj == null || obj is not Entity)
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (this.GetType() != obj.GetType())
            return false;

        Entity item = (Entity)obj;

        if (item.IsTransient() || this.IsTransient())
            return false;
        else
            return item.Id == this.Id;
    }

    public override int GetHashCode()
    {
        if (!IsTransient())
        {
            if (!_requestedHashCode.Equals(default))
                return _requestedHashCode;

            _requestedHashCode = this.Id.GetHashCode() ^ 31;
            return _requestedHashCode;
        }
        else
            return base.GetHashCode();
    }
}