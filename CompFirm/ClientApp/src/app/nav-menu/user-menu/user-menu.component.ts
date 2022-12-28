import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../shared/auth.service';

@Component({
  selector: 'app-user-menu',
  templateUrl: './user-menu.component.html',
  styleUrls: ['./user-menu.component.scss']
})
export class UserMenuComponent implements OnInit {

  constructor(
    private authService: AuthService,
    private router: Router) { }

  ngOnInit(): void {
  }

  isLoggedIn(): boolean {
    return this.authService.isUserLoggedIn();
  }

  logOut() {
    this.authService.logOut();

    location.reload();
  }

  goToLogin() {
    let url = `/auth/login?returnUrl=${this.router.url}`;

    console.log(url);

    this.router.navigate(['/auth/login'], { queryParams: { returnUrl: this.router.url } });
  }
}
