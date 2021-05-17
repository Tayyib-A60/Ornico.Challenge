import { AuthGuardService } from './../services/authGuard.service';
import { JwtModule } from '@auth0/angular-jwt';
import { MDBBootstrapModule } from 'angular-bootstrap-md';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { reducer } from './state/user.reducer';
import { UserEffects } from './state/user.effects';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { BrowserModule } from '@angular/platform-browser';
import { VerifyComponent } from './components/verify/verify.component';

export function tokenGetter() {
    return sessionStorage.getItem('currentUser')? JSON.parse(sessionStorage.getItem('currentUser')).token: null;
}

const userRoutes: Routes = [
    {path: 'user/login', component: LoginComponent},
    {path: 'user/register', component: RegisterComponent},
    {path: 'user/verify', component: VerifyComponent, canActivate: [AuthGuardService]}
];

@NgModule({
    imports: [
        BrowserModule,
        FormsModule,
        ReactiveFormsModule,
        MDBBootstrapModule.forRoot(),
        RouterModule.forChild(userRoutes),
        StoreModule.forFeature('user', reducer),
        EffectsModule.forFeature([ UserEffects ]),
        NgSelectModule,
        JwtModule.forRoot({
            config: {
              tokenGetter: tokenGetter
            }
        }),
    ],
    declarations: [
        LoginComponent,
        RegisterComponent,
        VerifyComponent
    ]
})
export class UserModule { }
