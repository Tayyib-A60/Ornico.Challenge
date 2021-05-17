import { StoryToUpdate } from './../shared/models/ApiResponse';
import { StoryResponse } from 'src/app/shared/models/ApiResponse';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Story } from './story.model';
import { throwError } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';

@Injectable({providedIn: 'root'})
export class StoryService {

url = environment.url;
token: string;

    constructor(private http: HttpClient) { }

    createStory(story: Story) {
        this.token = sessionStorage.getItem('currentUser')? JSON.parse(sessionStorage.getItem('currentUser')).token : null;
        const headers = new HttpHeaders({ 'Content-Type': 'application/json', 'Authorization': `Bearer ${this.token}` });
        return this.http.post(`${this.url}story/create`, story, { headers })
        .pipe(
            tap(data => data),
            catchError(this.handleError)
        );
    }

    updateStory(story: StoryToUpdate) {
      console.log(story);

        this.token = sessionStorage.getItem('currentUser')? JSON.parse(sessionStorage.getItem('currentUser')).token : null;
        const headers = new HttpHeaders({ 'Content-Type': 'application/json', 'Authorization': `Bearer ${this.token}` });
        return this.http.post(`${this.url}story/update`, story, { headers })
        .pipe(
            tap(data => data),
            catchError(this.handleError)
        );
    }

    deleteStory(storyID: string) {
        this.token = sessionStorage.getItem('currentUser')? JSON.parse(sessionStorage.getItem('currentUser')).token : null;
        const headers = new HttpHeaders({ 'Content-Type': 'application/json', 'Authorization': `Bearer ${this.token}` });
        return this.http.post(`${this.url}story/delete/${storyID}`, null, { headers })
        .pipe(
            tap(data => data),
            catchError(this.handleError)
        );
    }

    getStory(userId: number, storyId: number) {
        this.token = sessionStorage.getItem('currentUser')? JSON.parse(sessionStorage.getItem('currentUser')).token : null;
        const headers = new HttpHeaders({ 'Content-Type': 'application/json', 'Authorization': `Bearer ${this.token}` });
        return this.http.get(`${this.url}story/getStory/${userId}/${storyId}`, { headers })
        .pipe(
            tap(data => data),
            catchError(this.handleError)
        );
    }

    getStories() {
        this.token = sessionStorage.getItem('currentUser')? JSON.parse(sessionStorage.getItem('currentUser')).token : null;
        const headers = new HttpHeaders({ 'Content-Type': 'application/json', 'Authorization': `Bearer ${this.token}` });
        return this.http.post(`${this.url}story/get-stories`, null, { headers })
        .pipe(
            tap(data => data),
            catchError(this.handleError)
        );
    }

    getDrafts() {
      this.token = sessionStorage.getItem('currentUser')? JSON.parse(sessionStorage.getItem('currentUser')).token : null;
      const headers = new HttpHeaders({ 'Content-Type': 'application/json', 'Authorization': `Bearer ${this.token}` });
      return this.http.post(`${this.url}story/get-drafts`, null, { headers })
      .pipe(
          tap(data => data),
          catchError(this.handleError)
      );
  }

    // reviewStory(userId: number, story: Story) {
    //     this.token = sessionStorage.getItem('currentUser')? JSON.parse(sessionStorage.getItem('currentUser')).token : null;
    //     const headers = new HttpHeaders({ 'Content-Type': 'application/json', 'Authorization': `Bearer ${this.token}` });
    //     return this.http.post(`${this.url}story/reviewStory/${userId}`, story, { headers })
    //     .pipe(
    //         tap(data => data),
    //         catchError(this.handleError)
    //     );
    // }

    publishStory(storyID: string) {
        this.token = sessionStorage.getItem('currentUser')? JSON.parse(sessionStorage.getItem('currentUser')).token : null;
        const headers = new HttpHeaders({ 'Content-Type': 'application/json', 'Authorization': `Bearer ${this.token}` });
        return this.http.post(`${this.url}story/publish/${storyID}`, null, { headers })
        .pipe(
            tap(data => data),
            catchError(this.handleError)
        );
    }

    toggleVoteStory(storyID: string) {
        this.token = sessionStorage.getItem('currentUser')? JSON.parse(sessionStorage.getItem('currentUser')).token : null;
        const headers = new HttpHeaders({ 'Content-Type': 'application/json', 'Authorization': `Bearer ${this.token}` });
        return this.http.post(`${this.url}story/toggle-vote/${storyID}`, null, { headers })
        .pipe(
            tap(data => data),
            catchError(this.handleError)
        );
    }


    private handleError(err) {
        let errorMessage: string;
        if (err.error instanceof ErrorEvent) {
          errorMessage = `An error occurred: ${err.error.message}`;
        } else {
          errorMessage = `${err.error}`;
        }
        return throwError(errorMessage);
      }
}
