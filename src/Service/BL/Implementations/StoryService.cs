using Core.Constants;
using Core.Enums;
using System;
using Models.Entities;
using Models.ViewModels;
using Repository.Commands.Interfaces;
using Repository.Queries.Interfaces;
using Service.BL.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models.DTOs;
using AutoMapper;
using Services.Helpers;

namespace Service.BL.Implementations
{
    public class StoryService : IStoryService
    {
        private readonly IDapperCommandRepository<Story> _dCRStory;
        private readonly IDapperQueryRepository<Story> _dQRStory;
        private readonly IDapperCommandRepository<Vote> _dCRVote;
        private readonly IDapperQueryRepository<Vote> _dQRVote;
        private readonly IDapperQueryRepository<User> _dQRUser;
        private readonly IMapper _mapper;
        public StoryService(IDapperCommandRepository<Story> dCRStory, IDapperQueryRepository<Story> dQRStory, IDapperCommandRepository<Vote> dCRVote, IDapperQueryRepository<Vote> dQRVote, IDapperQueryRepository<User> dQRUser)
        {
            _dCRStory = dCRStory;
            _dQRStory = dQRStory;
            _dCRVote = dCRVote;
            _dQRVote = dQRVote;
            _dQRUser = dQRUser;

            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new AutomapperProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        public async Task<BaseResponse<StoryListResponse>> GetStories()
        {
            var response = new BaseResponse<StoryListResponse>();

            try
            {
                var query = new Dictionary<string, string>() { { "Type", "2" } };

                var storyQueryResult = await _dQRStory.GetByAsync(query);

                var stories = storyQueryResult.ToList();

                response.Status = true;
                var storiesResponseList = _mapper.Map<List<StoryResponse>>(stories);
                response.Data = new StoryListResponse
                {
                    Stories = storiesResponseList,
                    Count = storiesResponseList.Count,
                };
                response.Message = ResponseMessages.OperationSuccessful;

                return response;
            }
            catch(Exception ex)
            {
                return response;
            }
        }
        public async Task<BaseResponse<StoryListResponse>> GetDrafts(string userID)
        {
            var response = new BaseResponse<StoryListResponse>();

            try
            {
                var query = new Dictionary<string, string>() { { "Type", "1" }, { "UserID", userID } };
                var storyQueryResult = await _dQRStory.GetByAsync(query);

                var stories = storyQueryResult.ToList();

                response.Status = true;
                response.Data = new StoryListResponse
                {
                    Stories = _mapper.Map<List<StoryResponse>>(stories),
                    Count = stories.Count,
                };
                response.Message = ResponseMessages.OperationSuccessful;

                return response;
            }
            catch(Exception ex)
            {
                return response;
            }
        }

        public async Task<BaseResponse<StoryResponse>> PublishStory(string storyID, string userID)
        {
            var response = new BaseResponse<StoryResponse>();
            try
            {
                //var user = await GetUser(userID);
                var query = new Dictionary<string, string>() { { "ID", userID } };

                var userQueryResult = await _dQRUser.GetByAsync(query);

                var user = userQueryResult.FirstOrDefault();

                if (user == null)
                {
                    response.Message = "User not found";
                    return response;
                }

                var storyQuery = new Dictionary<string, string>() { { "ID", storyID }, { "UserID", userID } };

                var storyQueryResult = await _dQRStory.GetByAsync(storyQuery);

                var story = storyQueryResult.FirstOrDefault();

                if(story != null)
                {
                    story.Type = StoryType.Story;
                    await _dCRStory.UpdateAsync(story);

                    response.Status = true;
                    response.Message = ResponseMessages.OperationSuccessful;
                    response.Data = _mapper.Map<StoryResponse>(story);
                }
                else
                {
                    response.Message = "Story not found";
                }
                return response;
            }
            catch(Exception ex)
            {
                return response;
            }
        }
        public async Task<BaseResponse<StoryResponse>> CreateStory(CreateStoryDTO createStoryDTO, string userID)
        {
            var response = new BaseResponse<StoryResponse>();

            try
            {
                //var user = await GetUser(userID);

                var query = new Dictionary<string, string>() { { "ID", userID } };

                var userQueryResult = await _dQRUser.GetByAsync(query);

                var user = userQueryResult.FirstOrDefault();

                if (user == null)
                {
                    response.Message = "User not found";
                    return response;
                } 

                if(user.Role == Role.Reader)
                {
                    response.Message = "Action not allowed for user";
                    return response;
                }

                var story = new Story
                {
                    Author = user.FullName,
                    CreationDate = DateTime.Now,
                    ID = Guid.NewGuid(),
                    LastModifiedDate = DateTime.Now,
                    Content = createStoryDTO.Content,
                    Title = createStoryDTO.Title,
                    Type = StoryType.Draft,
                    UserID = user.ID,
                    Votes = 0
                };

                await _dCRStory.AddAsync(story);

                response.Status = true;
                response.Message = ResponseMessages.OperationSuccessful;
                response.Data = _mapper.Map<StoryResponse>(story);
                return response;
            }
            catch(Exception ex)
            {
                return response;
            }
        }

        public async Task<BaseResponse<ToggleVoteResponse>> ToggleVote(string storyID, string userID)
        {
            var response = new BaseResponse<ToggleVoteResponse>();
            try
            {
                var user = await GetUser(userID);

                if (user == null)
                {
                    response.Message = "User not found";
                    return response;
                }

                var storyQuery = new Dictionary<string, string>() { { "ID", storyID } }; // , { "UserID", toggleVoteDTO.UserID }

                var storyQueryResult = await _dQRStory.GetByAsync(storyQuery);

                var story = storyQueryResult.FirstOrDefault();

                if(story != null)
                {
                    var query = new Dictionary<string, string>() { { "StoryID", storyID }, { "UserID", userID } };

                    var voteQueryResult = await _dQRVote.GetByAsync(query);
                    var vote = voteQueryResult.FirstOrDefault();

                    if (vote != null)
                    {
                        vote.Voted = !vote.Voted;

                        await _dCRVote.UpdateAsync(vote);
                    }
                    else
                    {
                        vote = new Vote
                        {
                            CreationDate = DateTime.Now,
                            LastModifiedDate = DateTime.Now,
                            ID = Guid.NewGuid(),
                            StoryID = storyID,
                            UserID = userID,
                            Voted = true
                        };

                        await _dCRVote.AddAsync(vote);
                    }

                    if(vote.Voted)
                    {
                        story.Votes += 1;
                    } 
                    else
                    {
                        story.Votes -= 1;
                    }

                    await _dCRStory.UpdateAsync(story);

                    response.Status = true;
                    response.Message = ResponseMessages.OperationSuccessful;
                    response.Data = new ToggleVoteResponse
                    {
                        Votes = story.Votes,
                        StoryID = story.ID
                    };
                }
                else
                {
                    response.Message = "Story not found";
                }

                return response;
            }
            catch(Exception ex)
            {
                return response;
            }
        }

        private async Task<User> GetUser(string userID)
        {
            try
            {
                var query = new Dictionary<string, string>() { { "ID", userID } };

                var userQueryResult = await _dQRUser.GetByAsync(query);

                var user = userQueryResult.FirstOrDefault();
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<Story> GetStory(string storyID, string userID)
        {
            try
            {
                var query = new Dictionary<string, string>() { { "ID", storyID }, { "UserID", userID } };

                var storyQueryResult = await _dQRStory.GetByAsync(query);

                var story = storyQueryResult.FirstOrDefault();
                return story;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BaseResponse<StoryResponse>> UpdateStory(UpdateStoryDTO updateStoryDTO, string userID)
        {
            var response = new BaseResponse<StoryResponse>();

            try
            {
                var query = new Dictionary<string, string>() { { "ID", userID } };

                var userQueryResult = await _dQRUser.GetByAsync(query);

                var user = userQueryResult.FirstOrDefault();

                if (user == null)
                {
                    response.Message = "User not found";
                    return response;
                }

                if (user.Role == Role.Reader)
                {
                    response.Message = "Story not found";
                    return response;
                }

                var storyQuery = new Dictionary<string, string>() { { "ID", updateStoryDTO.StoryID }, { "UserID", userID } };

                var storyQueryResult = await _dQRStory.GetByAsync(storyQuery);

                var story = storyQueryResult.FirstOrDefault();
                //var story = await GetStory(updateStoryDTO.StoryID, userID);

                if(story != null)
                {
                    if (story.Type == StoryType.Story)
                    {
                        response.Message = "You can't update an already published story";
                        return response;
                    }

                    story.Content = updateStoryDTO.Content;
                    story.Title = updateStoryDTO.Title;
                    story.LastModifiedDate = DateTime.Now;

                    await _dCRStory.UpdateAsync(story);

                    response.Status = true;
                    response.Message = ResponseMessages.OperationSuccessful;
                    response.Data = _mapper.Map<StoryResponse>(story);
                    return response;
                }
                else
                {
                    response.Message = "Story not found";
                    return response;
                }

            }
            catch (Exception ex)
            {
                return response;
            }
        }

        public async Task<BaseResponse<DeleteStoryResponse>> DeleteStory(string storyID, string userID)
        {
            var response = new BaseResponse<DeleteStoryResponse>();

            try
            {
                var query = new Dictionary<string, string>() { { "ID", userID } };

                var userQueryResult = await _dQRUser.GetByAsync(query);

                var user = userQueryResult.FirstOrDefault();

                //var user = await GetUser(userID);

                if (user == null)
                {
                    response.Message = "User not found";
                    return response;
                }

                if (user.Role == Role.Reader)
                {
                    response.Message = "Action not allowed for user";
                    return response;
                }
                var storyQuery = new Dictionary<string, string>() { { "ID", storyID }, { "UserID", userID } };

                var storyQueryResult = await _dQRStory.GetByAsync(storyQuery);

                var story = storyQueryResult.FirstOrDefault();
                //var story = await GetStory(storyID, userID);

                if (story != null)
                {
                    if(story.Type == StoryType.Story)
                    {
                        response.Message = "You can't delete an already published story";
                        return response;
                    }

                    await _dCRStory.DeleteAsync(story.ID);
                    response.Status = true;
                    response.Message = ResponseMessages.OperationSuccessful;
                    response.Data = new DeleteStoryResponse
                    {
                        StoryID = storyID
                    };
                    return response;
                }
                else
                {
                    response.Message = "Story not found";
                    return response;
                }
            }
            catch(Exception ex)
            {
                return response;
            }
        }
    }
}
