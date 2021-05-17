import { Router } from '@angular/router';
import { StoryResponse } from './../../../shared/models/ApiResponse';
import { GetDrafts } from './../../state/story.actions';
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
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  stories: StoryResponse[] = [];
  currentUser: any;
  componentActive = true;
  storyForm: FormGroup;
  adminComment: string = '';

  constructor(private store: Store<storyReducer.StoryState>,
              private adminAuthGuard: AdminGuardService,
              private router: Router) { }

  ngOnInit() {
    // this.initializeForm();
    this.store.dispatch(new storyActions.GetDrafts());

    this.store.pipe(select(fromStory.getDrafts),
    takeWhile(() => this.componentActive))
    .subscribe(stories => {
      console.log(stories);

      this.stories = stories;
    });
  }

  publish(story: StoryResponse) {
    this.store.dispatch(new storyActions.PublishStory(story.id));
  }

  edit(storyResponse: StoryResponse) {
    this.router.navigate([`user/create-story/${storyResponse.id}`]);
  }

  delete(story: StoryResponse) {
    if(confirm('Continue delete')) {
      this.store.dispatch(new storyActions.DeleteStory(story.id));
    }
  }

}
