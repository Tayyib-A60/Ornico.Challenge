import { EmailVerificationDTO, EmailVerificationResponse } from './../shared/models/ApiResponse';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { User, UserToLogin } from './user.model';
import { throwError } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { NotifierService } from 'angular-notifier';

@Injectable({providedIn: 'root'})
export class UserService {

url = environment.url;
token: string;

    constructor(private http: HttpClient,private router: Router, private notifier: NotifierService) {
        // this.token = sessionStorage.getItem('currentUser')? JSON.parse(sessionStorage.getItem('currentUser')).token : null;
    }

    createUser(user: User) {
        const headers = new HttpHeaders({ 'Content-Type': 'application/json'});
        return this.http.post(`${this.url}auth/sign-up`, user, { headers })
        .pipe(
            tap(data => data),
            catchError(this.handleError)
        );
    }

    loginUser(user: UserToLogin) {
      const headers = new HttpHeaders({ 'Content-Type': 'application/json'});
        return this.http.post(`${this.url}auth/login`, user, { headers })
        .pipe(
            tap(data => data ),
            catchError(this.handleError)
        );
    }

    verifyUserEmail(emailVerification: EmailVerificationDTO) {
      this.token = sessionStorage.getItem('currentUser')? JSON.parse(sessionStorage.getItem('currentUser')).token : null;
      const headers = new HttpHeaders({ 'Content-Type': 'application/json', 'Authorization': `Bearer ${this.token}` });
        return this.http.post(`${this.url}auth/verify-account`, emailVerification, { headers })
        .pipe(
            tap(data => data ),
            catchError(this.handleError)
        );
    }

    getAdminUsers(userId: number) {
      this.token = sessionStorage.getItem('currentUser')? JSON.parse(sessionStorage.getItem('currentUser')).token : null;
      const headers = new HttpHeaders({ 'Content-Type': 'application/json', 'Authorization': `Bearer ${this.token}` });
        return this.http.get(`${this.url}user/getAdminUsers/${userId}`, { headers })
        .pipe(
            tap(data => data),
            catchError(this.handleError)
        );
    }

    logout() {
      sessionStorage.removeItem('currentUser');
      this.router.navigate(['/user/login']);
      this.notifier.notify('success', 'Logged out');
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
