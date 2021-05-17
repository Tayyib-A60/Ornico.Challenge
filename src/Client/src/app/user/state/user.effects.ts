import { Contants } from './../../shared/models/Contants';
import { ApiBaseResponse, UserLoginResponse, EmailVerificationResponse, EmailVerificationDTO } from './../../shared/models/ApiResponse';
import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { mergeMap, map, catchError } from 'rxjs/operators';

import { UserService } from '../user.service';
import { User, UserToLogin, Role } from '../user.model';

import { Action } from '@ngrx/store';
import { Actions, Effect, ofType } from '@ngrx/effects';
import * as userActions from './user.actions';
import { Router } from '@angular/router';
import { NotifierService } from 'angular-notifier';

@Injectable({
    providedIn: 'root'
})
export class UserEffects {
    constructor(private userService: UserService,
                private actions$: Actions,
                private notification: NotifierService,
                private router: Router) { }


    @Effect()
    createUser$: Observable<Action> = this.actions$.pipe(
        ofType(userActions.UserActionTypes.CreateUser),
        map((action: userActions.CreateUser) => action.payload),
        mergeMap((user: User) =>
          this.userService.createUser(user).pipe(
            map((baseResponse: ApiBaseResponse<UserLoginResponse>) => {

                if(baseResponse.status)
                {
                    this.notification.notify(Contants.Success,'User Created');
                    this.router.navigate(['user/login']);
                    return new userActions.CreateUserSuccess(baseResponse.data);
                }
                else
                {
                  this.notification.notify(Contants.Error, baseResponse.message);
                  return new userActions.CreateUserFailure(baseResponse.message);
                }
              }),
              catchError(err => {
                this.notification.notify(Contants.Error,`err`);
                return of(new userActions.CreateUserFailure(err))
            })
          )
        )
      );

    @Effect()
    loginUser$: Observable<Action> = this.actions$.pipe(
        ofType(userActions.UserActionTypes.LoginUser),
        map((action: userActions.LoginUser) => action.payload),
        mergeMap((user: UserToLogin) =>
          this.userService.loginUser(user).pipe(
            map((baseResponse: ApiBaseResponse<UserLoginResponse>) => {

              if(baseResponse.status)
              {
                  this.notification.notify(Contants.Success,'Login Successful');
                  if(baseResponse.data.role === Role.Author)
                  {
                      this.router.navigate(['/user/create-story']);
                  }
                  else
                  {
                      this.router.navigate(['/user/stories']);
                  }

                  return new userActions.LoginUserSuccess(baseResponse.data);
              }
              else
              {
                  this.notification.notify(Contants.Error, baseResponse.message);
                  return new userActions.LoginUserFailure(baseResponse.message);
              }
            }),
              catchError(err => {
                this.notification.notify(Contants.Error,`${err}`);
                return of(new userActions.LoginUserFailure(err))
            })
          )
        )
      );


    @Effect()
    verifyUserEmail$: Observable<Action> = this.actions$.pipe(
        ofType(userActions.UserActionTypes.VerifyUserEmail),
        map((action: userActions.VerifyUserEmail) => action.payload),
        mergeMap((emailVerDTO: EmailVerificationDTO) =>
          this.userService.verifyUserEmail(emailVerDTO).pipe(
            map((baseResponse: ApiBaseResponse<EmailVerificationResponse>) => {

              if(baseResponse.status)
              {
                  this.notification.notify(Contants.Success,'Email verified');
                  this.router.navigate(['/user/stories']);

                  return new userActions.VerifyUserEmailSuccess(baseResponse.data);
              }
              else
              {
                  this.notification.notify(Contants.Error, baseResponse.message);
                  return new userActions.VerifyUserEmailFailure(baseResponse.message);
              }
            }),
              catchError(err => {
                this.notification.notify(Contants.Error,`${err}`);
                return of(new userActions.VerifyUserEmailFailure(err))
            })
          )
        )
      );
}
