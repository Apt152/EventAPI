namespace IventisEventApi.Models
{
    public interface IEntity
    {
        Guid Id { get; set; }

        bool IsComplete()
        {
            if (Id == Guid.Empty) return false;
            return true;
        }
    }
}
