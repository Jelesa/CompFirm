import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../shared/auth.service';
import { AuthHttpService } from "../../shared/auth-http.service";

@Component({
  selector: 'app-auth-form',
  templateUrl: './auth-form.component.html',
  styleUrls: [
    './auth-form.component.scss',
    '../form.scss'
  ]
})
export class AuthFormComponent implements OnInit {

  authForm: FormGroup;
  submitError: boolean;

  loggedIn: boolean = false;
  authFailed: boolean = false;
  errorMessage: string = '';
  returnUrl: string;

  constructor(private fb: FormBuilder,
    private authService: AuthService,
    private authHttpService: AuthHttpService,
    private activatedRoute: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {
    this.authForm = this.fb.group({
      login: ['', [Validators.required]],
      password: ['', [Validators.required]]
    });

    this.activatedRoute.queryParams.subscribe(params => {
      this.returnUrl = !!params['returnUrl'] ? params['returnUrl'] : '';

      this.authFailed = !!params['authFailed'] && params['authFailed'] == 'true';
      console.log(this.returnUrl);
    })
  }

  get _login() {
    return this.authForm.get('login')
  }

  get _password() {
    return this.authForm.get('password')
  }

  submitLogin() {
    this.submitError = false;

    if (!this.authForm.valid) {
      alert('Проверьте корректность заполненных данных!');

      this.submitError = true;

      return;
    }

    this.authHttpService.logIn(this.authForm.value)
      .subscribe(response => {
        this.loggedIn = true;
        this.authService.setUserCredentials(response.token);

        setTimeout(_ => {
          this.router.navigate([this.returnUrl]);
        }, 3000)

      }, error => {
          if (!!error.error.message) {
            this.errorMessage = error.error.message;
          } else {
            this.errorMessage = 'Произошла ошибка. Попробуйте позже.';
          }
      });

  }

}
