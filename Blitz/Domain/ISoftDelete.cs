namespace Blitz.Domain
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}
