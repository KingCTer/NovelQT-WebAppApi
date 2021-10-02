using System;

namespace NovelQT.Domain.Core.Models
{
    public abstract class EntityTrackable : Entity
    {
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
