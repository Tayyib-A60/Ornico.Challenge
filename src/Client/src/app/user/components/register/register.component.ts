import { NotifierService } from 'angular-notifier';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { User } from '../../user.model';

import * as userReducer from '../../state/user.reducer';
import * as userActions from '../../state/user.actions';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css', './register.component.scss']
})
export class RegisterComponent implements OnInit {
  registrationForm: FormGroup;
  user: User;
  roles = [
    "Reader",
    "Author"
  ];
  role: string;

  constructor(private store: Store<userReducer.UserState>) { }

  ngOnInit() {
    this.initializeForm();
  }
  private initializeForm() {
    const fullName = '';
    const twitter = '';
    const instagram = '';
    const email = '';
    const password = '';
    const role = null;
    this.registrationForm = new FormGroup({
      'fullName': new FormControl(fullName, [Validators.required]),
      'email': new FormControl(email, [Validators.required, Validators.email]),
      'twitter': new FormControl(twitter, [Validators.required]),
      'instagram': new FormControl(instagram, [Validators.required]),
      'password': new FormControl(password, [Validators.required, Validators.minLength(8)]),
      'role': new FormControl(role, [Validators.required, Validators.min(1), Validators.max(2)])
    });
  }

  register() {
    this.registrationForm.get('role').patchValue(Number(this.registrationForm.get('role').value));
    this.store.dispatch(new userActions.CreateUser(this.registrationForm.value))
  }

}
