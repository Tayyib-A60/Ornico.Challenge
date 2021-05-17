import { StoryListResponse, ToggleVoteResponse, DeleteStoryResponse, StoryToUpdate } from './../../shared/models/ApiResponse';
import { Story } from '../story.model';

/* NgRx */
import { Action } from '@ngrx/store';
import { StoryResponse } from 'src/app/shared/models/ApiResponse';

export enum StoryActionTypes {
  CreateStory = '[Story] Create Story',
  CreateStorySuccess = '[Story] Create Story Success',
  CreateStoryFailure = '[Story] Create Story Failure',
  UpdateStory = '[Story] Update Story',
  UpdateStorySuccess = '[Story] Update Story Success',
  UpdateStoryFailure = '[Story] Update Story Failure',
  DeleteStory = '[Story] Delete Story',
  DeleteStorySuccess = '[Story] Delete Story Success',
  DeleteStoryFailure = '[Story] Delete Story Failure',
  PublishStory = '[Story] Publish Story',
  PublishStorySuccess = '[Story] Publish Story Success',
  PublishStoryFailure = '[Story] Publish Story Failure',
  GetStory = '[Story] Get Story',
  GetStorySuccess = '[Story] Get Story Success',
  GetStoryFailure = '[Story] Get Story Failure',
  GetDrafts = '[Story] Get Drafts',
  GetDraftsSuccess = '[Story] Get Drafts Success',
  GetDraftsFailure = '[Story] Get Drafts Failure',
  GetStories = '[Story] Get Stories',
  GetStoriesSuccess = '[Story] Get Stories Success',
  GetStoriesFailure = '[Story] Get Stories Failure',
  ToggleVoteStory = '[Story] Toggle Vote Story',
  ToggleVoteStorySuccess = '[Story] Toggle Vote Story Success',
  ToggleVoteStoryFailure = '[Story] Toggle Vote Story Failure',
}

// Action Creators

export class CreateStory implements Action {
  readonly type = StoryActionTypes.CreateStory;

  constructor(public payload: Story) {
      this.type = StoryActionTypes.CreateStory;
  }
}

export class CreateStorySuccess implements Action {
  readonly type = StoryActionTypes.CreateStorySuccess;

  constructor(public payload: StoryResponse) {
      this.type = StoryActionTypes.CreateStorySuccess
  }
}

export class CreateStoryFailure implements Action {
  readonly type = StoryActionTypes.CreateStoryFailure;

  constructor(public payload: string) {
      this.type = StoryActionTypes.CreateStoryFailure
  }
}

export class UpdateStory implements Action {
  readonly type = StoryActionTypes.UpdateStory;

  constructor(public payload: StoryToUpdate) {
      this.type = StoryActionTypes.UpdateStory;
  }
}

export class UpdateStorySuccess implements Action {
  readonly type = StoryActionTypes.UpdateStorySuccess;

  constructor(public payload: StoryResponse) {
      this.type = StoryActionTypes.UpdateStorySuccess
  }
}

export class UpdateStoryFailure implements Action {
  readonly type = StoryActionTypes.UpdateStoryFailure;

  constructor(public payload: string) {
      this.type = StoryActionTypes.UpdateStoryFailure
  }
}
export class DeleteStory implements Action {
  readonly type = StoryActionTypes.DeleteStory;

  constructor(public payload: string) {
      this.type = StoryActionTypes.DeleteStory;
  }
}

export class DeleteStorySuccess implements Action {
  readonly type = StoryActionTypes.DeleteStorySuccess;

  constructor(public payload: DeleteStoryResponse) {
      this.type = StoryActionTypes.DeleteStorySuccess
  }
}

export class DeleteStoryFailure implements Action {
  readonly type = StoryActionTypes.DeleteStoryFailure;

  constructor(public payload: string) {
      this.type = StoryActionTypes.DeleteStoryFailure
  }
}

