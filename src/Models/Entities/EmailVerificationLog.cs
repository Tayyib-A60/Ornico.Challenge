using Model.Attributes;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    [Table("EmailVerificationLogs")]
    [TableName("EmailVerificationLogs")]
    [Serializable]
    public class EmailVerificationLog : BaseEntity
    {
        public string Code { get; set; }
        public string Email { get; set; }
        public bool Used { get; set; }
    }
}
