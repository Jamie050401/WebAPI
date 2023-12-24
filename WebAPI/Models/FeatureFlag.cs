using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models;

public class FeatureFlag
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public required string Feature { get; set; }
    public bool IsEnabled { get; set; }
}
