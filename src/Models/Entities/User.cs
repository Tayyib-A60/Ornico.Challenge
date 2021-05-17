using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Enums;
using Model.Attributes;

namespace Models.Entities
{
    [Table("Users")]
    [TableName("Users")]
    [Serializable]
    public class User : BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public Role Role { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool EmailVerified { get; set; }
    }
}
