import { Contants } from './../../shared/models/Contants';
import { StoryListResponse, ToggleVoteResponse, DeleteStoryResponse, StoryToUpdate } from './../../shared/models/ApiResponse';
import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { mergeMap, map, catchError } from 'rxjs/operators';

import { StoryService } from '../story.service';
import { Story } from '../story.model';

import { Action } from '@ngrx/store';
import { Actions, Effect, ofType } from '@ngrx/effects';
import * as storyActions from './story.actions';
import { Router } from '@angular/router';
import { NotifierService } from 'angular-notifier';
import { ApiBaseResponse, StoryResponse } from 'src/app/shared/models/ApiResponse';

@Injectable({
    providedIn: 'root'
})
export class StoryEffects {
    constructor(private storyService: StoryService,
                private actions$: Actions,
                private notifier: NotifierService,
                private router: Router) { }


    @Effect()
    createStory$: Observable<Action> = this.actions$.pipe(
        ofType(storyActions.StoryActionTypes.CreateStory),
        map((action: storyActions.CreateStory) => action.payload),
        mergeMap((story: Story) =>
          this.storyService.createStory(story).pipe(
            map((baseResponse: ApiBaseResponse<StoryResponse>) => {

              if(baseResponse.status)
              {
                this.notifier.notify(Contants.Success,'Story Created');
                this.router.navigate(['user/dashboard']);
                return new storyActions.CreateStorySuccess(baseResponse.data);
              }
              else
              {
                this.notifier.notify(Contants.Error, baseResponse.message);
                return new storyActions.CreateStoryFailure(baseResponse.message)
              }

            }),
            catchError(err => {
              this.notifier.notify(Contants.Error,`${err}`);
                return of(new storyActions.CreateStoryFailure(err))
            })
          )
        )
      );

    @Effect()
    updateStory$: Observable<Action> = this.actions$.pipe(
        ofType(storyActions.StoryActionTypes.UpdateStory),
        map((action: storyActions.UpdateStory) => action.payload),
        mergeMap((story: StoryToUpdate) =>
          this.storyService.updateStory(story).pipe(
            map((baseResponse: ApiBaseResponse<StoryResponse>) => {
              console.log(baseResponse);

              if(baseResponse.status)
              {
                this.notifier.notify(Contants.Success,'Story Updated');
                this.router.navigate(['user/dashboard']);
                return new storyActions.UpdateStorySuccess(baseResponse.data);
              }
              else
              {
                this.notifier.notify(Contants.Error, baseResponse.message);
                return new storyActions.UpdateStoryFailure(baseResponse.message)
              }

            }),
            catchError(err => {
              this.notifier.notify(Contants.Error,`${err}`);
                return of(new storyActions.UpdateStoryFailure(err))
            })
          )
        )
      );

    @Effect()
    deleteStory$: Observable<Action> = this.actions$.pipe(
        ofType(storyActions.StoryActionTypes.DeleteStory),
        map((action: storyActions.DeleteStory) => action.payload),
        mergeMap((storyID: string) =>
          this.storyService.deleteStory(storyID).pipe(
            map((baseResponse: ApiBaseResponse<DeleteStoryResponse>) => {

              if(baseResponse.status)
              {
                this.notifier.notify(Contants.Success,'Story Deleted');
                this.router.navigate(['user/dashboard']);
                return new storyActions.DeleteStorySuccess(baseResponse.data);
              }
              else
              {
                this.notifier.notify(Contants.Error, baseResponse.message);
                return new storyActions.DeleteStoryFailure(baseResponse.message)
              }

            }),
            catchError(err => {
              this.notifier.notify(Contants.Error,`${err}`);
                return of(new storyActions.DeleteStoryFailure(err))
            })
          )
        )
      );


    @Effect()
    publishStory$: Observable<Action> = this.actions$.pipe(
        ofType(storyActions.StoryActionTypes.PublishStory),
        map((action: storyActions.PublishStory) => (action.payload)),
        mergeMap((storyID: string) =>
          this.storyService.publishStory(storyID).pipe(
            map((baseResponse: ApiBaseResponse<StoryResponse>) => {

              if(baseResponse.status)
              {
                this.notifier.notify(Contants.Success,'Story Published successfully');
                this.router.navigate(['user/stories']);
                return new storyActions.PublishStorySuccess(baseResponse.data);
              }
              else
              {
                this.notifier.notify(Contants.Error, baseResponse.message);
                return new storyActions.PublishStoryFailure(baseResponse.message)
              }

            }),
            catchError(err => {
              this.notifier.notify(Contants.Error,`${err}`);
                return of(new storyActions.PublishStoryFailure(err))
            })
          )
        )
      );


