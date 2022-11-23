import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthGuard } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/requests/user.service';
import { AbstractControl, FormControl } from '@angular/forms';
import { User } from 'src/interfaces/catalog/User';
import { C } from 'src/interfaces/constants';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
})
export class LoginComponent implements OnInit {
  userFeedback : boolean = true;
  passwordFeedback : boolean = true;

  username: FormControl = new FormControl();
  password: FormControl = new FormControl();

  message? : string;

  constructor(
    private userService: UserService,
    private authService: AuthGuard,
    private route: Router
  ) {
    this.username.setValidators(LoginComponent.nonEmptyControl);
    this.password.setValidators(LoginComponent.nonEmptyControl);
  }

  ngOnInit(): void {
    this.authService.logout();
  }

  login() {
    let username = this.username.value;
    let password = this.password.value;

    if (this.validateUsername() && this.validatePassword()) {
      this.userService.authUser(username, password, authRes => {
        let msg = authRes.data as string;
        if (msg == C.keyword.OK) {
          localStorage.setItem('isLoggedIn', "true");
          localStorage.setItem('type', authRes.data2.userType);
          this.route.navigate(["/home"]);
        } else {
          this.message = "Password or Username is incorrect!";
        }
      });
    }

  }

  validateUsername() : boolean {
    let isValid = this.username.valid;
    this.userFeedback = isValid;

    return isValid;
  }

  validatePassword() : boolean {
    let isValid = this.password.valid;
    this.passwordFeedback = isValid;

    return isValid;
  }

  static nonEmptyControl(control: AbstractControl) {
    let value = control.value;

    if (value) {
      return null;
    } else {
      return {  ["value"]: "is empty!" }
    }

  }

}
