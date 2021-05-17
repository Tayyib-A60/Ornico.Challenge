import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import * as userReducer from '../../state/user.reducer';
import * as userActions from '../../state/user.actions';
import { Store } from '@ngrx/store';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  returnUrl: string;
  user = {
    email: '',
    password: ''
  };
  constructor(private store: Store<userReducer.UserState>) { }

  ngOnInit() {
    this.initializeForm();
  }
  private initializeForm() {
    const email = '';
    const password = '';
    this.loginForm = new FormGroup({
      email: new FormControl(email, [Validators.required, Validators.email]),
      password: new FormControl(password, [Validators.required])
    });
  }

  login() {
    this.store.dispatch(new userActions.LoginUser(this.loginForm.value));
  }

}
