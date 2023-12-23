using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace gizzy;

public class Category
{
    public Guid Id { get; set; }

    [MaxLength(30, ErrorMessage = "No more than 30 characters")]
    [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Name cannot contain special characters")]
    public required string Name { get; set; }
    [Range(1, 100, ErrorMessage = "It must be between 1 and 100")]
    [DisplayName("Display Order")]
    public long DisplayOrder { get; set; }
}
