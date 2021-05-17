import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { StoryEffects } from './state/story.effects';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { reducer } from './state/story.reducer';
import { CreateStoryComponent } from './components/create-story/create-story.component';
import { MDBBootstrapModule } from 'angular-bootstrap-md';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { StoriesListComponent } from './components/stories-list/stories-list.component';
import { BrowserModule } from '@angular/platform-browser';
import { AuthGuardService } from '../services/authGuard.service';
import { UserGuardService } from '../services/user-guard.service';
import { JwtModule } from '@auth0/angular-jwt';
import { AdminGuardService } from '../services/admin-guard.service';
import { DashboardComponent } from './components/dashboard/dashboard.component';

export function tokenGetter() {
    return sessionStorage.getItem('currentUser')? JSON.parse(sessionStorage.getItem('currentUser')).token: null;
}

const storyRoutes: Routes = [
    { path: 'user/create-story', component: CreateStoryComponent , canActivate: [ AdminGuardService ] },
    { path: 'user/create-story/:id', component: CreateStoryComponent , canActivate: [ AdminGuardService ] },
    { path: 'user/stories', component: StoriesListComponent, canActivate: [ AuthGuardService ] },
    { path: 'user/dashboard', component: DashboardComponent, canActivate: [ AdminGuardService ] }
  ];
@NgModule({
    imports: [
        BrowserModule,
        ReactiveFormsModule,
        FormsModule,
        RouterModule.forChild(storyRoutes),
        StoreModule.forFeature('story', reducer),
        EffectsModule.forFeature([ StoryEffects ]),
        MDBBootstrapModule.forRoot(),
        JwtModule.forRoot({
            config: {
              tokenGetter: tokenGetter
            }
        }),
    ],
    declarations: [CreateStoryComponent, StoriesListComponent, DashboardComponent]
})
export class StoryModule { }
