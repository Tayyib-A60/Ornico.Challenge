import { createFeatureSelector, createSelector } from '@ngrx/store';
import * as fromRoot from '../../state/app.state';
import * as fromUser from './user.reducer';

export interface State extends fromRoot.AppState {
    user: fromUser.UserState;
}

// Selector functions
const getStoryFeatureState = createFeatureSelector<fromUser.UserState>('user');

export const getCurrentUser = createSelector(
    getStoryFeatureState,
    state => state.currentUser
);

// export const getAdminusers = createSelector(
//     getStoryFeatureState,
//     state => state.adminUsers
// );
