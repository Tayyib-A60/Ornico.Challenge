import { StoryState } from '../story/state/story.reducer';
import { UserState } from '../user/state/user.reducer';

// Representation of the entire app state
export interface AppState {
  story: StoryState,
  user: UserState
}
