import { StoryToUpdate } from './../../../shared/models/ApiResponse';
import { StoryResponse } from 'src/app/shared/models/ApiResponse';
import { ActivatedRoute, Params } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

import * as storyReducer from '../../state/story.reducer';
import * as storyActions from '../../state/story.actions';
import * as userActions from '../../../user/state/user.actions';
import { Store, select } from '@ngrx/store';
import { Observable } from 'rxjs';
import * as fromUser from '../../../user/state/index';
import { takeWhile } from 'rxjs/operators';
import { Story } from '../../story.model';
import * as fromStory from '../../state/index';

@Component({
  selector: 'app-create-story',
  templateUrl: './create-story.component.html',
  styleUrls: ['./create-story.component.scss']
})
export class CreateStoryComponent implements OnInit {
  storyForm: FormGroup;
  currentUser: any;
  currentUser$: Observable<any>;
  componentActive = true;
  adminUsers: any[];
  id: string;
  editMode: boolean;
  story: StoryResponse;

  constructor(private store: Store<storyReducer.StoryState>, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      this.id = params['id'];
      this.editMode = params['id'] != null;
    });

    if(this.editMode)
    {
      this.store.pipe(select(fromStory.getStory, this.id),
        takeWhile(() => this.componentActive))
        .subscribe((story: StoryResponse) => {

          this.initializeForm(story.title, story.content);
        });
    }
    else
    {
      this.initializeForm();
    }
  }

  private initializeForm(title = '', content = '') {
    console.log(this.editMode);
      this.storyForm = new FormGroup({
        title: new FormControl(title, Validators.required),
        content: new FormControl(content, Validators.required),
      });  }

  createStory() {
    if(this.editMode)
    {
      const storyToUpdate =
      {
        title:  this.storyForm.value['title'],
        content: this.storyForm.value['content'],
        storyID: this.id,
      } as StoryToUpdate;

      console.log(storyToUpdate);


      this.store.dispatch(new storyActions.UpdateStory(storyToUpdate));
      //this.storyForm.reset();
    }
    else
    {
      this.store.dispatch(new storyActions.CreateStory(this.storyForm.value));
      //this.storyForm.reset();
    }
  }

}
