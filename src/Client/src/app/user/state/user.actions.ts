import { EmailVerificationDTO, EmailVerificationResponse } from './../../shared/models/ApiResponse';
import { User, UserToLogin } from '../user.model';

/* NgRx */
import { Action } from '@ngrx/store';

export enum UserActionTypes {
  CreateUser = '[User] Create User',
  CreateUserSuccess = '[User] Create User Success',
  CreateUserFailure = '[User] Create User Failure',
  LoginUser = '[User] Login User',
  LoginUserSuccess = '[UserSuccess] Login User Success',
  LoginUserFailure = '[User] Login User Failure',
  LogoutUser = '[User] Logout User',
  VerifyUserEmail = '[User] Verify User Email',
  VerifyUserEmailSuccess = '[User] Verify User Email Success',
  VerifyUserEmailFailure = '[User] Verify User Email Failure',
}

// Action Creators

export class CreateUser implements Action {
  readonly type = UserActionTypes.CreateUser;

  constructor(public payload: User) {
      this.type = UserActionTypes.CreateUser;
  }
}

export class CreateUserSuccess implements Action {
  readonly type = UserActionTypes.CreateUserSuccess;

  constructor(public payload: any) {
      this.type = UserActionTypes.CreateUserSuccess
  }
}

export class CreateUserFailure implements Action {
  readonly type = UserActionTypes.CreateUserFailure;

  constructor(public payload: any) {
      this.type = UserActionTypes.CreateUserFailure
  }
}

export class LoginUser implements Action {
  readonly type = UserActionTypes.LoginUser;

  constructor(public payload: UserToLogin) {
      this.type = UserActionTypes.LoginUser
  }
}
export class LoginUserSuccess implements Action {
  readonly type = UserActionTypes.LoginUserSuccess;

  constructor(public payload: any) {
      this.type = UserActionTypes.LoginUserSuccess
  }
}
export class LoginUserFailure implements Action {
  readonly type = UserActionTypes.LoginUserFailure;

  constructor(public payload: any) {
      this.type = UserActionTypes.LoginUserFailure
  }
}

export class LogoutUser implements Action {
  readonly type = UserActionTypes.LogoutUser;

  constructor() {
      this.type = UserActionTypes.LogoutUser
  }
}

export class VerifyUserEmail implements Action {
  readonly type = UserActionTypes.VerifyUserEmail;

  constructor(public payload: EmailVerificationDTO) {
      this.type = UserActionTypes.VerifyUserEmail
  }
}
export class VerifyUserEmailSuccess implements Action {
  readonly type = UserActionTypes.VerifyUserEmailSuccess;

  constructor(public payload: EmailVerificationResponse) {
      this.type = UserActionTypes.VerifyUserEmailSuccess
  }
}
export class VerifyUserEmailFailure implements Action {
  readonly type = UserActionTypes.VerifyUserEmailFailure;

  constructor(public payload: string) {
      this.type = UserActionTypes.VerifyUserEmailFailure
  }
}

export type UserActions = CreateUser
    | CreateUserSuccess
    | CreateUserFailure
    | LoginUser
    | LoginUserSuccess
    | LoginUserFailure
    | LogoutUser
    | VerifyUserEmail
    | VerifyUserEmailSuccess
    | VerifyUserEmailFailure;
