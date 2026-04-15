using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pinula.Shared.Models;

public partial class Comment
{
    public Guid RecipeId { get; set; }

    public Guid UserId { get; set; }

    public string? Text { get; set; }

    public short Rating { get; set; }

    public DateTime? CreatedAt { get; set; }
    [JsonIgnore]
    public virtual Recipe Recipe { get; set; } = null!;
    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}
