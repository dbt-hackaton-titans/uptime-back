using System.ComponentModel.DataAnnotations;

namespace Titans.Uptime.Domain.Models
{
    public class SystemEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Component> Components { get; set; } = new List<Component>();
        public virtual ICollection<UptimeCheck> UptimeChecks { get; set; } = new List<UptimeCheck>();
    }
}
