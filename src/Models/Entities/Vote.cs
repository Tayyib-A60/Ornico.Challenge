using System;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Attributes;

namespace Models.Entities
{
    [Table("Votes")]
    [TableName("Votes")]
    public class Vote : BaseEntity
    {
        public string UserID { get; set; }
        public bool Voted { get; set; }
        public string StoryID { get; set; }
    }
}
