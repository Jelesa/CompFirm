import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AuthHttpService } from '../../shared/auth-http.service';
import { UpdateUserRequest } from "../../home/models/user-info.model";

@Component({
  selector: 'app-edit-user-card',
  templateUrl: './edit-user-card.component.html',
  styleUrls: ['./edit-user-card.component.scss']
})
export class EditUserCardComponent implements OnInit {

  userInfo: UpdateUserRequest;

  editUserForm: FormGroup;

  confirmClicked: boolean = false;
  edited: boolean = false;
  error: boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private authHttpService: AuthHttpService) { }

  ngOnInit(): void {
    this.editUserForm = this.formBuilder.group({
      surname: '',
      name: '',
      patronymic: '',
      password: '',
      newPassword: '',
    });

    this.authHttpService.getUserInfo()
      .subscribe(res => {
        this.userInfo = res;

        console.log(this.userInfo);

        this.editUserForm.setValue({
          surname: res.surname,
          name: res.name,
          patronymic: res.patronymic,
          password: '',
          newPassword: '',
        });
      });
  }

  confirm() {

    this.confirmClicked = true;
    this.error = false;

    this.authHttpService.updateUser(this.editUserForm.value)
      .subscribe(_ => {
        console.log('update');
        this.edited = true;
        setTimeout(_ => {
          location.reload();
        }, 1000)


        //setTimeout(_ => {
        //  this.router.navigate(['cabinet']);
        //}, 1000)

      }, error => {
        console.error(error);
        this.confirmClicked = false;
        this.error = true;
      });
  }

  cancel() {
    this.router.navigate([`cabinet`]);
  }

}
