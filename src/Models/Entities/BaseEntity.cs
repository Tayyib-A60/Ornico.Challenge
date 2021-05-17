using System;

namespace Models.Entities
{
    [Serializable]
    public class BaseEntity
    {
        public Guid ID { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}
