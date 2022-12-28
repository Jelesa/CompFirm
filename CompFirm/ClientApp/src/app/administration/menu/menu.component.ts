import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from "../../shared/auth-http.service";
import { UserInfo } from "../../home/models/user-info.model";
import { Router } from '@angular/router';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {

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

  goToCabinet() {
    this.router.navigate([`cabinet`]);
  }

  goToRequests() {
    this.router.navigate([`administration/request-list`]);
  }

  goToReceipts() {
    this.router.navigate([`administration/products-moving/receipts`]);
  }

  goToRefunds() {
    this.router.navigate([`administration/products-moving/refunds`]);
  }

  goToGivings() {
    this.router.navigate([`administration/products-moving/givings`]);
  }

  goToManufacturers() {
    this.router.navigate([`administration/manufacturer-list`]);
  }

  goToCharacteristics() {
    this.router.navigate([`administration/characteristic-list`]);
  }

  goToProductList() {
    this.router.navigate([`administration/product-list`]);
  }

  goToGroupList() {
    this.router.navigate([`administration/group-list`]);
  }

  goToReports() {
    this.router.navigate([`administration/reports`]);
  }
}
