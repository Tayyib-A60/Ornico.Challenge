using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Core.Enums;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Models.ViewModels
{

    class ViewModels
    {
    }
    public class StoryListResponse
    {
        public List<StoryResponse> Stories { get; set; }
        public int CurrentPage { get; set; }
        public int Count { get; set; }
    }
    public class ToggleVoteResponse
    {
        public int Votes { get; set; }
        public Guid StoryID { get; set; }
    }

    public class DeleteStoryResponse
    {
        public string StoryID { get; set; }
    }
    public class StoryResponse
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public StoryType Type { get; set; }
        public int Votes { get; set; }
        public string Author { get; set; }
        public Guid ID { get; set; }
        public DateTime CreationDate { get; set; }
    }
    public class UserLoginResponse
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public bool IsVerfied { get; set; }
        public Role Role { get; set; }
    }
    public class EmailVerificationResponse
    {
        public string Email { get; set; }
    }
}
