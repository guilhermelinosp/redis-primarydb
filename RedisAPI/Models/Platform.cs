using System.ComponentModel.DataAnnotations;

namespace RedisAPI.Models
{
    public class Platform
    {
        public string Id { get; set; } = $"{Guid.NewGuid()}";

        [Required]
        public string? Name { get; set; }
    }
}
