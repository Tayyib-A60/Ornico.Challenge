using Core.Helpers;
using Models.Entities;
using Models.ViewModels;
using Repository.Commands.Interfaces;
using Repository.Queries.Interfaces;
using Service.BL.Implementations;
using Service.BL.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using AutoMapper;
using Core.Constants;
using System;
using Core.Enums;
using Models.DTOs;

namespace Tests
{
    [Trait("Category", "StoryService")]
    public class StoryServiceShould
    {
        private readonly IStoryService _storyService;
        private readonly Mock<IDapperCommandRepository<Story>> _dCRStory = new Mock<IDapperCommandRepository<Story>>();
        private readonly Mock<IDapperQueryRepository<Story>> _dQRStory = new Mock<IDapperQueryRepository<Story>>();
        private readonly Mock<IDapperCommandRepository<Vote>> _dCRVote = new Mock<IDapperCommandRepository<Vote>>();
        private readonly Mock<IDapperQueryRepository<Vote>> _dQRVote = new Mock<IDapperQueryRepository<Vote>>();
        private readonly Mock<IDapperQueryRepository<User>> _dQRUser = new Mock<IDapperQueryRepository<User>>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly IConfigurationRoot Configuration;
        private readonly Guid _userID;
        private readonly Guid _storyID;
        public StoryServiceShould()
        {
            var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();

            _storyService = new StoryService(_dCRStory.Object, _dQRStory.Object, _dCRVote.Object, _dQRVote.Object, _dQRUser.Object);

            _userID = Guid.NewGuid();
            _storyID = Guid.NewGuid();
        }

        [Fact]
        public async Task Should_Get_Stories()
        {
            // Arrange

            var stories = new List<Story>
            {
                new Story
                {
                    ID = Guid.NewGuid(),
                    Votes = 0,
                    Type = StoryType.Story
                }
            };

            var story = stories.FirstOrDefault();

            var storiesMappingResponse = new List<StoryResponse>
            {
                new StoryResponse
                {
                    ID = story.ID,
                    Votes = 0,
                    Type = StoryType.Story
                }
            };

            var queryResult = stories.AsQueryable();

            // Act
            _dQRStory.Setup(d => d.GetByAsync(new Dictionary<string, string>() { { "Type", "2" } })).ReturnsAsync(queryResult);
            _mapper.Setup(m => m.Map<List<StoryResponse>>(queryResult.ToList())).Returns(storiesMappingResponse);

            var result = await _storyService.GetStories();

            // Assert
            Assert.True(result.Status);
            Assert.NotNull(result.Data);
            Assert.True(result.Data.Stories.Count.Equals(1));
            Assert.True(result.Data.Stories.FirstOrDefault().Type.Equals(StoryType.Story));
        }

        [Fact]
        public async Task Should_Get_Drafts()
        {
            // Arrange
            
            var stories = new List<Story>
            {
                new Story
                {
                    ID = Guid.NewGuid(),
                    Votes = 0,
                    Type = StoryType.Draft,
                    UserID = _userID
                }
            };

            var storiesMappingResponse = new List<StoryResponse>
            {
                new StoryResponse
                {
                    ID = Guid.NewGuid(),
                    Votes = 0,
                    Type = StoryType.Draft
                }
            };

            var story = stories.FirstOrDefault();

            var queryResult = stories.AsQueryable();

            // Act
            _dQRStory.Setup(d => d.GetByAsync(new Dictionary<string, string>() { { "Type", "1" }, { "UserID", _userID.ToString() } })).ReturnsAsync(queryResult);
            _mapper.Setup(m => m.Map<List<StoryResponse>>(stories)).Returns(storiesMappingResponse);

            var result = await _storyService.GetDrafts(_userID.ToString());

            // Assert
            Assert.True(result.Status);
            Assert.NotNull(result.Data);
            Assert.True(result.Data.Stories.Count.Equals(1));
            Assert.True(result.Data.Stories.FirstOrDefault().Type.Equals(StoryType.Draft));
        }

        [Fact]
        public async Task Should_Create_Story()
        {
            // Arrange
            

            var userList = new List<User>
            {
                new User
                {
                    ID = _userID,
                    Role = Role.Author
                }
            };

            var story = new Story
            {
                Type = StoryType.Draft
            };

            _dQRUser.Setup(d => d.GetByAsync(new Dictionary<string, string>() { { "ID", _userID.ToString() } })).ReturnsAsync(userList.AsQueryable());

            _dCRStory.Setup(d => d.AddAsync(story));

            // Act

            var result = await _storyService.CreateStory(new CreateStoryDTO(), _userID.ToString());

            // Assert
            Assert.True(result.Status);
            Assert.NotNull(result.Data);
            Assert.True(result.Data.Type.Equals(StoryType.Draft));
        }

