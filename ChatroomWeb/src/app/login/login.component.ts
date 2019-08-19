import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../api/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  errorMessage: string;
  newUser: boolean;
  
  constructor(private authService:AuthService, private router: Router) { }

  ngOnInit() {
  }

  login(form: NgForm) {
    let credentials = JSON.stringify(form.value);

    let action: Observable<any>;

    if (this.newUser)
      action = this.authService.signUp(credentials);
    else
      action = this.authService.login(credentials);

    action.subscribe(response => {
      let token = (<any>response).token;
      let user = (<any>response).user;
      localStorage.setItem("jwt", token);
      localStorage.setItem("user", user)
      this.errorMessage = "";
      this.router.navigate(["/"]);
    }, err => {
      this.errorMessage = err.error;
    });
  }

  logOut() {
    localStorage.removeItem("jwt");
  }

  getButtonAction(): string
  {
    if (this.newUser)
      return "Sign Up";
    else
      return "Sign In";
  }
}
