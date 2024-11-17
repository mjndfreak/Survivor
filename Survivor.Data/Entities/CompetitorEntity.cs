namespace Survivor.Data.Entities;

public class CompetitorEntity : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int CategoryId { get; set; }
    public CategoryEntity Category { get; set; }
}