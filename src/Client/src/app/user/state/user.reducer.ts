import { UserLoginResponse } from './../../shared/models/ApiResponse';
import { UserActionTypes, UserActions } from './user.actions';


export interface UserState {
  currentUser: UserLoginResponse;
  error: string;
  response: any;
}

const initialState: UserState = {
  currentUser: null,
  error: '',
  response: null,
};

export function reducer(state = initialState, action: UserActions): UserState {

  switch (action.type) {
    case UserActionTypes.CreateUserSuccess:
      return {
        ...state,
        currentUser: action.payload,
        error: ''
      };
    case UserActionTypes.CreateUserFailure:
      return {
        ...state,
        response: null,
        error: action.payload
      };
    case UserActionTypes.LoginUserSuccess:
      sessionStorage.setItem('currentUser', JSON.stringify(action.payload));
      return {
        ...state,
        currentUser: action.payload,
        error: ''
      };
    case UserActionTypes.LoginUserFailure:
      return {
        ...state,
        currentUser: null,
        error: action.payload
      };
    case UserActionTypes.LogoutUser:
      sessionStorage.removeItem('currentUser');
      return {
        ...state,
        currentUser: null,
        error: ''
      };
    case UserActionTypes.VerifyUserEmailSuccess:
      let user = JSON.parse(sessionStorage.getItem('currentUser'));
      sessionStorage.removeItem('currentUser');
      user.isVerified = true;
      sessionStorage.setItem('currentUser', JSON.stringify(user));
      return {
        ...state,
        currentUser: { ...state.currentUser, isVerified: true },
        error: ''
      };
    case UserActionTypes.VerifyUserEmailFailure:
      return {
        ...state,
        error: action.payload
      };
    default:
      return {
        ...state
      };
  }
}
