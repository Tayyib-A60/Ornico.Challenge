import { StoryResponse } from './../../../shared/models/ApiResponse';
import { Component, OnInit } from '@angular/core';
import { Story } from '../../story.model';

import * as fromStory from '../../state/index';
import * as storyActions from '../../state/story.actions';
import * as storyReducer from '../../state/story.reducer';
import { Store, select } from '@ngrx/store';
import { takeWhile } from 'rxjs/operators';
import { AdminGuardService } from '../../../services/admin-guard.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-stories-list',
  templateUrl: './stories-list.component.html',
  styleUrls: ['./stories-list.component.scss']
})
export class StoriesListComponent implements OnInit {
  stories: StoryResponse[] = [];
  currentUser: any;
  componentActive = true;
  storyForm: FormGroup;
  adminComment: string = '';
  isVerified: boolean;

  constructor(private store: Store<storyReducer.StoryState>,
              private adminAuthGuard: AdminGuardService) { }

  ngOnInit() {
    this.store.dispatch(new storyActions.GetStories());

    this.store.pipe(select(fromStory.getStories),
    takeWhile(() => this.componentActive))
    .subscribe(stories => {
      console.log(stories);

      this.stories = stories;
    });

    this.isVerified = this.adminAuthGuard.isVerified();
  }


  isAdmin() {
    return this.adminAuthGuard.isAdmin();
  }

  toggleVote(story: StoryResponse) {
    this.store.dispatch(new storyActions.ToggleVoteStory(story.id));

    this.store.pipe(select(fromStory.getStories),
    takeWhile(() => this.componentActive))
    .subscribe(stories => {
      console.log(stories);

      this.stories = stories;
    });
  }

  reject(story: Story) {
    const storyToReview = {
      ...story,
      approved: false,
      adminComment: this.adminComment
    };
    // this.store.dispatch(new storyActions.ReviewStory(storyToReview));
    this.adminComment = '';
  }
}
