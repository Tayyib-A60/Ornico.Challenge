import { StoryListResponse, StoryResponse } from './../../shared/models/ApiResponse';
import { Story } from '../story.model';
import { StoryActionTypes, StoryActions, DeleteStorySuccess, DeleteStoryFailure } from './story.actions';

export interface StoryState {
  storyList: StoryListResponse;
  draftList: StoryListResponse;
  story: StoryResponse;
  error: string;
  response: any;
}

const initialState: StoryState = {
  storyList: new StoryListResponse(),
  draftList: new StoryListResponse(),
  story: null,
  error: '',
  response: null
};

export function reducer(state = initialState, action: StoryActions): StoryState {

  switch (action.type) {
    case StoryActionTypes.CreateStoryFailure:
      return {
        ...state,
        response: null,
        error: action.payload
      };
    case StoryActionTypes.UpdateStoryFailure:
      return {
        ...state,
        response: null,
        error: action.payload
      };
    case StoryActionTypes.DeleteStorySuccess:
      let remainingDrafts = state.draftList.stories.filter(d => d.id != action.payload.storyID);
      return {
        ...state,
        draftList: { ...state.draftList, count: state.draftList.count - 1, stories: remainingDrafts},
        error: ''
      };

    case StoryActionTypes.DeleteStoryFailure:
      return {
        ...state,
        response: null,
        error: action.payload
      };
    case StoryActionTypes.PublishStorySuccess:
      const updatedStories = state.draftList.stories.filter(
        story => story.id != action.payload.id
      );
      return {
        ...state,
        draftList: { ...state.draftList, stories: updatedStories },
        error: ''
      };
    case StoryActionTypes.PublishStoryFailure:
      return {
        ...state,
        error: action.payload
      };
    case StoryActionTypes.GetStoriesSuccess:
      return {
        ...state,
        storyList: action.payload,
        error: ''
      };
    case StoryActionTypes.GetStoriesFailure:
      return {
        ...state,
        // stories: [],
        error: action.payload
      };
    case StoryActionTypes.GetStorySuccess:
        return {
          ...state,
          story: action.payload,
          error: ''
        };
    case StoryActionTypes.GetStoriesFailure:
        return {
          ...state,
          story: null,
          error: action.payload
        };
    case StoryActionTypes.GetDraftsSuccess:
        return {
          ...state,
          draftList: action.payload,
          error: ''
        };
    case StoryActionTypes.GetStoriesFailure:
        return {
          ...state,
          error: action.payload
        };
    case StoryActionTypes.ToggleVoteStorySuccess:
      let affectedStory = state.storyList.stories.filter(
        story => story.id == action.payload.storyID
      )[0];

      affectedStory = {...affectedStory, votes: action.payload.votes};

      const unaffectedStories = state.storyList.stories.filter(
        story => story.id != action.payload.storyID
      );
      return {
        ...state,
        storyList: { ...state.storyList, stories: [...unaffectedStories, affectedStory] },
        error: ''
      };
    case StoryActionTypes.ToggleVoteStoryFailure:
        return {
          ...state,
          error: action.payload
        };

    default:
      return { ...state }
  }
}