    @Effect()
    getStories$: Observable<Action> = this.actions$.pipe(
        ofType(storyActions.StoryActionTypes.GetStories),
        mergeMap(() =>
          this.storyService.getStories().pipe(
            map((baseResponse: ApiBaseResponse<StoryListResponse>) => {

              if(baseResponse.status)
              {
                this.notifier.notify(Contants.Success,'Stories Loaded successful');
                this.router.navigate(['user/stories']);
                return new storyActions.GetStoriesSuccess(baseResponse.data);
              }
              else
              {
                this.notifier.notify(Contants.Error, baseResponse.message);
                return new storyActions.GetStoriesFailure(baseResponse.message)
              }

            }),
            catchError(err => {
              this.notifier.notify(Contants.Error,`${err}`);
                return of(new storyActions.GetStoriesFailure(err))
            })
          )
        )
      );


    @Effect()
    getDrafts$: Observable<Action> = this.actions$.pipe(
        ofType(storyActions.StoryActionTypes.GetDrafts),
        mergeMap(() =>
          this.storyService.getDrafts().pipe(
            map((baseResponse: ApiBaseResponse<StoryListResponse>) => {

              if(baseResponse.status)
              {
                this.notifier.notify(Contants.Success,'Drafts Loaded successful');
                this.router.navigate(['user/dashboard']);
                return new storyActions.GetDraftsSuccess(baseResponse.data);
              }
              else
              {
                this.notifier.notify(Contants.Error, baseResponse.message);
                return new storyActions.GetDraftsFailure(baseResponse.message)
              }

            }),
            catchError(err => {
              this.notifier.notify(Contants.Error,`${err}`);
                return of(new storyActions.GetDraftsFailure(err))
            })
          )
        )
      );


    @Effect()
    toggleVoteStory$: Observable<Action> = this.actions$.pipe(
        ofType(storyActions.StoryActionTypes.ToggleVoteStory),
        map((action: storyActions.ToggleVoteStory) => (action.payload)),
        mergeMap((storyID: string) =>
          this.storyService.toggleVoteStory(storyID).pipe(
            map((baseResponse: ApiBaseResponse<ToggleVoteResponse>) => {

              if(baseResponse.status)
              {
                this.notifier.notify(Contants.Success,'Operation Successful');
                // this.router.navigate(['user/dashboard']);
                return new storyActions.ToggleVoteStorySuccess(baseResponse.data);
              }
              else
              {
                this.notifier.notify(Contants.Error, baseResponse.message);
                return new storyActions.ToggleVoteStoryFailure(baseResponse.message)
              }

            }),
            catchError(err => {
              this.notifier.notify(Contants.Error,`${err}`);
                return of(new storyActions.ToggleVoteStoryFailure(err))
            })
          )
        )
      );


      // @Effect()
      // getStory$: Observable<Action> = this.actions$.pipe(
      //     ofType(storyActions.StoryActionTypes.GetStory),
      //     mergeMap((action: storyActions.GetStory) => this.storyService.getStory(action.payload['userId'], action.payload['storyId'])
      //     .pipe(
      //         map(story => new storyActions.GetStorySuccess(story)),
      //           catchError(err => {
      //             this.notifier.notify(Contants.Error,`${err}`);
      //             return of(new storyActions.GetStoryFailure(err))
      //         })
      //     )
      //   )
      // );

      // @Effect()
      // getStories$: Observable<Action> = this.actions$.pipe(
      //     ofType(storyActions.StoryActionTypes.GetStories),
      //     mergeMap((action: storyActions.GetStories) => this.storyService.getStories(action.payload)
      //     .pipe(
      //         map(stories => new storyActions.GetStoriesSuccess(stories)),
      //           catchError(err => {
      //             this.notifier.notify(Contants.Error,`${err}`);
      //             return of(new storyActions.GetStoriesFailure(err))
      //         })
      //     )
      //   )
      // );


}