export class PublishStory implements Action {
  readonly type = StoryActionTypes.PublishStory;

  constructor(public payload: string) {
      this.type = StoryActionTypes.PublishStory
  }
}

export class PublishStorySuccess implements Action {
  readonly type = StoryActionTypes.PublishStorySuccess;

  constructor(public payload: StoryResponse) {
      this.type = StoryActionTypes.PublishStorySuccess
  }
}

export class PublishStoryFailure implements Action {
  readonly type = StoryActionTypes.PublishStoryFailure;

  constructor(public payload: any) {
      this.type = StoryActionTypes.PublishStoryFailure
  }
}

export class GetStory implements Action {
  readonly type = StoryActionTypes.GetStory;

  constructor(public payload: any) {
      this.type = StoryActionTypes.GetStory
  }
}

export class GetStorySuccess implements Action {
  readonly type = StoryActionTypes.GetStorySuccess;

  constructor(public payload: any) {
      this.type = StoryActionTypes.GetStorySuccess
  }
}

export class GetStoryFailure implements Action {
  readonly type = StoryActionTypes.GetStoryFailure;

  constructor(public payload: any) {
      this.type = StoryActionTypes.GetStoryFailure
  }
}

export class GetStories implements Action {
  readonly type = StoryActionTypes.GetStories;

  constructor() {
      this.type = StoryActionTypes.GetStories
  }
}

export class GetStoriesSuccess implements Action {
  readonly type = StoryActionTypes.GetStoriesSuccess;

  constructor(public payload: StoryListResponse) {
      this.type = StoryActionTypes.GetStoriesSuccess
  }
}

export class GetStoriesFailure implements Action {
  readonly type = StoryActionTypes.GetStoriesFailure;

  constructor(public payload: any) {
      this.type = StoryActionTypes.GetStoriesFailure
  }
}

export class GetDrafts implements Action {
  readonly type = StoryActionTypes.GetDrafts;

  constructor() {
      this.type = StoryActionTypes.GetDrafts
  }
}

export class GetDraftsSuccess implements Action {
  readonly type = StoryActionTypes.GetDraftsSuccess;

  constructor(public payload: StoryListResponse) {
      this.type = StoryActionTypes.GetDraftsSuccess
  }
}

export class GetDraftsFailure implements Action {
  readonly type = StoryActionTypes.GetDraftsFailure;

  constructor(public payload: any) {
      this.type = StoryActionTypes.GetDraftsFailure
  }
}

export class ToggleVoteStory implements Action {
  readonly type = StoryActionTypes.ToggleVoteStory;

  constructor(public payload: string) {
      this.type = StoryActionTypes.ToggleVoteStory
  }
}

export class ToggleVoteStorySuccess implements Action {
  readonly type = StoryActionTypes.ToggleVoteStorySuccess;

  constructor(public payload: ToggleVoteResponse) {
      this.type = StoryActionTypes.ToggleVoteStorySuccess
  }
}

export class ToggleVoteStoryFailure implements Action {
  readonly type = StoryActionTypes.ToggleVoteStoryFailure;

  constructor(public payload: any) {
      this.type = StoryActionTypes.ToggleVoteStoryFailure
  }
}

export type StoryActions = CreateStory
    | CreateStorySuccess
    | CreateStoryFailure
    | UpdateStory
    | UpdateStorySuccess
    | UpdateStoryFailure
    | DeleteStory
    | DeleteStorySuccess
    | DeleteStoryFailure
    | PublishStory
    | PublishStorySuccess
    | PublishStoryFailure
    | GetStory
    | GetStorySuccess
    | GetStoryFailure
    | GetStories
    | GetStoriesSuccess
    | GetStoriesFailure
    | GetDrafts
    | GetDraftsSuccess
    | GetDraftsFailure
    | ToggleVoteStory
    | ToggleVoteStorySuccess
    | ToggleVoteStoryFailure;
