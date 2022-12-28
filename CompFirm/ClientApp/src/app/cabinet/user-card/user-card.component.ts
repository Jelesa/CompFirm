import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from "../../shared/auth-http.service";
import { UserInfo } from "../../home/models/user-info.model";
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-card',
  templateUrl: './user-card.component.html',
  styleUrls: ['./user-card.component.scss']
})
export class UserCardComponent implements OnInit {

  userInfo: UserInfo;
  isLoading: boolean = true;

  constructor(
    private router: Router,
    private authHttpService: AuthHttpService) { }

  ngOnInit() {
    this.authHttpService.getUserInfo()
      .subscribe(res => {
        this.userInfo = res;
        this.isLoading = false;
      });
  }

  getUserFio() {
    return `${this.userInfo.surname} ${this.userInfo.name} ${this.userInfo.patronymic}`.trim().replace('  ', ' ');
  }

  navigateToEditUserCard() {
    this.router.navigate([`cabinet/edit-user-card`]);
  }

  goToAdministration() {
    this.router.navigate([`administration`]);
  }

}
