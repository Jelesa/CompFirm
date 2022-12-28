import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthHttpService } from "../../shared/auth-http.service";

@Component({
  selector: 'app-registration-form',
  templateUrl: './registration-form.component.html',
  styleUrls: [
    './registration-form.component.scss',
    '../form.scss'
  ]
})
export class RegistrationFormComponent implements OnInit {

  registrationForm: FormGroup;
  submitError: boolean;

  userCreated: boolean = false;
  errorMessage: string = '';

  constructor(private fb: FormBuilder,
    private authHttpService: AuthHttpService) { }

  ngOnInit() {
    this.registrationForm = this.fb.group({
      login: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(20), Validators.pattern(/^[A-z0-9_]+$/)]],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(20), Validators.pattern(/^[A-z0-9_]+$/)]],
      family: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(20)]],
      name: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(20)]],
      surname: ['', [Validators.minLength(4), Validators.maxLength(20)]],
      phone: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(20), Validators.pattern(/^\+7[0-9]{10}$/)]],
    })
  }


  get _login() {
    return this.registrationForm.get('login')
  }

  get _password() {
    return this.registrationForm.get('password')
  }

  get _family() {
    return this.registrationForm.get('family')
  }

  get _name() {
    return this.registrationForm.get('name')
  }

  get _phone() {
    return this.registrationForm.get('phone')
  }

  get _surname() {
    return this.registrationForm.get('surname')
  }

  submitRegistration() {
    this.submitError = false;

    if (!this.registrationForm.valid) {
      alert('Проверьте корректность заполненных данных!');

      this.submitError = true;

      return;
    }

    this.authHttpService.createUser(this.registrationForm.value)
      .subscribe(response => {
        this.userCreated = true;
      }, error => {

        console.log(error);
        console.log(error.error);
        console.log(error.error.message);

        this.errorMessage = error.error.message;
      });

  }
}
