import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders }   from '@angular/common/http';
import { NgForm } from '@angular/forms';
import { environment } from '../../environments/environment'
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  private serviceUrl = `${environment.apiUrl}/api/auth/login`;
  invalidLogin: boolean;
  
  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit() {
  }

  login(form: NgForm) {
    let credentials = JSON.stringify(form.value);
    this.http.post(this.serviceUrl, credentials, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      let token = (<any>response).token;
      let user = (<any>response).user;
      localStorage.setItem("jwt", token);
      localStorage.setItem("user", user)
      this.invalidLogin = false;
      this.router.navigate(["/"]);
    }, err => {
      this.invalidLogin = true;
    });
  }

  logOut() {
    localStorage.removeItem("jwt");
  }
}
