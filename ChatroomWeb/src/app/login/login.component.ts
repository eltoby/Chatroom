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
  private serviceUrl = `${environment.apiUrl}/api/auth`;
  errorMessage: string;
  newUser: boolean;
  
  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit() {
  }

  login(form: NgForm) {
    let credentials = JSON.stringify(form.value);

    let url = `${this.serviceUrl}/login`;

    if (this.newUser)
      url = `${this.serviceUrl}/addUser`;

      this.http.post(url, credentials, {
        headers: new HttpHeaders({
          "Content-Type": "application/json"
        })
    }).subscribe(response => {
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
}
