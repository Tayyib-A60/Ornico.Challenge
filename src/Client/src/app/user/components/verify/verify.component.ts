import { EmailVerificationDTO } from './../../../shared/models/ApiResponse';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import * as userReducer from '../../state/user.reducer';
import * as userActions from '../../state/user.actions';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-verify',
  templateUrl: './verify.component.html',
  styleUrls: ['./verify.component.scss']
})
export class VerifyComponent implements OnInit {

  code: string;

  constructor(private store: Store<userReducer.UserState>) { }

  ngOnInit() {

  }

  verify() {

    const emailVerification = { code: this.code } as EmailVerificationDTO;

    this.store.dispatch(new userActions.VerifyUserEmail(emailVerification));
  }

}
