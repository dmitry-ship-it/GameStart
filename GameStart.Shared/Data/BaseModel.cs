namespace GameStart.Shared.Data
{
    public abstract class BaseModel : IEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
