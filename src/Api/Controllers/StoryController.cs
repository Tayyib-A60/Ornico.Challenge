using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Service.BL.Interfaces;
using System.Threading.Tasks;
using Models.DTOs;
using Models.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;

namespace Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class StoryController : ControllerBase
    {
        private readonly IStoryService _storyService;
        public StoryController(IStoryService storyService)
        {
            _storyService = storyService;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(BaseResponse<StoryResponse>), 200)]
        public async Task<IActionResult> CreateStory([FromBody] CreateStoryDTO createStoryDTO)
        {
            var response = await _storyService.CreateStory(createStoryDTO, GetUserId());
            return Ok(response);
        }

        [HttpPost("update")]
        [ProducesResponseType(typeof(BaseResponse<StoryResponse>), 200)]
        public async Task<IActionResult> UpdateStory([FromBody] UpdateStoryDTO updateStoryDTO)
        {
            var response = await _storyService.UpdateStory(updateStoryDTO, GetUserId());
            return Ok(response);
        }

        [HttpPost("delete/{storyID}")]
        [ProducesResponseType(typeof(BaseResponse<StoryResponse>), 200)]
        public async Task<IActionResult> DeleteStory(string storyID)
        {
            var response = await _storyService.DeleteStory(storyID, GetUserId());
            return Ok(response);
        }

        [HttpPost("publish/{storyID}")]
        [ProducesResponseType(typeof(BaseResponse<string>), 200)]
        public async Task<IActionResult> PublishStory(string storyID)
        {
            var response = await _storyService.PublishStory(storyID, GetUserId());
            return Ok(response);
        }
        
        [HttpPost("toggle-vote/{storyID}")]
        [ProducesResponseType(typeof(BaseResponse<ToggleVoteResponse>), 200)]
        public async Task<IActionResult> ToggleVote(string storyID)
        {
            var response = await _storyService.ToggleVote(storyID, GetUserId());
            return Ok(response);
        }
        
        [HttpPost("get-stories")]
        [ProducesResponseType(typeof(BaseResponse<StoryListResponse>), 200)]
        public async Task<IActionResult> GetStories()
        {
            //var response = new BaseResponse<StoryListResponse>();

            //var items = new Dictionary<string, string>();

            //if(query.Count < 1)
            //{
            //    response.Message = "Query can not be empty";
            //    return Ok(response);
            //}

            //foreach (var kvp in query)
            //{
            //    var str = kvp.Value.ToString();

            //    items.Add(kvp.Key, str);
            //}

            var response = await _storyService.GetStories();
            return Ok(response);
        }


        [HttpPost("get-drafts")]
        [ProducesResponseType(typeof(BaseResponse<StoryListResponse>), 200)]
        public async Task<IActionResult> GetDrafts()
        {
            var response = await _storyService.GetDrafts(GetUserId());
            return Ok(response);
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

    }
}
