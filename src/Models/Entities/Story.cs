using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Enums;
using Model.Attributes;

namespace Models.Entities
{
    [Table("Stories")]
    [TableName("Stories")]
    [Serializable]
    public class Story : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public StoryType Type { get; set; }
        public Guid UserID { get; set; }
        public int Votes { get; set; }
        public string Author { get; set; }
    }
}
