using Postgrest.Attributes;
using Postgrest.Models;

[Table("categories")]
public class CategoryModel : BaseModel, IIdentity
{
    [PrimaryKey("id", false)]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    public CategoryModel NewCategoryModel(string name)
    {
        Name = name;
        return this;
    }
}