        [Fact]
        public async Task Should_Delete_Story()
        {
            // Arrange
            
            var storyID = Guid.NewGuid();

            var userList = new List<User>
            {
                    new User
                {
                    ID = _userID,
                    Role = Role.Author
                }
            };

            var stories = new List<Story>
            {
                new Story
                {
                    ID = storyID,
                    Votes = 0,
                    Type = StoryType.Draft,
                    UserID = _userID
                }
            };

            var story = stories.FirstOrDefault();

            _dQRUser.Setup(d => d.GetByAsync(new Dictionary<string, string>() { { "ID", _userID.ToString() } })).ReturnsAsync(userList.AsQueryable());

            _dQRStory.Setup(d => d.GetByAsync(new Dictionary<string, string>() { { "ID", storyID.ToString() }, { "UserID", _userID.ToString() } })).ReturnsAsync(stories.AsQueryable());

            // Act

            var result = await _storyService.DeleteStory(storyID.ToString(), _userID.ToString());

            // Assert
            Assert.True(result.Data.StoryID.ToString().Equals(storyID.ToString()));
            Assert.True(result.Status);
        }

        [Fact]
        public async Task Should_Publish_Story()
        {
            // Arrange
            
            var storyID = Guid.NewGuid();

            var userList = new List<User>
            {
                    new User
                {
                    ID = _userID,
                    Role = Role.Author
                }
            };

            var stories = new List<Story>
            {
                new Story
                {
                    ID = storyID,
                    Votes = 0,
                    Type = StoryType.Draft,
                    UserID = _userID
                }
            };

            var story = stories.FirstOrDefault();

            _dQRUser.Setup(d => d.GetByAsync(new Dictionary<string, string>() { { "ID", _userID.ToString() } })).ReturnsAsync(userList.AsQueryable());

            _dQRStory.Setup(d => d.GetByAsync(new Dictionary<string, string>() { { "ID", storyID.ToString() }, { "UserID", _userID.ToString() } })).ReturnsAsync(stories.AsQueryable());

            story.Type = StoryType.Story;
            _dCRStory.Setup(d => d.UpdateAsync(story));

            _mapper.Setup(m => m.Map<StoryResponse>(story)).Returns(new StoryResponse { ID = storyID, Type = story.Type });

            // Act

            var result = await _storyService.PublishStory(storyID.ToString(), _userID.ToString());

            // Assert
            Assert.True(result.Status);
            Assert.True(result.Data.ID.ToString().Equals(storyID.ToString()));
            Assert.True(result.Data.Type.Equals(StoryType.Story));

        }
        
        [Fact]
        public async Task Should_Update_Story()
        {
            // Arrange
            
            var storyID = Guid.NewGuid();

            var userList = new List<User>
            {
                    new User
                {
                    ID = _userID,
                    Role = Role.Author
                }
            };

            var stories = new List<Story>
            {
                new Story
                {
                    ID = storyID,
                    Votes = 0,
                    Type = StoryType.Draft,
                    UserID = _userID
                }
            };

            var story = stories.FirstOrDefault();

            var updateStoryDTO = new UpdateStoryDTO
            {
                Content = "New Content",
                StoryID = storyID.ToString(),
                Title = "New Title"
            };

            _dQRUser.Setup(d => d.GetByAsync(new Dictionary<string, string>() { { "ID", _userID.ToString() } })).ReturnsAsync(userList.AsQueryable());

            _dQRStory.Setup(d => d.GetByAsync(new Dictionary<string, string>() { { "ID", storyID.ToString() }, { "UserID", _userID.ToString() } })).ReturnsAsync(stories.AsQueryable());

            story.Title = updateStoryDTO.Title;
            story.Content = updateStoryDTO.Content;

            _dCRStory.Setup(d => d.UpdateAsync(story));

            _mapper.Setup(m => m.Map<StoryResponse>(story)).Returns(new StoryResponse { ID = storyID, Type = story.Type, Title = updateStoryDTO.Title, Content = updateStoryDTO.Content });

            // Act

            var result = await _storyService.UpdateStory(updateStoryDTO, _userID.ToString());

            // Assert
            Assert.True(result.Status);
            Assert.True(result.Data.ID.ToString().Equals(storyID.ToString()));
            Assert.True(result.Data.Type.Equals(StoryType.Draft));
            Assert.True(result.Data.Title.Equals(updateStoryDTO.Title));

        }
    }
}
