namespace AmBlitz.Domain
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}
