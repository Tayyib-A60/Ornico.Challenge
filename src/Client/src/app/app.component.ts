import { Component } from '@angular/core';
import { Router, NavigationStart, NavigationEnd,
  Event, NavigationCancel, NavigationError } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

  const jwthelper = new JwtHelperService();
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent {

  showLoadingIndicator = true;
  payload: any;
  expired: boolean;

  constructor(private router: Router) {
      this.router.events.subscribe((routerEvent: Event) => {
      if (routerEvent instanceof NavigationStart) {
        this.showLoadingIndicator = true;
      }
      if (routerEvent instanceof NavigationEnd || routerEvent instanceof NavigationCancel || routerEvent instanceof NavigationError) {
        this.showLoadingIndicator = false;
      }
    });
  }

  ngOnInit() {
      const token = sessionStorage.getItem('currentUser') ? JSON.parse(sessionStorage.getItem('currentUser')).token : null;
      this.payload = jwthelper.decodeToken(token);
      this.expired = jwthelper.isTokenExpired(token);

      if(this.expired) {
        sessionStorage.removeItem('currentUser');
        this.router.navigate(['/user/login']);
      }
  }
}
