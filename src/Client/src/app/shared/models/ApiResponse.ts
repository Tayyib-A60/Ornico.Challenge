import { Role } from './../../user/user.model';
import { Token } from "@angular/compiler/src/ml_parser/lexer";

export interface ApiBaseResponse<T> {
  status: boolean,
  message: string,
  data: T
}

export interface UserLoginResponse {
  token: string,
  email: string,
  role: Role,
  isVerified: boolean
}

export interface StoryResponse {
  title: string,
  content: string,
  type: StoryType,
  votes: Number,
  author: string,
  id: string,
  creationDate: Date,
}

export interface StoryToUpdate {
  title: string,
  content: string,
  storyID: string,
}

export interface EmailVerificationDTO {
    code: string
}

export class StoryListResponse {
  currentPage: number;
  count: number;
  stories: Array<StoryResponse>;
}

export interface ToggleVoteResponse {
  votes: number,
  storyID: string
}

export interface DeleteStoryResponse {
  storyID: string
}

export enum StoryType {
  Draft,
  Story
}

export interface EmailVerificationResponse {
  email: string
}
