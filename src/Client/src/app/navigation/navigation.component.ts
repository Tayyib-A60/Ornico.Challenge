import { AdminGuardService } from '../services/admin-guard.service';
import { Component, OnInit } from '@angular/core';
import { AuthGuardService } from '../services/authGuard.service';
import { Router, ActivatedRoute } from '@angular/router';
import * as userActions from '../user/state/user.actions';
import * as userReducer from '../user/state/user.reducer';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent implements OnInit {
  searchString = '';
    constructor(private authGuardService: AuthGuardService,
                private router: Router, private route: ActivatedRoute,
                private adminGuard: AdminGuardService,
                private store: Store<userReducer.UserState>) {
  }

 isAuthenticated(): boolean {
   return this.authGuardService.isAuthenticated();
 }

 isAuthor(): boolean {
   return this.authGuardService.isAuthor();
 }

 isAdmin(): boolean {
   return this.adminGuard.isAdmin();
 }
 ngOnInit() {
  //  this.vehicleResolver.searchString = this.searchString;
 }

 logout() {
   this.store.dispatch(new userActions.LogoutUser());
   this.router.navigate(['/user/login']);
 }

 isVerified() {
   return this.adminGuard.isVerified();
 }

}
