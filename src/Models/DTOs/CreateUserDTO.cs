using Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DTOs
{
    public class CreateUserDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public Role Role { get; set; }
        public string Password { get; set; }
    }

    public class CreateStoryDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public class UpdateStoryDTO 
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string StoryID{ get; set; }
    }
}
