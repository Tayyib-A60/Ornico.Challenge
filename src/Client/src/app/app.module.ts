import { AdminGuardService } from './services/admin-guard.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { MDBBootstrapModule } from 'angular-bootstrap-md';
import { NotifierModule } from 'angular-notifier';

import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavigationComponent } from './navigation/navigation.component';
import { HttpClientModule } from '@angular/common/http';
import { AuthGuardService } from './services/authGuard.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PaginationComponent } from './shared/pagination/pagination.component';
import { JwtModule } from '@auth0/angular-jwt';
import { BrowserModule } from '@angular/platform-browser';
import { StoreModule } from '@ngrx/store';
import { StoryModule } from './story/story.module';
import { UserModule } from './user/user.module';
import { EffectsModule } from '@ngrx/effects';
import { environment } from 'src/environments/environment';

export function tokenGetter() {
  return sessionStorage.getItem('currentUser')? JSON.parse(sessionStorage.getItem('currentUser')).token: null;
}

@NgModule({
  declarations: [
    AppComponent,
    NavigationComponent,
    PaginationComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    StoryModule,
    UserModule,
    StoreModule.forRoot({}),
    StoreDevtoolsModule.instrument({
      name: 'Story',
      maxAge: 25,
      logOnly: environment.production
    }),
    EffectsModule.forRoot([]),
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter
      }
    }),
    NotifierModule.withConfig({
      position: {
        horizontal: {
          position: 'right',
          distance: 25
        },
        vertical: {
          position: 'top',
          distance: 12,
          gap: 10
        }
      }
    }),
    MDBBootstrapModule.forRoot()
  ],
  providers: [ AuthGuardService, AdminGuardService],
  bootstrap: [AppComponent]
})
export class AppModule {
}
