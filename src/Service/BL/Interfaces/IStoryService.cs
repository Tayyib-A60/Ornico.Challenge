using Models.DTOs;
using Models.Entities;
using Models.ViewModels;
using Services.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.BL.Interfaces
{
    public interface IStoryService : IAutoDependencyService
    {
        Task<BaseResponse<StoryResponse>> CreateStory(CreateStoryDTO createStoryDTO, string userID);
        Task<BaseResponse<StoryResponse>> UpdateStory(UpdateStoryDTO createStoryDTO, string userID);
        Task<BaseResponse<DeleteStoryResponse>> DeleteStory(string storyID, string userID);
        Task<BaseResponse<ToggleVoteResponse>> ToggleVote(string storyID, string userID);
        Task<BaseResponse<StoryResponse>> PublishStory(string storyID, string userID);
        Task<BaseResponse<StoryListResponse>> GetStories();
        Task<BaseResponse<StoryListResponse>> GetDrafts(string userID);
    }
}